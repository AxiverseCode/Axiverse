using SharpDX.Direct3D12;
using System;
using System.Collections.Generic;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Is responsible for creating all other objects (textures, buffers, shaders, pipeline states, etc.)
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        /// <summary>
        /// Gets the native d3d device
        /// </summary>
        internal Device NativeDevice { get; private set; }

        /// <summary>
        /// Gets the list of resources bound to this device.
        /// </summary>
        public List<GraphicsResource> Resources { get; } = new List<GraphicsResource>();
        public List<IPresenterResource> PresenterResources { get; } = new List<IPresenterResource>();

        public Queue<GraphicsResource> UploadQueue { get; } = new Queue<GraphicsResource>();

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

        public void PrintLiveObjects()
        {
#if DEBUG
            using (var debugDevice = NativeDevice.QueryInterface<DebugDevice>())
            {
                // http://sharpdx.org/forum/4-general/1241-reportliveobjects
                // Contains several Refcount: 0 lines. This cannot be avoided, but is still be useful to
                // find memory leaks (all objects should have Refcount=0, the device still has RefCount=3)
                debugDevice.ReportLiveDeviceObjects(ReportingLevel.Detail);
            }
#endif
        }

        /// <summary>
        /// Creates a <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <returns></returns>
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
        internal class DescriptorHeapPool : DisposableObjectPool<DescriptorHeap>
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

            private List<DescriptorHeap> heaps = new List<DescriptorHeap>();
            private DescriptorHeap heap;
            private CpuDescriptorHandle handle;
            private int remaining;

            public DescriptorAllocator(GraphicsDevice device, DescriptorHeapType heapType) : base(device)
            {
                HeapType = heapType;
                Stride = device.NativeDevice.GetDescriptorHandleIncrementSize(heapType);
            }

            protected override void Dispose(bool disposing)
            {
                if (!IsDisposed)
                {
                    foreach (var heap in heaps)
                    {
                        heap.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            public CpuDescriptorHandle Allocate(int count)
            {
                Requires.IsNotDisposed(this);

                if (heap == null || remaining < count)
                {
                    if (heap != null)
                    {
                        heaps.Add(heap);
                    }

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

        #region IDisposable Support
        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    foreach (var resource in Resources)
                    {
                        resource.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
