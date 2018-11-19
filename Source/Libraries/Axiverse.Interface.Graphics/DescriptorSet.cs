using SharpDX.Direct3D12;
using System.Diagnostics.Contracts;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// A set of descriptors representing the state of bound buffers for an operation.
    /// </summary>
    /// <remarks>
    /// This is the binding between a set of shaders and the parameters that it requres to be
    /// bound.
    /// </remarks>
    public class DescriptorSet : GraphicsResource
    {
        /// <summary>
        /// Gets the <see cref="DescriptorLayout"/>.
        /// </summary>
        public DescriptorLayout Layout { get; }
        internal readonly CpuDescriptorHandle ShaderResourceViewHandle;
        internal readonly CpuDescriptorHandle SamplerHandle;

        /// <summary>
        /// Constructs a <see cref="DescriptorSet"/>.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="layout"></param>
        public DescriptorSet(GraphicsDevice device, DescriptorLayout layout) : base(device)
        {
            Layout = layout;

            ShaderResourceViewHandle = device.ShaderResourceViewAllocator.Allocate(layout.ShaderResourceViewCount);
            SamplerHandle = device.SamplerAllocator.Allocate(layout.SamplerCount);
        }

        /// <summary>
        /// Sets a constant buffer.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="buffer"></param>
        public void SetConstantBuffer(int slot, GraphicsBuffer buffer)
        {
            Requires.That(Layout.Entries[slot].Type == DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView);
            Requires.That(buffer.Size % 256 == 0);
            Device.NativeDevice.CreateConstantBufferView(new ConstantBufferViewDescription
            {
                BufferLocation = buffer.GpuHandle,
                SizeInBytes = buffer.Size, // TODO: Size needs to be 256 byte aligned
            }, ShaderResourceViewHandle + Layout.Entries[slot].Index * Device.ShaderResourceViewAllocator.Stride);
        }

        /// <summary>
        /// Sets a constant buffer.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public void SetConstantBuffer(int slot, GraphicsBuffer buffer, int offset, int size)
        {
            Contract.Requires(Layout.Entries[slot].Type == DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView);
            Device.NativeDevice.CreateConstantBufferView(new ConstantBufferViewDescription
            {
                BufferLocation = buffer.GpuHandle + offset,
                SizeInBytes = size, // TODO: Size needs to be 256 byte aligned
            }, ShaderResourceViewHandle + Layout.Entries[slot].Index * Device.ShaderResourceViewAllocator.Stride);
        }

        /// <summary>
        /// Sets a shader resource view.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="resource"></param>
        public void SetShaderResourceView(int slot, Texture resource)
        {
            Device.NativeDevice.CreateShaderResourceView(resource.Resource, new ShaderResourceViewDescription
            {

                Shader4ComponentMapping = 5768,
                //Shader4ComponentMapping = D3DXUtilities.DefaultComponentMapping(),
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Dimension = ShaderResourceViewDimension.Texture2D,
                // TODO(axiverse): Take this from the texture resource
                Texture2D = { MipLevels = resource.MipLevels },
            }, ShaderResourceViewHandle + Layout.Entries[slot].Index * Device.ShaderResourceViewAllocator.Stride);
        }

        /// <summary>
        /// Sets a sampler state.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="samplerState"></param>
        public void SetSamplerState(int slot, SamplerState samplerState)
        {
            Contract.Requires(Layout.Entries[slot].Type == DescriptorLayout.EntryType.SamplerState);
            Device.NativeDevice.CopyDescriptorsSimple(
                1, SamplerHandle + Layout.Entries[slot].Index * Device.SamplerAllocator.Stride, samplerState.NativeSampler, DescriptorHeapType.Sampler);
        }
    }
}
