using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;
using Axiverse.Injection;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class GeometryShader : Shader
    {
        struct ConstantData
        {

        }

        public RootSignature RootSignature;
        public PipelineState PipelineState;

        // cbufferview
        // samplers

        public GeometryShader(GraphicsDevice device) : base(device)
        {
            
        }

        public void Initialize()
        {
            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new SharpDX.Direct3D12.InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 0)
            };

            // Pipeline state.
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
            var pipelineStateDescription = new PipelineStateDescription()
            {
                InputLayout = new SharpDX.Direct3D12.InputLayoutDescription(inputElementDescs),
                RootSignature = RootSignature.Create(Device),
                VertexShader = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0"),
                PixelShader = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0"),
            };
            PipelineState = PipelineState.Create(Device, pipelineStateDescription);

            // Root parameters.
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
            RootSignature = RootSignature.Create(Device, rootSignatureDescription);

        }

        /// <summary>
        /// binds to cbuffer index - for instanced drawing?
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindings"></param>
        public void Bind(int index, IBindingProvider bindings)
        {
            binder.SetValues(ref data[index], bindings);
            // write to cbuffer
            int offset = 0; // stride * index
            Utilities.Write(IntPtr.Add(IntPtr.Zero, offset), ref data[index]);
        }

        private ConstantData[] data = new ConstantData[10];
        private Binder binder = new Binder(typeof(ConstantData));
    }
}
