using System;
using System.Collections.Generic;
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

        private CommandAllocator[] mCmdAllocator;
        private Fence[] mFences;
        private long[] mFenceValues;

        public CommandList(GraphicsDevice device) : base(device)
        {

        }

        public void Initialize(int numBuffers)
        {
            mCmdAllocator   = new CommandAllocator[numBuffers];
            mFences         = new Fence[numBuffers];
            mFenceValues    = new long[numBuffers];
            for (int i = 0; i < numBuffers; i++)
            {
                mCmdAllocator[i]    = Device.NativeDevice.CreateCommandAllocator(CommandListType.Direct);
                mFenceValues[i]     = 0;
                mFences[i]          = Device.NativeDevice.CreateFence(mFenceValues[i], FenceFlags.None);
            }
            nativeCommandList = Device.NativeDevice.CreateCommandList(CommandListType.Direct, mCmdAllocator[0], null);
            // We close it as it starts in open state
            NativeCommandList.Close();
        }

        /// <summary>
        /// Should be called at the start of the frame. This method waits if needed for the GPU
        /// </summary>
        /// <param name="chain"></param>
        public void Reset(SwapChain chain)
        {
            int idx     = chain.CurrentBufferIndex;
            int waits   = 0;
            int start   = Environment.TickCount;
            while (mFences[idx].CompletedValue < mFenceValues[idx])
            {
                // ...wait...
                waits++;
            }
            int end = Environment.TickCount;
            if(waits > 0)
            {
                // We just have 1ms res with Environment.TickCount, we need a hires timer for accurate results
                int waitedMs = end - start;
                if(waitedMs >= 1)
                {
                    Console.WriteLine("Waited:" + waitedMs + "ms!");
                }

            }

            mCmdAllocator[idx].Reset();
            NativeCommandList.Reset(mCmdAllocator[idx], null);
        }

        public GraphicsCommandList GetNativeContext()
        {
            return NativeCommandList;
        }

        public void Close()
        {
            NativeCommandList.Close();
        }

        /// <summary>
        /// This should be called after this context is executed. This way we will add
        /// sync commands at the end of the queue
        /// </summary>
        /// <param name="chain"></param>
        public void FinishFrame(SwapChain chain)
        {
            int idx = chain.CurrentBufferIndex;
            mFenceValues[idx]++;
            chain.GetNativeQueue().Signal(mFences[idx], mFenceValues[idx]);
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

        public static CommandList Create(GraphicsDevice device, int bufferCount)
        {
            var commandList = new CommandList(device);
            commandList.Initialize(bufferCount);
            return commandList;
        }
    }
}
