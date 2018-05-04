﻿using System;
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
    public class GeometryShader : Shader
    {
        [StructLayout(LayoutKind.Sequential)]
        struct PerObject
        {
            Matrix WorldViewProjection;
        }

        // cbufferview
        // samplers
        public GeometryShader(GraphicsDevice device) : base(device)
        {
            
        }

        public void Initialize()
        {
            // Pipeline state.
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
            VertexShader = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0");
            PixelShader = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "vs_5_0");

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

            var layout = new DescriptorLayout(
                DescriptorLayout.EntryType.ShaderResourceView,
                DescriptorLayout.EntryType.ShaderResourceView,
                DescriptorLayout.EntryType.SamplerState);
        }
    }
}