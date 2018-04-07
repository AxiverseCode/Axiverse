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
                Text        = "Axiverse | HelloGraphics",
            };
            form.Show();

            // Init the rendering device
            GraphicsDevice device = new GraphicsDevice();
            device.Init();
            // Init a swap chain
            SwapChain chain = new SwapChain(device);
            chain.Initialize(form);
            // Init a graphics context
            Axiverse.Interface.Graphics.CommandList context = new Axiverse.Interface.Graphics.CommandList(device);
            context.Initialize(SwapChain.BufferCount);

            // NOTE: I think we could work with prebaked root signatures (we can define it
            // as an HLSL shader and then use it for all of our PSOs. 
            // Root signature
            var rootSignatureDesc = new SharpDX.Direct3D12.RootSignatureDescription(SharpDX.Direct3D12.RootSignatureFlags.AllowInputAssemblerInputLayout);
            var rootSignature = device.NativeDevice.CreateRootSignature(rootSignatureDesc.Serialize());
            
            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new SharpDX.Direct3D12.InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0,0)
            };

            // Shaders
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
            var vbyte = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0");
            var pbyte = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0");

            var psDesc = new PipelineStateDescription()
            {
                InputLayout = new SharpDX.Direct3D12.InputLayoutDescription(inputElementDescs),
                RootSignature = rootSignature,
                VertexShader = vbyte,
                PixelShader = pbyte,
            };

            var pipelineState = new PipelineState(device);
            pipelineState.Initialize(psDesc);

            // Lets create some resources
            int[] indices = new int[] { 0, 2, 1 };
            GraphicsBuffer indexBuff = new GraphicsBuffer(device);
            indexBuff.InitializeAsIndexBuffer
            (
                context.GetNativeContext(), 
                Utilities.SizeOf(indices),
                Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0), 
                false
            );

            float[] vertices = new float[] { 0.0f, 0.25f, 0.0f, -0.25f, 0.0f, 0.0f, 0.25f, 0.0f, 0.0f };
            GraphicsBuffer vtxBuffer = new GraphicsBuffer(device);
            vtxBuffer.InitializeAsVertexBuffer
            ( 
                context.GetNativeContext(), 
                Utilities.SizeOf(vertices),
                sizeof(float) * 3,
                Marshal.UnsafeAddrOfPinnedArrayElement(vertices, 0), 
                false
            );

            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var backBuffer = chain.StartFrame();
                    var backBufferHandle = chain.GetCurrentColorHandle();
                    context.Reset(chain);

                    context.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);
                    {
                        context.SetColorTarget(backBufferHandle);
                        context.SetViewport(0, 0, 1024, 720);
                        context.SetScissor(0, 0, 1024, 720);
                        context.ClearTargetColor(backBufferHandle, 1.0f, 0.0f, 1.0f, 1.0f);

                        context.GetNativeContext().SetGraphicsRootSignature(rootSignature);
                        context.GetNativeContext().PipelineState = pipelineState.NativePipelineState;
                        context.GetNativeContext().PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

                        context.SetIndexBuffer(indexBuff.NativeIndexBufferView);
                        context.SetVertexBuffer(vtxBuffer.NativeVertexBufferView);
                        context.DrawIndexed(3);
                    }
                    context.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);

                    context.Close();
                    chain.ExecuteCommandList(context.GetNativeContext());
                    context.FinishFrame(chain);
                    chain.Present();
                }
            }
        }
    }
}
