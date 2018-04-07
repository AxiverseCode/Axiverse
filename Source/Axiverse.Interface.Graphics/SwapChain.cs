using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Represents a chain of back buffers and is responsible 
    /// for showing the final rendered image on the screen.
    /// </summary>
    public class SwapChain
    {
        /// <summary>
        /// The size of the back buffer chain
        /// </summary>
        public static int BufferCount = 3;
        public int CurBufferIdx;

        private CommandQueue mPresentQueue;
        private SwapChain3 mPresentChain;
        private int mWidth;
        private int mHeight;

        private DescriptorHeap mBackBufferHeap;
        private SharpDX.Direct3D12.Resource[] mBackBuffers;
        private CpuDescriptorHandle[] mBackbufferHandles;

        /// <summary>
        /// Initializes this present chain and retrieves the backbuffers
        /// </summary>
        /// <param name="window">The output window</param>
        /// <param name="device"></param>
        public void Init(RenderForm window, GraphicsDevice device)
        {
            // Lets create a present command queue
            var queueDesc = new CommandQueueDescription(CommandListType.Direct);
            mPresentQueue = device.NativeDevice.CreateCommandQueue(queueDesc);

            // Descirbe and create the swap chain
            using (var factory = new Factory4())
            {
                mWidth  = window.ClientSize.Width;
                mHeight = window.ClientSize.Height;
                var swapChainDescription = new SwapChainDescription
                {
                    BufferCount         = BufferCount,
                    ModeDescription     = new ModeDescription(mWidth, mHeight, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    Usage               = Usage.RenderTargetOutput,
                    SwapEffect          = SwapEffect.FlipDiscard,
                    OutputHandle        = window.Handle,
                    Flags               = SwapChainFlags.None,
                    SampleDescription   = new SampleDescription(1, 0),
                    IsWindowed          = true
                };
                using (var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, mPresentQueue, swapChainDescription))
                {
                    mPresentChain   = tempSwapChain.QueryInterface<SwapChain3>();
                    CurBufferIdx    = mPresentChain.CurrentBackBufferIndex;
                }
            }
            // We need now to retrieve the back buffers:
            // 1) We need a heap to store the views
            var renderTargetViewHeapDescription = new DescriptorHeapDescription
            {
                DescriptorCount = BufferCount,
                Flags           = DescriptorHeapFlags.None,
                Type            = DescriptorHeapType.RenderTargetView,
            };
            mBackBufferHeap = device.NativeDevice.CreateDescriptorHeap(renderTargetViewHeapDescription);
            // 2) Now we create the views
            mBackbufferHandles = new CpuDescriptorHandle[BufferCount];
            int rtHandleSize    = device.NativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            mBackBuffers        = new SharpDX.Direct3D12.Resource[BufferCount];
            var handle          = mBackBufferHeap.CPUDescriptorHandleForHeapStart;
            for (int i = 0; i < BufferCount; i++)
            {
                mBackBuffers[i]         = mPresentChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                mBackbufferHandles[i]   = handle + (rtHandleSize * i);
                device.NativeDevice.CreateRenderTargetView(mBackBuffers[i], null, mBackbufferHandles[i]);
            }
        }

        public CpuDescriptorHandle GetCurrentColorHandle()
        {
            return mBackbufferHandles[CurBufferIdx];
        }

        public SharpDX.Direct3D12.Resource StartFrame()
        {
            return mBackBuffers[CurBufferIdx];
        }

        public void ExecuteCommandList(GraphicsCommandList list)
        {
            mPresentQueue.ExecuteCommandList(list);
        }

        public CommandQueue GetNativeQueue()
        {
            return mPresentQueue;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Present()
        {
            mPresentChain.Present(0, PresentFlags.None);
            CurBufferIdx = mPresentChain.CurrentBackBufferIndex;
        }


        void Resize()
        {

        }

        void Dispose()
        {

        }

    }
}
