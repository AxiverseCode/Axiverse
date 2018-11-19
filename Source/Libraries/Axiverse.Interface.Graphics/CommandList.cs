using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using System;


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
            compiledCommandList = new CompiledCommandList
            {
                CommandList = this
            };
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
                    CloseShaderResourceViewHeap();
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
                    if (entry.Type == DescriptorLayout.EntryType.ConstantBufferShaderResourceOrUnorderedAccessView)
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
                    CloseSamplerHeap();
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

        private void CloseShaderResourceViewHeap()
        {
            if (shaderResourceViewOffset == 0)
            {
                return;
            }
            compiledCommandList.ShaderResourceViewHeaps.Add(shaderResourceViewDescriptorHeap);

            shaderResourceViewDescriptorHeap = Device.ShaderResourceViewDescriptorHeaps.Take();
            shaderResourceViewOffset = 0;
            descriptorHeaps[0] = shaderResourceViewDescriptorHeap;
            NativeCommandList.SetDescriptorHeaps(descriptorHeaps);
        }

        private void CloseSamplerHeap()
        {
            if (samplerOffset == 0)
            {
                return;
            }
            compiledCommandList.SamplerHeaps.Add(samplerDescriptorHeap);

            samplerDescriptorHeap = Device.SamplerHeaps.Take();
            samplerOffset = 0;
            descriptorHeaps[1] = samplerDescriptorHeap;
            NativeCommandList.SetDescriptorHeaps(descriptorHeaps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        public void SetIndexBuffer(GraphicsBuffer buffer, int size, IndexBufferType type)
        {
            if (buffer != null)
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
            else
            {
                NativeCommandList.SetIndexBuffer(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        public void SetIndexBuffer(IndexBufferBinding binding)
        {
            if (binding != null)
            {
                SetIndexBuffer(binding.Buffer, binding.Count * binding.Stride, binding.Type);
            }
            else
            {
                NativeCommandList.SetIndexBuffer(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="slot"></param>
        /// <param name="size"></param>
        /// <param name="stride"></param>
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
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="slot"></param>
        public void SetVertexBuffer(VertexBufferBinding binding, int slot)
        {
            SetVertexBuffer(binding.Buffer, slot, binding.Count * binding.Stride, binding.Stride);
        }



        /// <summary>
        /// Synchronously waits for the fence value.
        /// </summary>
        public void Wait()
        {
            while (fence.CompletedValue < fenceValue)
            {
                System.Threading.Thread.Sleep(1);
            }
        }



        /// <summary>
        /// Should be called at the start of the frame. This method waits if needed for the GPU
        /// </summary>
        /// <param name="presenter"></param>
        public void Reset(Presenter presenter)
        {
            int index = presenter.BackBufferIndex;
            int waits = 0;
            int start = Environment.TickCount;
            while (fence.CompletedValue < fenceValue)
            {
                System.Threading.Thread.Sleep(1);
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

        /// <summary>
        /// Closes the command list and returns a compile command list with the utilized resources.
        /// </summary>
        /// <returns></returns>
        public CompiledCommandList Close()
        {
            CloseShaderResourceViewHeap();
            CloseSamplerHeap();
            NativeCommandList.Close();
            return compiledCommandList;
        }

        /// <summary>
        /// This should be called after this context is executed. This way we will add
        /// sync commands at the end of the queue
        /// </summary>
        /// <param name="presenter"></param>
        public void FinishFrame(Presenter presenter)
        {
            fenceValue++;
            presenter.NativeCommandQueue.Signal(fence, fenceValue);
        }

        public void SetViewport(int x, int y, int w, int h)
        {
            ViewportF viewport = new ViewportF
            {
                Width = w,
                Height = h,
                X = x,
                Y = y,
                MaxDepth = 1.0f,
                MinDepth = 0.0f
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

        [Obsolete]
        public void SetRenderTargets(CpuDescriptorHandle render, CpuDescriptorHandle depth)
        {
            NativeCommandList.SetRenderTargets(1, render, depth);
        }

        [Obsolete]
        public void SetRenderTargets(CpuDescriptorHandle render, Texture depth)
        {
            NativeCommandList.SetRenderTargets(1, render, depth?.NativeDepthStencilView);
        }

        public void SetRenderTargets(Texture render, Texture depth)
        {
            NativeCommandList.SetRenderTargets(1, render.NativeRenderTargetView, depth?.NativeDepthStencilView);
        }

        public void ClearTargetColor(CpuDescriptorHandle handle, float r, float g, float b, float a)
        {
            NativeCommandList.ClearRenderTargetView(handle, new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
        }

        public void ClearTargetColor(Texture texture, float r, float g, float b, float a)
        {
            NativeCommandList.ClearRenderTargetView(texture.NativeRenderTargetView, new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
        }

        public void ClearDepth(CpuDescriptorHandle handle, float depth)
        {
            NativeCommandList.ClearDepthStencilView(handle, ClearFlags.FlagsDepth, depth, 0);
        }

        public void ClearDepth(Texture texture, float depth)
        {
            NativeCommandList.ClearDepthStencilView(texture.NativeDepthStencilView, ClearFlags.FlagsDepth, depth, 0);
        }

        public void ResourceTransition(Resource resource, ResourceState before, ResourceState after)
        {
            NativeCommandList.ResourceBarrierTransition(resource, (ResourceStates)before, (ResourceStates)after);
        }

        public void ResourceTransition(Texture resource, ResourceState before, ResourceState after)
        {
            NativeCommandList.ResourceBarrierTransition(resource.Resource, (ResourceStates)before, (ResourceStates)after);
        }

        public void SetRootSignature(RootSignature rootSignature)
        {
            NativeCommandList.SetGraphicsRootSignature(rootSignature.NativeRootSignature);
        }

        public void Draw(int count, bool indexed)
        {
            if (indexed)
            {
                DrawIndexed(count);
            }
            else
            {
                Draw(count);
            }
        }

        public void Draw(int count)
        {
            NativeCommandList.DrawInstanced(count, 1, 0, 0);
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
