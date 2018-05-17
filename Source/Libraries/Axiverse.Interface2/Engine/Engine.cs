using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Graphics.Shaders;

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
            var commandList = CommandList.Create(device);

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
            var descriptorSet = new DescriptorSet(device, geometryShader.Layout);

            var frame = 0.0f;
            var world = Matrix4.Identity;
            var view = Matrix4.LookAtRH(new Vector3(0, 10, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var projection = Matrix4.PerspectiveFovRH(Functions.DegreesToRadians(60.0f),
                1.0f * form.ClientSize.Width / form.ClientSize.Height,
                2.0f,
                2000.0f);
            var transforms = new GeometryShader.PerObject[1];
            transforms[0].WorldViewProjection = Matrix4.Transpose(world * view * projection);
            transforms[0].Color = new Vector4(0.4f, 0.5f, 0.8f, 1);
            var constantBuffer = GraphicsBuffer.Create(device, transforms, false);

            var texture = new Texture(device);
            texture.Load(@"..\..\..\..\..\Resources\Textures\Placeholder Grid.jpg");

            descriptorSet.SetConstantBuffer(0, constantBuffer);
            descriptorSet.SetShaderResourceView(1, texture);
            descriptorSet.SetSamplerState(0, SamplerState.Create(device, null));

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
                Offset = 0,
            };
            var vertexBinding = new VertexBufferBinding
            {
                Buffer = vertexBuffer,
                Count = 3,
                Offset = 0,
            };


            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    frame += 1;
                    view = Matrix4.LookAtRH(
                        new Vector3(
                            10 * Functions.Sin(Functions.DegreesToRadians(frame)),
                            4 * Functions.Sin(Functions.DegreesToRadians(frame / 3)),
                            10 * Functions.Cos(Functions.DegreesToRadians(frame))),
                        new Vector3(0, 0, 0), new Vector3(0, 1, 0));
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
                        commandList.SetColorTarget(backBufferHandle);
                        commandList.SetViewport(0, 0, 1024, 720);
                        commandList.SetScissor(0, 0, 1024, 720);
                        commandList.ClearTargetColor(backBufferHandle, 0.2f, 0.2f, 0.2f, 1.0f);

                        commandList.SetRootSignature(pipelineStateDescription.RootSignature);
                        commandList.PipelineState = pipelineState;
                        commandList.SetDescriptors(descriptorSet);

                        commandList.SetIndexBuffer(indexBuffer, indexBuffer.Size, IndexBufferType.Integer32);
                        commandList.SetVertexBuffer(vertexBuffer, 0, vertexBuffer.Size, PositionColorTexture.Layout.Stride);
                        commandList.DrawIndexed(indices.Length);
                    }
                    commandList.ResourceTransition(backBuffer, ResourceState.RenderTarget, ResourceState.Present);

                    commandList.Close();
                    swapChain.ExecuteCommandList(commandList);
                    commandList.FinishFrame(swapChain);
                    swapChain.Present();
                }
            }
        }
    }
}
