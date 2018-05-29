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
            if (!IsDisposed)
            {
                NativePipelineState.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void Initialize(PipelineStateDescription description)
        {
            var pipelineStateDescription = new GraphicsPipelineStateDescription()
            {
                // From mesh
                InputLayout = FromVertexLayout(description.InputLayout),

                // From effect
                RootSignature = description.RootSignature.NativeRootSignature,
                VertexShader = new SharpDX.Direct3D12.ShaderBytecode(description.VertexShader),
                PixelShader = new SharpDX.Direct3D12.ShaderBytecode(description.PixelShader),

                // Common
                RasterizerState = RasterizerStateDescription.Default(),
                BlendState = BlendStateDescription.Default(),
                DepthStencilFormat = Format.D32_Float,
                DepthStencilState = SharpDX.Direct3D12.DepthStencilStateDescription.Default(),
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = PipelineStateFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                StreamOutput = new StreamOutputDescription()
            };
            pipelineStateDescription.RenderTargetFormats[0] = Format.B8G8R8A8_UNorm;
            NativePipelineState = Device.NativeDevice.CreateGraphicsPipelineState(pipelineStateDescription);
        }

        public static InputLayoutDescription FromVertexLayout(VertexLayout layout)
        {
            InputElement[] elements = new InputElement[layout.Elements.Count];

            for (int i = 0; i < layout.Elements.Count; i++)
            {
                var element = layout.Elements[i];
                elements[i] = new InputElement(element.Name, 0, VertexLayout.GetFormat(element.Format), element.Offset, 0);
            }

            return new InputLayoutDescription(elements);
        }

        public static PipelineState Create(GraphicsDevice device, PipelineStateDescription description)
        {
            var pipelineState = new PipelineState(device);
            pipelineState.Initialize(description);
            return pipelineState;
        }
    }
}
