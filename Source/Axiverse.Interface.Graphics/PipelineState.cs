using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DXGI;

using NativePipelineState = SharpDX.Direct3D12.PipelineState;

namespace Axiverse.Interface.Graphics
{
    public class PipelineState : GraphicsResource
    {
        internal NativePipelineState NativePipelineState;

        public PrimitiveType PrimitiveType = PrimitiveType.TriangleList;

        protected PipelineState(GraphicsDevice device) : base(device)
        {

        }

        protected override void Dispose(bool disposing)
        {
            NativePipelineState.Dispose();
            base.Dispose(disposing);
        }

        protected void Initialize(PipelineStateDescription description)
        {
            var pipelineStateDescription = new SharpDX.Direct3D12.GraphicsPipelineStateDescription()
            {
                // From mesh
                InputLayout = description.InputLayout,

                // From effect
                RootSignature = description.RootSignature.NativeRootSignature,
                VertexShader = new SharpDX.Direct3D12.ShaderBytecode(description.VertexShader),
                PixelShader = new SharpDX.Direct3D12.ShaderBytecode(description.PixelShader),

                // Common
                RasterizerState = SharpDX.Direct3D12.RasterizerStateDescription.Default(),
                BlendState = SharpDX.Direct3D12.BlendStateDescription.Default(),
                DepthStencilFormat = SharpDX.DXGI.Format.D32_Float,
                DepthStencilState = new SharpDX.Direct3D12.DepthStencilStateDescription() { IsDepthEnabled = false, IsStencilEnabled = false },
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = SharpDX.Direct3D12.PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = SharpDX.Direct3D12.PipelineStateFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                StreamOutput = new SharpDX.Direct3D12.StreamOutputDescription()
            };
            pipelineStateDescription.RenderTargetFormats[0] = Format.B8G8R8A8_UNorm;
            NativePipelineState = Device.NativeDevice.CreateGraphicsPipelineState(pipelineStateDescription);
        }

        public static PipelineState Create(GraphicsDevice device, PipelineStateDescription description)
        {
            var pipelineState = new PipelineState(device);
            pipelineState.Initialize(description);
            return pipelineState;
        }
    }
}
