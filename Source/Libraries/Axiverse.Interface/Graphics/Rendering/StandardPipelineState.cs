using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Axiverse.Interface.Graphics
{
    using System.Runtime.InteropServices;
    using SharpDX.Direct3D12;

    public class StandardPipelineState : GraphicsPipelineState
    {
        public SharpDX.Direct3D12.Resource ConstantBuffer;
        public DescriptorHeap ConstantBufferViewShaderResourceViewDescriptorHeap;
        public int ConstantBufferViewShaderResourceViewDescriptorSize;
        public DescriptorHeap SamplerViewDescriptorHeap;

        public IntPtr ConstantBufferData;
        

        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 256)]
        struct PerObject
        {
            public Matrix WorldViewProjection;
        }

        public StandardPipelineState(Renderer renderer) : base(renderer)
        {

        }

        public override void Load(string path, InputLayoutDescription inputLayoutDescription)
        {
            base.Load(path, inputLayoutDescription);

            const int constantCount = 5;

            // constant buffer
            var constantBufferViewShaderResourceViewHeapDescription = new DescriptorHeapDescription
            {
                // first index is used for texture
                DescriptorCount = constantCount + 1,
                Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                Flags = DescriptorHeapFlags.ShaderVisible,
            };
            ConstantBufferViewShaderResourceViewDescriptorHeap = Device.CreateDescriptorHeap(constantBufferViewShaderResourceViewHeapDescription);
            ConstantBufferViewShaderResourceViewDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);

            ConstantBuffer = Device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(Utilities.SizeOf<PerObject>() * constantCount),
                ResourceStates.GenericRead);


            for (int i = 0; i < constantCount; i++)
            {
                var constantBufferViewDescription = new ConstantBufferViewDescription()
                {
                    BufferLocation = ConstantBuffer.GPUVirtualAddress + Utilities.SizeOf<PerObject>() * i,
                    SizeInBytes = Utilities.SizeOf<PerObject>(),
                };

                Device.CreateConstantBufferView(
                    constantBufferViewDescription,
                    ConstantBufferViewShaderResourceViewDescriptorHeap.CPUDescriptorHandleForHeapStart + (ConstantBufferViewShaderResourceViewDescriptorSize * (i + 1)));
            }
            ConstantBufferData = ConstantBuffer.Map(0);


            //sampler buffer view heap
            SamplerViewDescriptorHeap = Device.CreateDescriptorHeap(new DescriptorHeapDescription()
            {
                DescriptorCount = 1,
                Type = DescriptorHeapType.Sampler,
                Flags = DescriptorHeapFlags.ShaderVisible
            });

            //bind sampler
            Device.CreateSampler(new SamplerStateDescription()
            {
                Filter = Filter.ComparisonAnisotropic,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MinimumLod = float.MinValue,
                MaximumLod = float.MaxValue,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Never
            }, SamplerViewDescriptorHeap.CPUDescriptorHandleForHeapStart);
        }

        public void SetTransform(Matrix matrix, int index = 0)
        {
            PerObject perObject = new PerObject
            {
                WorldViewProjection = matrix
            };
            Utilities.Write(ConstantBufferData + (Utilities.SizeOf<PerObject>() * index), ref perObject);
        }

        public void ApplyConstantBuffer(GraphicsCommandList commandList, int index)
        {
            commandList.SetGraphicsRootDescriptorTable(0,
                ConstantBufferViewShaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart + ConstantBufferViewShaderResourceViewDescriptorSize * (index + 1));
        }

        public void SetTexture(Texture texture)
        {
            Device.CreateShaderResourceView(
                texture.Resource,
                texture.ShaderResourceViewDescription,
                ConstantBufferViewShaderResourceViewDescriptorHeap.CPUDescriptorHandleForHeapStart);
        }

        public void Apply(GraphicsCommandList commandList)
        {
            commandList.SetGraphicsRootSignature(RootSignature);

            commandList.SetDescriptorHeaps(2, new[] { ConstantBufferViewShaderResourceViewDescriptorHeap, SamplerViewDescriptorHeap });
            commandList.SetGraphicsRootDescriptorTable(0, ConstantBufferViewShaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart + ConstantBufferViewShaderResourceViewDescriptorSize); // matrix
            commandList.SetGraphicsRootDescriptorTable(1, ConstantBufferViewShaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart); // texture
            commandList.SetGraphicsRootDescriptorTable(2, SamplerViewDescriptorHeap.GPUDescriptorHandleForHeapStart);
        }

        public override void Dispose()
        {
            SamplerViewDescriptorHeap.Dispose();
            ConstantBufferViewShaderResourceViewDescriptorHeap.Dispose();

            ConstantBuffer.Unmap(0);
            ConstantBuffer.Dispose();

            base.Dispose();
        }
    }
}
