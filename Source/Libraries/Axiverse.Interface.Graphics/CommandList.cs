using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;


namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Used to perform rendering operations.
    /// </summary>
    public class CommandList : GraphicsResource
    {
        private GraphicsCommandList nativeCommandList;
        internal GraphicsCommandList NativeCommandList => nativeCommandList;

        private PipelineState pipelineState;
        public PipelineState PipelineState
        {
            get => pipelineState;
            set
            {
                pipelineState = value;
                NativeCommandList.PipelineState = value.NativePipelineState;
                NativeCommandList.PrimitiveTopology = (PrimitiveTopology)value.PrimitiveType;
            }
        }

        private CompiledCommandList compiledCommandList;
        private CommandAllocator commandAllocator;
        private Fence fence;
        private long fenceValue;

        /// <summary>
        /// The current shader resource view <see cref="DescriptorHeap"/> to copy descriptors into                             /
        /// for execution with this command list. When it is filled, we will add it to the list of
        /// heaps in the compiled command list for releasing after execution.
        /// </summary>
        private DescriptorHeap shaderResourceViewDescriptorHeap;
        private int shaderResourceViewOffset;

        /// <summary>
        /// The current sampler <see cref="DescriptorHeap"/> to copy descriptors into for execution
        /// with this command list.
        /// </summary>
        private DescriptorHeap samplerDescriptorHeap;
        private int samplerOffset;

        /// <summary>
        /// Fixed descriptor heap array for setting the descriptor heaps.
        /// </summary>
        private readonly DescriptorHeap[] descriptorHeaps = new DescriptorHeap[2];

        protected CommandList(GraphicsDevice device) : base(device)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                NativeCommandList.Dispose();
                shaderResourceViewDescriptorHeap.Dispose();
                samplerDescriptorHeap.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            commandAllocator = Device.CommandAllocators.Take();
            compiledCommandList = new CompiledCommandList();
            fenceValue = 0;
            fence = Device.NativeDevice.CreateFence(0, FenceFlags.None);
            
            nativeCommandList = Device.NativeDevice.CreateCommandList(CommandListType.Direct, commandAllocator, null);
            // We close it as it starts in open state
            NativeCommandList.Close();



            // Create heaps to copy resources into.
            shaderResourceViewDescriptorHeap = Device.ShaderResourceViewDescriptorHeaps.Take();
            shaderResourceViewOffset = 0;
            samplerDescriptorHeap = Device.SamplerHeaps.Take();
            samplerOffset = 0;

            descriptorHeaps[0] = shaderResourceViewDescriptorHeap;
            descriptorHeaps[1] = samplerDescriptorHeap;
        }

        public void SetDescriptors(DescriptorSet descriptors)
        {
            NativeCommandList.SetDescriptorHeaps(descriptorHeaps);

            if (descriptors.Layout.ShaderResourceViewCount != 0)
            {
                if (shaderResourceViewOffset + descriptors.Layout.ShaderResourceViewCount >
                    Device.ShaderResourceViewDescriptorHeaps.Size)
                {
                    // No more space in the current descriptor heap
                    compiledCommandList.ShaderResourceViewHeaps.Add(shaderResourceViewDescriptorHeap);

                    shaderResourceViewDescriptorHeap = Device.ShaderResourceViewDescriptorHeaps.Take();
                    shaderResourceViewOffset = 0;
                    descriptorHeaps[0] = shaderResourceViewDescriptorHeap;
                    NativeCommandList.SetDescriptorHeaps(descriptorHeaps);
                }

                // Copy CBSRV descriptors from cpu descriptor table into the rolling gpu upload descriptor heap.
                Device.NativeDevice.CopyDescriptorsSimple(
                    descriptors.Layout.ShaderResourceViewCount,
                    shaderResourceViewDescriptorHeap.CPUDescriptorHandleForHeapStart
                        + (shaderResourceViewOffset * Device.ShaderResourceViewDescriptorHeaps.Stride),
                    descriptors.ShaderResourceViewHandle,
                    DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
                GpuDescriptorHandle gpuHandle = shaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart
                    + (shaderResourceViewOffset * Device.ShaderResourceViewDescriptorHeaps.Stride);
                shaderResourceViewOffset += descriptors.Layout.ShaderResourceViewCount;

                foreach (var entry in descriptors.Layout.Entries)
                {
                    if (entry.Type == DescriptorLayout.EntryType.ShaderResourceView)
                    {
                        NativeCommandList.SetGraphicsRootDescriptorTable(
                            entry.Slot,
                            gpuHandle + (entry.Index * Device.ShaderResourceViewDescriptorHeaps.Stride));
                    }
                }
            }



            if (descriptors.Layout.SamplerCount != 0)
            {
                if (samplerOffset + descriptors.Layout.SamplerCount >
                    Device.SamplerHeaps.Size)
                {
                    // No more space in the current descriptor heap
                    compiledCommandList.SamplerHeaps.Add(samplerDescriptorHeap);

                    samplerDescriptorHeap = Device.SamplerHeaps.Take();
                    samplerOffset = 0;
                    descriptorHeaps[1] = samplerDescriptorHeap;
                    NativeCommandList.SetDescriptorHeaps(descriptorHeaps);
                }
                
                // Copy sampler descriptors from cpu descriptor table into the rolling gpu upload descriptor heap.
                Device.NativeDevice.CopyDescriptorsSimple(
                    descriptors.Layout.SamplerCount,
                    samplerDescriptorHeap.CPUDescriptorHandleForHeapStart
                        + (samplerOffset * Device.SamplerHeaps.Stride),
                    descriptors.SamplerHandle,
                    DescriptorHeapType.Sampler);
                GpuDescriptorHandle samplerGpuHandle = samplerDescriptorHeap.GPUDescriptorHandleForHeapStart
                    + (samplerOffset * Device.SamplerHeaps.Stride);
                samplerOffset += descriptors.Layout.SamplerCount;

                foreach (var entry in descriptors.Layout.Entries)
                {
                    if (entry.Type == DescriptorLayout.EntryType.SamplerState)
                    {
                        NativeCommandList.SetGraphicsRootDescriptorTable(
                            entry.Slot,
                            samplerGpuHandle + (entry.Index * Device.SamplerHeaps.Stride));
                    }
                }
            }
        }

        public void SetIndexBuffer(GraphicsBuffer buffer, int size, IndexBufferType type)
        {
            var view = new IndexBufferView
            {
                BufferLocation = buffer.GpuHandle,
                Format = (type == IndexBufferType.Integer32) ?
                    SharpDX.DXGI.Format.R32_UInt : SharpDX.DXGI.Format.R16_UInt,
                SizeInBytes = size,
            };
            NativeCommandList.SetIndexBuffer(view);
        }

        public void SetVertexBuffer(GraphicsBuffer buffer, int slot, int size, int stride)
        {
            var view = new VertexBufferView
            {
                BufferLocation = buffer.GpuHandle,
                SizeInBytes = size,
                StrideInBytes = stride,
            };
            NativeCommandList.SetVertexBuffer(slot, view);
        }









        /// <summary>
        /// Should be called at the start of the frame. This method waits if needed for the GPU
        /// </summary>
        /// <param name="swapChain"></param>
        public void Reset(SwapChain swapChain)
        {
            int index = swapChain.CurrentBufferIndex;
            int waits = 0;
            int start = Environment.TickCount;
            while (fence.CompletedValue < fenceValue)
            {
                // ...wait...
                waits++;
            }

            int end = Environment.TickCount;
            if (waits > 0)
            {
                // We just have 1ms res with Environment.TickCount, we need a hires timer for accurate results
                int waitedMs = end - start;
                if (waitedMs >= 1)
                {
                    Console.WriteLine("Waited:" + waitedMs + "ms!");
                }

            }

            commandAllocator.Reset();
            NativeCommandList.Reset(commandAllocator, null);
        }

        public void Close()
        {
            NativeCommandList.Close();
        }

        /// <summary>
        /// This should be called after this context is executed. This way we will add
        /// sync commands at the end of the queue
        /// </summary>
        /// <param name="swapChain"></param>
        public void FinishFrame(SwapChain swapChain)
        {
            int index = swapChain.CurrentBufferIndex;
            fenceValue++;
            swapChain.NativeCommandQueue.Signal(fence, fenceValue);
        }

        public void SetViewport(int x,int y,int w,int h)
        {
            ViewportF viewport = new ViewportF
            {
                Width       = w,
                Height      = h,
                X           = x,
                Y           = y,
                MaxDepth    = 1.0f,
                MinDepth    = 0.0f
            };
            NativeCommandList.SetViewport(viewport);
        }

        public void SetScissor(int x, int y, int w, int h)
        {
            SharpDX.Rectangle rectangle = new SharpDX.Rectangle(x, y, w, h);
            NativeCommandList.SetScissorRectangles(rectangle);
        }

        public void SetColorTarget(CpuDescriptorHandle view)
        {
            NativeCommandList.SetRenderTargets(1, view, null);
        }

        public void ClearTargetColor(CpuDescriptorHandle handle,float r,float g, float b, float a)
        {
            NativeCommandList.ClearRenderTargetView(handle, new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
        }

        public void ResourceTransition(SharpDX.Direct3D12.Resource resource,ResourceState before, ResourceState after)
        {
            NativeCommandList.ResourceBarrierTransition(resource, (ResourceStates)before, (ResourceStates)after);
        }

        public void SetRootSignature(RootSignature rootSignature)
        {
            NativeCommandList.SetGraphicsRootSignature(rootSignature.NativeRootSignature);
        }

        public void DrawIndexed(int idxCnt)
        {
            NativeCommandList.DrawIndexedInstanced(idxCnt, 1, 0, 0, 0);
        }









        public static CommandList Create(GraphicsDevice device)
        {
            var commandList = new CommandList(device);
            commandList.Initialize();
            return commandList;
        }
    }
}
