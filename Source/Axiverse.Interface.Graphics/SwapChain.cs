using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public class SwapChain : GraphicsResource
    {
        /// <summary>
        /// The size of the back buffer chain
        /// </summary>
        public int BufferCount = 3;
        public int CurrentBufferIndex;

        internal CommandQueue NativeCommandQueue;
        internal SwapChain3 NativeSwapChain;
        private int width;
        private int height;
        
        internal SharpDX.Direct3D12.Resource[] NativeBackBuffers;
        private CpuDescriptorHandle[] backBufferHandles;

        private SwapChain(GraphicsDevice device) : base(device)
        {

        }

        /// <summary>
        /// Initializes this present chain and retrieves the backbuffers
        /// </summary>
        /// <param name="target">The output window</param>
        private void Initialize(Control target)
        {
            // Lets create a present command queue
            var queueDesc = new CommandQueueDescription(CommandListType.Direct);
            NativeCommandQueue = Device.NativeDevice.CreateCommandQueue(queueDesc);
            
            // Descirbe and create the swap chain
            using (var factory = new Factory4())
            {
                width  = target.ClientSize.Width;
                height = target.ClientSize.Height;
                var swapChainDescription = new SwapChainDescription
                {
                    BufferCount         = BufferCount,
                    ModeDescription     = new ModeDescription(width, height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    Usage               = Usage.RenderTargetOutput,
                    SwapEffect          = SwapEffect.FlipDiscard,
                    OutputHandle        = target.Handle,
                    Flags               = SwapChainFlags.None,
                    SampleDescription   = new SampleDescription(1, 0),
                    IsWindowed          = true
                };
                using (var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, NativeCommandQueue, swapChainDescription))
                {
                    NativeSwapChain   = tempSwapChain.QueryInterface<SwapChain3>();
                    CurrentBufferIndex    = NativeSwapChain.CurrentBackBufferIndex;
                }
            }
            // We need now to retrieve the back buffers:
            // 1) We need a heap to store the views
            var handle = Device.RenderTargetViewAllocator.Allocate(BufferCount);

            // 2) Now we create the views
            backBufferHandles = new CpuDescriptorHandle[BufferCount];
            NativeBackBuffers = new SharpDX.Direct3D12.Resource[BufferCount];

            for (int i = 0; i < BufferCount; i++)
            {
                NativeBackBuffers[i] = NativeSwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                backBufferHandles[i] = handle + (Device.RenderTargetViewAllocator.Stride * i);
                Device.NativeDevice.CreateRenderTargetView(NativeBackBuffers[i], null, backBufferHandles[i]);
            }
        }

        public CpuDescriptorHandle GetCurrentColorHandle()
        {
            return backBufferHandles[CurrentBufferIndex];
        }

        public SharpDX.Direct3D12.Resource StartFrame()
        {
            return NativeBackBuffers[CurrentBufferIndex];
        }

        public void ExecuteCommandList(CommandList list)
        {
            NativeCommandQueue.ExecuteCommandList(list.NativeCommandList);
        }
        
        public void Present()
        {
            NativeSwapChain.Present(0, PresentFlags.None);
            CurrentBufferIndex = NativeSwapChain.CurrentBackBufferIndex;
        }


        void Resize()
        {

        }

        public static SwapChain Create(GraphicsDevice device, Control target)
        {
            var swapChain = new SwapChain(device);
            swapChain.Initialize(target);
            return swapChain;
        }

        protected override void Dispose(bool disposing)
        {
            NativeSwapChain.Dispose();
            NativeSwapChain = null;
            NativeCommandQueue.Dispose();
            NativeCommandQueue = null;
            base.Dispose(disposing);
        }

    }
}
