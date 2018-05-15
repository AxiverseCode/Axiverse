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

        internal CommandAllocatorPool CommandAllocators;
        internal DescriptorHeapPool ShaderResourceViewDescriptorHeaps;
        internal DescriptorHeapPool SamplerHeaps;
        internal const int ShaderResourceViewDescriptorHeapSize = 2048;
        internal const int SamplerHeapSize = 64;

        /// <summary>
        /// Initializes the GPU device
        /// </summary>
        public void Initialize()
        {
#if DEBUG
            DebugInterface.Get().EnableDebugLayer();
#endif
            NativeDevice = new Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);

            // Create allocators.
            {
                SamplerAllocator = new DescriptorAllocator(this, DescriptorHeapType.Sampler);
                ShaderResourceViewAllocator = new DescriptorAllocator(
                    this,
                    DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
                DepthStencilViewAllocator =
                    new DescriptorAllocator(this, DescriptorHeapType.DepthStencilView);
                RenderTargetViewAllocator =
                    new DescriptorAllocator(this, DescriptorHeapType.RenderTargetView);
            }

            // Create pools.
            {
                CommandAllocators = new CommandAllocatorPool(this);
                ShaderResourceViewDescriptorHeaps = new DescriptorHeapPool(
                    this,
                    DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                    ShaderResourceViewDescriptorHeapSize);
                SamplerHeaps = new DescriptorHeapPool(
                    this, DescriptorHeapType.Sampler, SamplerHeapSize);
            }
        }

        public static GraphicsDevice Create()
        {
            var graphicsDevice = new GraphicsDevice();
            graphicsDevice.Initialize();
            return graphicsDevice;
        }

        /// <summary>
        /// Object pool for command allocators.
        /// </summary>
        internal class CommandAllocatorPool : ObjectPool<CommandAllocator>
        {
            public GraphicsDevice Device { get; }

            public CommandAllocatorPool(GraphicsDevice device) 
            {
                Device = device;
            }

            protected override CommandAllocator Create()
            {
                return Device.NativeDevice.CreateCommandAllocator(CommandListType.Direct);
            }

            protected override void Reset(CommandAllocator item)
            {
                item.Reset();
            }
        }

        /// <summary>
        /// Object pool for descriptor heaps.
        /// </summary>
        internal class DescriptorHeapPool : ObjectPool<DescriptorHeap>
        {
            public GraphicsDevice Device { get; }
            public DescriptorHeapType HeapType { get; }
            public int Stride { get; }
            public int Size { get; }

            public DescriptorHeapPool(GraphicsDevice device, DescriptorHeapType heapType, int size)
            {
                Device = device;
                HeapType = heapType;
                Size = size;
                Stride = device.NativeDevice.GetDescriptorHandleIncrementSize(heapType);
            }

            protected override DescriptorHeap Create()
            {
                var description = new DescriptorHeapDescription
                {
                    DescriptorCount = Size,
                    Flags = DescriptorHeapFlags.ShaderVisible,
                    Type = HeapType,
                };
                return Device.NativeDevice.CreateDescriptorHeap(description);
            }
        }

        /// <summary>
        /// Allocator for individual descriptors. These individual allocations are used to stage
        /// descriptors to be copied into descriptor heaps by a <see cref="CommandList"/>.
        /// </summary>
        internal class DescriptorAllocator : GraphicsResource
        {
            public DescriptorHeapType HeapType { get; }
            public int Stride { get; }

            private DescriptorHeap heap;
            private CpuDescriptorHandle handle;
            private int remaining;

            public DescriptorAllocator(GraphicsDevice device, DescriptorHeapType heapType) : base(device)
            {
                HeapType = heapType;
                Stride = device.NativeDevice.GetDescriptorHandleIncrementSize(heapType);
            }

            public CpuDescriptorHandle Allocate(int count)
            {
                if (heap == null || remaining < count)
                {
                    heap = Device.NativeDevice.CreateDescriptorHeap(new DescriptorHeapDescription
                    {
                        Flags = DescriptorHeapFlags.None,
                        Type = HeapType,
                        DescriptorCount = DescriptorsPerHeap,
                        NodeMask = 1,
                    });
                    remaining = DescriptorsPerHeap;
                    handle = heap.CPUDescriptorHandleForHeapStart;
                }

                var result = handle;

                handle.Ptr += Stride * count;
                remaining -= count;

                return result;
            }

            public static int DescriptorsPerHeap = 256;
        }
    }
}
