using SharpDX.Direct3D12;
using System.Runtime.InteropServices;

namespace Axiverse.Interface.Graphics.Shaders
{
    /// <summary>
    /// Physically based shader.
    /// </summary>
    public class PhysicallyBasedShader : Shader
    {
        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 256)]
        public struct TransformConstants
        {
            public Matrix4 WorldViewProjection;
            public Matrix4 SkyProjection;
            public Matrix4 SceneRotation;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 256)]
        public struct ShadingConstants
        {
            public Vector3 Direction1;
            public Vector3 Radiance1;

            public Vector3 Direction2;
            public Vector3 Radiance2;

            public Vector3 Direction3;
            public Vector3 Radiance3;

            public Vector3 Eye;
        }

        public DescriptorLayout Layout { get; private set; }

        /// <summary>
        /// Constructs a physically based shader.
        /// </summary>
        /// <param name="device"></param>
        public PhysicallyBasedShader(GraphicsDevice device) : base(device)
        {

        }

        public void Initialize()
        {
            // Pipeline state.
            var testShaderPath = "../../../../../Resources/Engine/PBR.hlsl";
            VertexShader = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0");
            PixelShader = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0");

            // Root parameters.
            var rootParameters = new RootParameter[]
            {
                new RootParameter(ShaderVisibility.All,
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
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.Sampler,
                        BaseShaderRegister = 0,
                        DescriptorCount = 1
                    }),
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.Sampler,
                        BaseShaderRegister = 0,
                        DescriptorCount = 1
                    }),
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.Sampler,
                        BaseShaderRegister = 0,
                        DescriptorCount = 1
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

            Layout = new DescriptorLayout(
                DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView,
                DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView,
                DescriptorLayout.EntryType.SamplerState);
        }
    }
}
