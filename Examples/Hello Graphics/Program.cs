using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

using SharpDX.Direct3D12;
using SharpDX.D3DCompiler;

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
            RenderDevice device = new RenderDevice();
            device.Init();
            // Init a swap chain
            SwapChain chain = new SwapChain();
            chain.Init(form, device);
            // Init a graphics context
            RenderContext context = new RenderContext();
            context.Init(device.NativeDevice,SwapChain.BufferCount);

            // NOTE: I think we could work with prebaked root signatures (we can define it
            // as an HLSL shader and then use it for all of our PSOs. 
            // Root signature
            var rootSignatureDesc = new RootSignatureDescription(RootSignatureFlags.AllowInputAssemblerInputLayout);
            var rootSignature = device.NativeDevice.CreateRootSignature(rootSignatureDesc.Serialize());

            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0,0)
            };

            // Shaders
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
            var vbyte = SharpDX.D3DCompiler.ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0",ShaderFlags.Debug);
            var vertexShader = new SharpDX.Direct3D12.ShaderBytecode(vbyte);
            var pbyte = SharpDX.D3DCompiler.ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0");
            var pixelShader = new SharpDX.Direct3D12.ShaderBytecode(pbyte);

            // NOTE: This should be an Object 
            // Describe and create the graphics pipeline state object (PSO).
            var psoDesc = new GraphicsPipelineStateDescription()
            {
                InputLayout             = new InputLayoutDescription(inputElementDescs),
                RootSignature           = rootSignature,
                VertexShader            = vertexShader,
                PixelShader             = pixelShader,
                RasterizerState         = RasterizerStateDescription.Default(),
                BlendState              = BlendStateDescription.Default(),
                DepthStencilFormat      = SharpDX.DXGI.Format.D32_Float,
                DepthStencilState       = new DepthStencilStateDescription() { IsDepthEnabled = false, IsStencilEnabled = false },
                SampleMask              = int.MaxValue,
                PrimitiveTopologyType   = PrimitiveTopologyType.Triangle,
                RenderTargetCount       = 1,
                Flags                   = PipelineStateFlags.None,
                SampleDescription       = new SharpDX.DXGI.SampleDescription(1, 0),
                StreamOutput            = new StreamOutputDescription()
            };
            psoDesc.RenderTargetFormats[0] = SharpDX.DXGI.Format.R8G8B8A8_UNorm;
            var pipelineState = device.NativeDevice.CreateGraphicsPipelineState(psoDesc);

            // Lets create some resources
            int[] indices = new int[] { 0, 2, 1 };
            Axiverse.Interface.Graphics.Buffer indexBuff = new Axiverse.Interface.Graphics.Buffer();
            indexBuff.InitAsIndexBuffer
            (
                device.NativeDevice, 
                context.GetNativeContext(), 
                Utilities.SizeOf(indices),
                Marshal.UnsafeAddrOfPinnedArrayElement(indices, 0), 
                false
            );

            float[] vertices = new float[] { 0.0f, 0.25f, 0.0f, -0.25f, 0.0f, 0.0f, 0.25f, 0.0f, 0.0f };
            Axiverse.Interface.Graphics.Buffer vtxBuffer = new Axiverse.Interface.Graphics.Buffer();
            vtxBuffer.InitAsVertexBuffer
            (
                device.NativeDevice, 
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
                        context.GetNativeContext().PipelineState = pipelineState;
                        context.GetNativeContext().PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

                        context.SetIndexBuffer(indexBuff.AsIndexBuffer());
                        context.SetVertexBuffer(vtxBuffer.AsVertexBuffer());
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
