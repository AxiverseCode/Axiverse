using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D12;


namespace Axiverse.Interface.Engine.Rendering
{
    /// <summary>
    /// Used to perform rendering operations.
    /// </summary>
    public class RenderContext
    {
        private GraphicsCommandList mCmdList;
        private CommandAllocator[] mCmdAllocator;
        private Fence[] mFences;
        private long[] mFenceValues;

        public void Init(Device device,int numBuffers)
        {
            mCmdAllocator   = new CommandAllocator[numBuffers];
            mFences         = new Fence[numBuffers];
            mFenceValues    = new long[numBuffers];
            for (int i = 0; i < numBuffers; i++)
            {
                mCmdAllocator[i]    = device.CreateCommandAllocator(CommandListType.Direct);
                mFenceValues[i]     = 0;
                mFences[i]          = device.CreateFence(mFenceValues[i], FenceFlags.None);
            }
            mCmdList = device.CreateCommandList(CommandListType.Direct, mCmdAllocator[0], null);
            // We close it as it starts in open state
            mCmdList.Close();
        }

        /// <summary>
        /// Should be called at the start of the frame. This method waits if needed for the GPU
        /// </summary>
        /// <param name="chain"></param>
        public void Reset(SwapChain chain)
        {
            int idx     = chain.CurBufferIdx;
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
            mCmdList.Reset(mCmdAllocator[idx], null);
        }

        public GraphicsCommandList GetNativeContext()
        {
            return mCmdList;
        }

        public void Close()
        {
            mCmdList.Close();
        }

        /// <summary>
        /// This should be called after this context is executed. This way we will add
        /// sync commands at the end of the queue
        /// </summary>
        /// <param name="chain"></param>
        public void FinishFrame(SwapChain chain)
        {
            int idx = chain.CurBufferIdx;
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
            mCmdList.SetViewport(viewport);
        }

        public void SetScissor(int x, int y, int w, int h)
        {
            SharpDX.Rectangle rectangle = new SharpDX.Rectangle(x, y, w, h);
            mCmdList.SetScissorRectangles(rectangle);
        }

        public void SetColorTarget(CpuDescriptorHandle view)
        {
            mCmdList.SetRenderTargets(1, view, null);
        }

        public void ClearTargetColor(CpuDescriptorHandle handle,float r,float g, float b, float a)
        {
            mCmdList.ClearRenderTargetView(handle, new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
        }

        public void ResourceTransition(SharpDX.Direct3D12.Resource resource,ResourceStates before, ResourceStates after)
        {
            mCmdList.ResourceBarrierTransition(resource, before, after);
        }

        public void SetIndexBuffer(IndexBufferView view)
        {
            mCmdList.SetIndexBuffer(view);
        }

        public void SetVertexBuffer(VertexBufferView view)
        {
            mCmdList.SetVertexBuffer(0, view);
        }

        public void DrawIndexed(int idxCnt)
        {
            mCmdList.DrawIndexedInstanced(idxCnt, 1, 0, 0, 0);
        }
    }
}
