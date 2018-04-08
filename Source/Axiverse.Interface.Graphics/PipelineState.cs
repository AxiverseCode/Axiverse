using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    public class PipelineState : GraphicsResource
    {
        public SharpDX.Direct3D12.PipelineState NativePipelineState;

        public PrimitiveType PrimitiveType = PrimitiveType.TriangleList;

        protected PipelineState(GraphicsDevice device) : base(device)
        {

        }

        protected void Initialize(PipelineStateDescription description)
        {
            var psoDesc = new SharpDX.Direct3D12.GraphicsPipelineStateDescription()
            {
                InputLayout = description.InputLayout,
                RootSignature = description.RootSignature.NativeRootSignature,
                VertexShader = new SharpDX.Direct3D12.ShaderBytecode(description.VertexShader),
                PixelShader = new SharpDX.Direct3D12.ShaderBytecode(description.PixelShader),
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
            psoDesc.RenderTargetFormats[0] = SharpDX.DXGI.Format.R8G8B8A8_UNorm;
            NativePipelineState = Device.NativeDevice.CreateGraphicsPipelineState(psoDesc);
        }

        public static PipelineState Create(GraphicsDevice device, PipelineStateDescription description)
        {
            var pipelineState = new PipelineState(device);
            pipelineState.Initialize(description);
            return pipelineState;
        }
    }
}
