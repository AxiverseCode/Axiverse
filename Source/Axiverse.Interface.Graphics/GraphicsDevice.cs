using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Is responsible for creating all other objects (textures, buffers, shaders, pipeline states, etc.)
    /// </summary>
    public class GraphicsDevice
    {
        /// <summary>
        /// Gets the native d3d device
        /// </summary>
        internal Device NativeDevice { get; private set; }

        /// <summary>
        /// Gets the list of resources bound to this device.
        /// </summary>
        public List<GraphicsResource> Resources { get; } = new List<GraphicsResource>();


        internal DescriptorAllocator SamplerAllocator;
        internal DescriptorAllocator ShaderResourceViewAllocator;
        internal DescriptorAllocator DepthStencilViewAllocator;
        internal DescriptorAllocator RenderTargetViewAllocator;

        /// <summary>
        /// Initializes the GPU device
        /// </summary>
        public void Initialize()
        {
#if DEBUG
            DebugInterface.Get().EnableDebugLayer();
#endif
            NativeDevice = new Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);

            // Create Allocators
            SamplerAllocator = new DescriptorAllocator(this, DescriptorHeapType.Sampler);
            ShaderResourceViewAllocator= new DescriptorAllocator(
                this,
                DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
            DepthStencilViewAllocator =
                new DescriptorAllocator(this, DescriptorHeapType.DepthStencilView);
            RenderTargetViewAllocator =
                new DescriptorAllocator(this, DescriptorHeapType.RenderTargetView);
        }

        public static GraphicsDevice Create()
        {
            var graphicsDevice = new GraphicsDevice();
            graphicsDevice.Initialize();
            return graphicsDevice;
        }

        public class DescriptorAllocator : GraphicsResource
        {
            private readonly DescriptorHeapType heapType;
            private DescriptorHeap heap;
            private CpuDescriptorHandle handle;
            private int remaining;
            public readonly int Stride;

            public DescriptorAllocator(GraphicsDevice device, DescriptorHeapType heapType) : base(device)
            {
                this.heapType = heapType;
                Stride = device.NativeDevice.GetDescriptorHandleIncrementSize(heapType);
            }

            public CpuDescriptorHandle Allocate(int count)
            {
                if (heap == null || remaining < count)
                {
                    heap = Device.NativeDevice.CreateDescriptorHeap(new DescriptorHeapDescription
                    {
                        Flags = DescriptorHeapFlags.None,
                        Type = heapType,
                        DescriptorCount = DescriptorsPerHeap,
                        NodeMask = 1,
                    });
                    remaining = DescriptorsPerHeap;
                    handle = heap.CPUDescriptorHandleForHeapStart;
                }

                var result = handle;

                handle.Ptr += Stride;
                remaining -= count;

                return result;
            }

            public static int DescriptorsPerHeap = 256;
        }
    }
}
