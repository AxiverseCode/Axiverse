using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Graphics.Shaders;
using Axiverse.Interface.Rendering;

namespace Axiverse.Interface.Engine
{
    public class Engine
    {
        RenderForm form;

        public SceneSystem SceneSystem { get; set; }

        public Engine()
        {
            SceneSystem = new SceneSystem();
        }

        public void Initialize()
        {
            // Create a window
            form = new RenderForm()
            {
                Width = 1024,
                Height = 720,
                Text = "Axiverse | Hello Graphics",
            };
        }

        public void Run()
        {

            form.Show();

            // Init the rendering device
            var device = GraphicsDevice.Create();
            var swapChain = SwapChain.Create(device, form);
            var depth = new Texture(device);
            depth.CreateDepth(form.ClientSize.Width, form.ClientSize.Height);
            var commandList = CommandList.Create(device);

            // Bind resources
            Injector.Global.Bind(device);

            // Shaders
            var geometryShader = new GeometryShader(device);
            geometryShader.Initialize();
            var pipelineStateDescription = new PipelineStateDescription()
            {
                InputLayout = PositionColorTexture.Layout,
                RootSignature = geometryShader.RootSignature,
                VertexShader = geometryShader.VertexShader,
                PixelShader = geometryShader.PixelShader,
            };
            var pipelineState = PipelineState.Create(device, pipelineStateDescription);

            var frame = 0.0f;
            var world = Matrix4.Identity;
            var view = Matrix4.LookAtRH(new Vector3(0, 10, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var projection = Matrix4.PerspectiveFovRH(Functions.DegreesToRadians(60.0f),
                1.0f * form.ClientSize.Width / form.ClientSize.Height,
                2.0f,
                2000.0f);

            var texture = new Texture(device);
            texture.Load(@".\Resources\Textures\Placeholder Grid.jpg");
            

            // Lets create some resources
            var cube = Primitives<PositionColorTexture>.Cube();
            var indices = cube.Item1;
            var vertices = cube.Item2;
            var indexBuffer = GraphicsBuffer.Create(device, indices, false);

            var vertexBuffer = GraphicsBuffer.Create(device, vertices, false);
            var indexBinding = new IndexBufferBinding
            {
                Buffer = indexBuffer,
                Count = indices.Length,
                Type = IndexBufferType.Integer32,
                //Offset = 0,
            };
            var vertexBinding = new VertexBufferBinding
            {
                Buffer = vertexBuffer,
                Count = vertices.Length,
                Stride = PositionColorTexture.Layout.Stride,
                //Offset = 0,
            };
            var meshDraw = new MeshDraw
            {
                IndexBuffer = indexBinding,
                VertexBuffers = new[] { vertexBinding },
                Count = indices.Length,
            };
            var mesh1 = new Mesh
            {
                Draw = meshDraw,
            };
            var mesh2 = new Mesh
            {
                Draw = meshDraw,
            };
            var meshes = new[] { mesh1, mesh2 };

            var transforms = new GeometryShader.PerObject[meshes.Length];
            transforms[0].WorldViewProjection = Matrix4.Transpose(world * view * projection);
            transforms[0].Color = new Vector4(0.4f, 0.5f, 0.8f, 1);
            transforms[1].Color = new Vector4(1f, 1f, 1f, 1);
            var constantBuffer = GraphicsBuffer.Create(device, transforms, false);

            var descriptorSets = new DescriptorSet[meshes.Length];
            int perObjectSize = Utilities.SizeOf<GeometryShader.PerObject>();
            for (int i = 0; i < descriptorSets.Length; i++)
            {
                descriptorSets[i] = new DescriptorSet(device, geometryShader.Layout);
                descriptorSets[i].SetShaderResourceView(1, texture);
                descriptorSets[i].SetSamplerState(0, SamplerState.Create(device, null));
                descriptorSets[i].SetConstantBuffer(0, constantBuffer, i * perObjectSize, perObjectSize);
            }


            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    frame += 1;
                    view = Matrix4.LookAtRH(
                        new Vector3(
                            10 * Functions.Sin(Functions.DegreesToRadians(frame / 10)),
                            4 * Functions.Sin(Functions.DegreesToRadians(frame / 30)),
                            10 * Functions.Cos(Functions.DegreesToRadians(frame / 10))),
                        new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                    mesh1.Bindings[Key.From<Matrix4>()] = Matrix4.Transpose(world * view * projection);
                    mesh2.Bindings[Key.From<Matrix4>()] = 
                        Matrix4.Transpose(
                            Matrix4.FromQuaternion(Quaternion.FromEuler(frame / 100f, frame / 747, frame / 400))
                            * view * projection);


                    transforms[0].WorldViewProjection = Matrix4.Transpose(world * view * projection);

                    constantBuffer.Write(transforms);

                    var backBuffer = swapChain.StartFrame();
                    var backBufferHandle = swapChain.GetCurrentColorHandle();
                    commandList.Reset(swapChain);

                    if (texture.UploadResource != null)
                    {
                        texture.Prepare(commandList);
                    }

                    commandList.ResourceTransition(backBuffer, ResourceState.Present, ResourceState.RenderTarget);
                    {
                        commandList.SetRenderTargets(backBufferHandle, depth.NativeDepthStencilView);
                        commandList.SetViewport(0, 0, 1024, 720);
                        commandList.SetScissor(0, 0, 1024, 720);
                        commandList.ClearDepth(depth.NativeDepthStencilView, 1.0f);
                        commandList.ClearTargetColor(backBufferHandle, 0.2f, 0.2f, 0.2f, 1.0f);

                        commandList.SetRootSignature(pipelineStateDescription.RootSignature);
                        commandList.PipelineState = pipelineState;

                        for (int i = 0; i < meshes.Length; i++)
                        {
                            transforms[i].WorldViewProjection = (Matrix4)meshes[i].Bindings[Key.From<Matrix4>()];

                            commandList.SetDescriptors(descriptorSets[i]);

                            Draw(commandList, meshDraw);
                        }
                        //commandList.SetIndexBuffer(indexBinding);
                        //commandList.SetVertexBuffer(vertexBinding, 0);
                        //commandList.SetIndexBuffer(indexBuffer, indexBuffer.Size, IndexBufferType.Integer32);
                        //commandList.SetVertexBuffer(vertexBuffer, 0, vertexBuffer.Size, PositionColorTexture.Layout.Stride);
                        //commandList.DrawIndexed(indices.Length);
                        
                    }
                    commandList.ResourceTransition(backBuffer, ResourceState.RenderTarget, ResourceState.Present);

                    var compiled = commandList.Close();
                    swapChain.ExecuteCommandList(compiled);
                    commandList.FinishFrame(swapChain);
                    swapChain.Present();
                }
            }
        }

        public void Draw(CommandList commandList, MeshDraw meshDraw)
        {
            commandList.SetIndexBuffer(meshDraw.IndexBuffer);
            for (int i = 0; i < meshDraw.VertexBuffers.Length; i++)
            {
                commandList.SetVertexBuffer(meshDraw.VertexBuffers[i], i);
            }
            commandList.DrawIndexed(meshDraw.Count);
        }

    }
}
