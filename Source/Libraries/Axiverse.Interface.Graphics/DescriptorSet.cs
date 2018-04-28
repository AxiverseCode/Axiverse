using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

            ShaderResourceViewHandle = device.ShaderResourceViewAllocator.Allocate(layout.ShaderResourceViewCount);
            SamplerHandle = device.SamplerAllocator.Allocate(layout.SamplerCount);
        }

        public void SetConstantBuffer(int slot, GraphicsBuffer buffer, int offset, int size)
        {
            Contract.Requires(Layout.Entries[slot].Type == DescriptorLayout.EntryType.ShaderResourceView);
            Device.NativeDevice.CreateConstantBufferView(new ConstantBufferViewDescription
            {
                BufferLocation = buffer.GpuHandle + offset,
                SizeInBytes = size, // TODO: Size needs to be 256 byte aligned
            }, ShaderResourceViewHandle + Layout.Entries[slot].Index * Device.ShaderResourceViewAllocator.Stride);
        }

        public void SetShaderResourceView(int slot, GraphicsResource resource)
        {
            
        }

        public void SetSamplerState(int slot, SamplerState samplerState)
        {
            Contract.Requires(Layout.Entries[slot].Type == DescriptorLayout.EntryType.SamplerState);
            Device.NativeDevice.CopyDescriptorsSimple(
                1, SamplerHandle + Layout.Entries[slot].Index * Device.SamplerAllocator.Stride, samplerState.NativeSampler, DescriptorHeapType.Sampler);
        }
    }
}
