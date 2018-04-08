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
        public GraphicsCommandList NativeCommandList => nativeCommandList;

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

        private CommandAllocator[] commandAllocators;
        private Fence[] fences;
        private long[] fenceValues;

        public CommandList(GraphicsDevice device) : base(device)
        {

        }

        public void Initialize(int bufferCount)
        {
            commandAllocators = new CommandAllocator[bufferCount];
            fences = new Fence[bufferCount];
            fenceValues = new long[bufferCount];
            for (int i = 0; i < bufferCount; i++)
            {
                commandAllocators[i] = Device.NativeDevice.CreateCommandAllocator(CommandListType.Direct);
                fenceValues[i] = 0;
                fences[i] = Device.NativeDevice.CreateFence(fenceValues[i], FenceFlags.None);
            }
            nativeCommandList = Device.NativeDevice.CreateCommandList(CommandListType.Direct, commandAllocators[0], null);
            // We close it as it starts in open state
            NativeCommandList.Close();
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
            while (fences[index].CompletedValue < fenceValues[index])
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

            commandAllocators[index].Reset();
            NativeCommandList.Reset(commandAllocators[index], null);
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
            fenceValues[index]++;
            swapChain.NativeCommandQueue.Signal(fences[index], fenceValues[index]);
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

        public void SetIndexBuffer(GraphicsBuffer view)
        {
            NativeCommandList.SetIndexBuffer(view.NativeIndexBufferView);
        }

        public void SetVertexBuffer(GraphicsBuffer view)
        {
            NativeCommandList.SetVertexBuffer(0, view.NativeVertexBufferView);
        }

        public void DrawIndexed(int idxCnt)
        {
            NativeCommandList.DrawIndexedInstanced(idxCnt, 1, 0, 0, 0);
        }

        public void Draw(IndexBufferBinding index, VertexBufferBinding vertex)
        {
            NativeCommandList.SetIndexBuffer(index.Buffer.NativeIndexBufferView);
            NativeCommandList.SetVertexBuffer(0, vertex.Buffer.NativeVertexBufferView);
            NativeCommandList.DrawIndexedInstanced(index.Count, 1, index.Offset, vertex.Offset, 0);
        }

        public void Draw(IndexBufferBinding index, VertexBufferBinding[] vertices)
        {
            Contract.Requires<IndexOutOfRangeException>(vertices.Length > 0);
            for (int i = 0; i < vertices.Length; i++)
            {
                Contract.Requires(vertices[i].Offset == vertices[0].Offset);
            }

            NativeCommandList.SetIndexBuffer(index.Buffer.NativeIndexBufferView);
            for (int i = 0; i < vertices.Length; i++)
            {
                NativeCommandList.SetVertexBuffer(i, vertices[i].Buffer.NativeVertexBufferView);
            }
            NativeCommandList.DrawIndexedInstanced(index.Count, 1, index.Offset, vertices[0].Offset, 0);
        }

        public static CommandList Create(GraphicsDevice device, int bufferCount)
        {
            var commandList = new CommandList(device);
            commandList.Initialize(bufferCount);
            return commandList;
        }
    }
}
