using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

using Axiverse.Interface.Graphics;

namespace HelloGraphics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a window
            RenderForm form = new RenderForm()
            {
                Width       = 1024,
                Height      = 720,
                BackColor   = System.Drawing.Color.Black,
                Text        = "Axiverse | Hello Graphics",
            };
            form.Show();

            // Init the rendering device
            var device = GraphicsDevice.Create();
            var swapChain = SwapChain.Create(device, form);
            var commandList = CommandList.Create(device);

            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new SharpDX.Direct3D12.InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 0)
            };

            // Shaders
            var testShaderPath = "../../../../../Resources/Engine/Test/test.hlsl";
            var pipelineStateDescription = new PipelineStateDescription()
            {
                InputLayout = new SharpDX.Direct3D12.InputLayoutDescription(inputElementDescs),
                RootSignature = RootSignature.Create(device),
                VertexShader = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0"),
                PixelShader = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0"),
            };
            var pipelineState = PipelineState.Create(device, pipelineStateDescription);

            // Lets create some resources
            var indices = new int[] { 0, 2, 1 };
            var vertices = new float[] { 0.0f, 0.25f, 0.0f, -0.25f, 0.0f, 0.0f, 0.25f, 0.0f, 0.0f };
            var indexBuffer = GraphicsBuffer.CreateIndexBuffer(device, indices, false);
            var vertexBuffer = GraphicsBuffer.CreateVertexBuffer(device, vertices, 3, false);
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
                    var backBuffer = swapChain.StartFrame();
                    var backBufferHandle = swapChain.GetCurrentColorHandle();
                    commandList.Reset(swapChain);

                    commandList.ResourceTransition(backBuffer, ResourceState.Present, ResourceState.RenderTarget);
                    {
                        commandList.SetColorTarget(backBufferHandle);
                        commandList.SetViewport(0, 0, 1024, 720);
                        commandList.SetScissor(0, 0, 1024, 720);
                        commandList.ClearTargetColor(backBufferHandle, 1.0f, 0.0f, 1.0f, 1.0f);

                        commandList.SetRootSignature(pipelineStateDescription.RootSignature);
                        commandList.PipelineState = pipelineState;
                        
                        commandList.Draw(indexBinding, vertexBinding);
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
