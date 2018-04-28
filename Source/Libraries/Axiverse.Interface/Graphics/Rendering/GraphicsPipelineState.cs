using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Resources;
using SharpDX.DXGI;

using Axiverse.Resources;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class GraphicsPipelineState
    {
        public Renderer Renderer { get; private set; }
        public Device Device { get; private set; }

        public PipelineState PipelineState;
        public RootSignature RootSignature;

        public GraphicsPipelineState(Renderer renderer)
        {
            Renderer = renderer;
            Device = renderer.Device;
        }

        public virtual void Load(string path, InputLayoutDescription inputLayoutDescription)
        {
            // root description
            var rootParameters = new RootParameter[]
            {
                new RootParameter(ShaderVisibility.Vertex,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.ConstantBufferView,
                        BaseShaderRegister = 0,
                        OffsetInDescriptorsFromTableStart = int.MinValue,
                        DescriptorCount = 1,
                    }),
                new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.ShaderResourceView,
                        DescriptorCount = 1,
                        OffsetInDescriptorsFromTableStart = int.MinValue,
                        BaseShaderRegister = 0
                    }),
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.Sampler,
                        BaseShaderRegister = 0,
                        DescriptorCount = 1
                    }),
            };

            var rootSignatureDescription = new RootSignatureDescription(RootSignatureFlags.AllowInputAssemblerInputLayout, rootParameters);
            RootSignature = Renderer.Device.CreateRootSignature(rootSignatureDescription.Serialize());
            
            // pixel and vertex shaders
            var shaderFlags = SharpDX.D3DCompiler.ShaderFlags.Debug;
            byte[] shaderBuffer;

            using (var fileStream = Store.Default.Open(path, FileMode.Open))
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                var shaderString = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                var preamble = Encoding.UTF8.GetPreamble();
                var buffer = memoryStream.GetBuffer();
                shaderBuffer = memoryStream.ToArray();

                // TODO: Remove byte order mark if it exists
                //if (shaderString.StartsWith(Encoding.UTF8.GetString(preamble)))
                //{
                //    shaderBuffer = buffer.Skip(preamble.Length).Take(buffer.Length - preamble.Length).ToArray();
                //}
                //else
                //{
                //    shaderBuffer = memoryStream.ToArray();
                //}
            }
            var vertexBytecode = SharpDX.D3DCompiler.ShaderBytecode.Compile(shaderBuffer, "VSMain", "vs_5_0", shaderFlags);
            var pixelBytecode = SharpDX.D3DCompiler.ShaderBytecode.Compile(shaderBuffer, "PSMain", "ps_5_0", shaderFlags);

            var vertexShader = new ShaderBytecode(vertexBytecode);
            var pixelShader = new ShaderBytecode(pixelBytecode);

            var rasterizerState = RasterizerStateDescription.Default();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.Wireframe;

            // pipeline state
            var pipelineStateDescription = new GraphicsPipelineStateDescription()
            {
                InputLayout = inputLayoutDescription,
                RootSignature = RootSignature,
                VertexShader = vertexShader,
                PixelShader = pixelShader,
                RasterizerState = rasterizerState,
                BlendState = BlendStateDescription.Default(),
                DepthStencilFormat = Format.D32_Float,
                DepthStencilState = DepthStencilStateDescription.Default(),
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = PipelineStateFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                StreamOutput = new StreamOutputDescription()
            };
            pipelineStateDescription.RenderTargetFormats[0] = Format.B8G8R8A8_UNorm;
            //pipelineStateDescription.DepthStencilState.DepthWriteMask = DepthWriteMask.Zero;
            PipelineState = Renderer.Device.CreateGraphicsPipelineState(pipelineStateDescription);
        }

        public virtual void Dispose()
        {
            PipelineState.Dispose();
            RootSignature.Dispose();
        }
    }
}
