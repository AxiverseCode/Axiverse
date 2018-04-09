using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class DescriptorSet : GraphicsResource 
    {
        public DescriptorLayout Layout { get; }
        internal readonly CpuDescriptorHandle ShaderResourceViewHandle;
        internal readonly CpuDescriptorHandle SamplerHandle;

        private DescriptorSet(GraphicsDevice device, DescriptorLayout layout) : base(device)
        {
            Layout = layout;

            ShaderResourceViewHandle = device.ShaderResourceViewAllocator.Allocate(1);
            SamplerHandle = device.SamplerAllocator.Allocate(1);
        }

        public void SetConstantBuffer(int slot, GraphicsBuffer buffer, int offset, int size)
        {
            Device.NativeDevice.CreateConstantBufferView(new ConstantBufferViewDescription
            {
                BufferLocation = buffer.GpuHandle + offset,
                SizeInBytes = size, // TODO: Size needs to be 256 byte aligned
            }, ShaderResourceViewHandle + 0);
        }

        public void SetShaderResourceView(int slot, GraphicsResource resource)
        {

        }

        public void SetSamplerState(int slot, SamplerState samplerState)
        {

            Device.NativeDevice.CopyDescriptorsSimple(
                1, SamplerHandle + 0, samplerState.NativeSampler, DescriptorHeapType.Sampler);
        }
    }
}
