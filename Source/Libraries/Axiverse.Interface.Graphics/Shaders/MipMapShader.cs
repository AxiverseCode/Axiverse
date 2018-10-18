using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;
using Axiverse.Injection;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class MipMapShader : Shader
    {
        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 256)]
        public struct PerObject
        {
            public Matrix4 WorldViewProjection;
            public Vector4 Color;
        }

        public DescriptorLayout Layout { get; private set; }

        public ShaderBytecode ComputeShader { get; set; }

        // cbufferview
        // samplers
        public MipMapShader(GraphicsDevice device) : base(device)
        {

        }

        public void Initialize()
        {
            // Pipeline state.
            var testShaderPath = "../../../../../Resources/Engine/MipMap.hlsl";
            ComputeShader = ShaderBytecode.CompileFromFile(testShaderPath, "GenerateMipMaps", "cs_5_0");

            // Root parameters.
            var rootParameters = new RootParameter[]
            {
                new RootParameter(ShaderVisibility.All, new RootConstants(0, 0, 2)),
                new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.ShaderResourceView,
                        DescriptorCount = 1,
                        OffsetInDescriptorsFromTableStart = int.MinValue, // D3D12_DESCRIPTOR_RANGE_OFFSET_APPEND
                        BaseShaderRegister = 0
                    }),
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.UnorderedAccessView,
                        BaseShaderRegister = 0,
                        OffsetInDescriptorsFromTableStart = int.MinValue, // D3D12_DESCRIPTOR_RANGE_OFFSET_APPEND
                        DescriptorCount = 1
                    }),
            };

            var staticSamplers = new StaticSamplerDescription[]
            {
                new StaticSamplerDescription()
                {
                    Filter = Filter.MinMagLinearMipPoint,
                    AddressUVW = TextureAddressMode.Clamp,
                    MipLODBias = 0f,
                    ComparisonFunc = Comparison.Never,
                    MinLOD = 0f,
                    MaxLOD = float.MaxValue,
                    MaxAnisotropy = 0,
                    BorderColor = StaticBorderColor.OpaqueBlack,
                    ShaderRegister = 0,
                    RegisterSpace = 0,
                    ShaderVisibility = ShaderVisibility.All
                }
            };

            var rootSignatureDescription = new RootSignatureDescription(
                RootSignatureFlags.AllowInputAssemblerInputLayout, rootParameters, staticSamplers);
            RootSignature = RootSignature.Create(Device, rootSignatureDescription);

            Layout = new DescriptorLayout(
                DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView,
                DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView);
        }
    }
}
