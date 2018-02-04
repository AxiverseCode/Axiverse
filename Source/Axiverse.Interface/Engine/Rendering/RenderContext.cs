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

        public void Reset(SwapChain chain)
        {
            int idx = chain.CurBufferIdx;
            while (mFences[idx].CompletedValue < mFenceValues[idx])
            {
                // ...wait...
            }
            Console.Write("Reset: " + idx + "," + mFences[idx].CompletedValue + "," + mFenceValues[idx] + "\n");
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

        public void FinishFrame(SwapChain chain)
        {
            int idx = chain.CurBufferIdx;
            mFenceValues[idx]++;
            chain.GetNativeQueue().Signal(mFences[idx], mFenceValues[idx]);

            Console.Write("FinishFrame: " + idx + "," + mFences[idx].CompletedValue + "," + mFenceValues[idx] + "\n");
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
    }
}
