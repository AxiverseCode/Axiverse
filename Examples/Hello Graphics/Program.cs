using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

using Axiverse.Interface.Graphics;
using System.Runtime.InteropServices;

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
            var chain = SwapChain.Create(device, form);
            var commandList = CommandList.Create(device, SwapChain.BufferCount);
            
            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new SharpDX.Direct3D12.InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0,0)
            };

            // Shaders
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
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

            var indexBuffer = GraphicsBuffer.CreateIndexBuffer(
                device,
                commandList.GetNativeContext(), 
                Utilities.SizeOf(indices),
                Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0), 
                false);

            var vertexBuffer = GraphicsBuffer.CreateVertexBuffer(
                device,
                commandList.GetNativeContext(),
                Utilities.SizeOf(vertices),
                sizeof(float) * 3,
                Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0),
                false);

            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var backBuffer = chain.StartFrame();
                    var backBufferHandle = chain.GetCurrentColorHandle();
                    commandList.Reset(chain);

                    commandList.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);
                    {
                        commandList.SetColorTarget(backBufferHandle);
                        commandList.SetViewport(0, 0, 1024, 720);
                        commandList.SetScissor(0, 0, 1024, 720);
                        commandList.ClearTargetColor(backBufferHandle, 1.0f, 0.0f, 1.0f, 1.0f);

                        commandList.SetRootSignature(pipelineStateDescription.RootSignature);
                        commandList.PipelineState = pipelineState;
                        commandList.GetNativeContext().PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

                        commandList.SetIndexBuffer(indexBuffer);
                        commandList.SetVertexBuffer(vertexBuffer);
                        commandList.DrawIndexed(3);
                    }
                    commandList.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);

                    commandList.Close();
                    chain.ExecuteCommandList(commandList.GetNativeContext());
                    commandList.FinishFrame(chain);
                    chain.Present();
                }
            }
        }
    }
}
