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
        private CommandAllocator mCmdAllocator;

        public void Init(Device device)
        {
            mCmdAllocator = device.CreateCommandAllocator(CommandListType.Direct);
            mCmdList = device.CreateCommandList(CommandListType.Direct, mCmdAllocator, null);
            // We close it as it starts in open state
            mCmdList.Close();
        }

        public void Reset()
        {
            mCmdAllocator.Reset();
            mCmdList.Reset(mCmdAllocator, null);
        }

        public GraphicsCommandList GetNativeContext()
        {
            return mCmdList;
        }

        public void Close()
        {
            mCmdList.Close();
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
