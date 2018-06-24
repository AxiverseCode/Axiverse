using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class Presenter : GraphicsResource
    {
        public PresenterDescription Description { get; set; }

        public Texture BackBuffer { get; set; }

        public int BackBufferIndex => bufferIndex;

        public Texture DepthStencilBuffer { get; set; }

        public bool IsFullscreen { get; set; }

        public Presenter(GraphicsDevice device, PresenterDescription description) : base(device)
        {
            Description = description;
        }

        public void Initialize()
        {
            // Lets create a present command queue
            var queueDesc = new CommandQueueDescription(CommandListType.Direct);
            NativeCommandQueue = Device.NativeDevice.CreateCommandQueue(queueDesc);
            
            // Descirbe and create the swap chain
            using (var factory = new Factory4())
            {
                var width = Description.Width;
                var height = Description.Height;
                var swapChainDescription = new SwapChainDescription
                {
                    BufferCount = bufferCount,
                    ModeDescription = new ModeDescription(width, height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    Usage = Usage.RenderTargetOutput,
                    SwapEffect = SwapEffect.FlipDiscard,
                    OutputHandle = Description.WindowHandle,
                    Flags = SwapChainFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    IsWindowed = true
                };
                using (var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, NativeCommandQueue, swapChainDescription))
                {
                    NativeSwapChain = tempSwapChain.QueryInterface<SwapChain3>();
                    bufferIndex = NativeSwapChain.CurrentBackBufferIndex;
                }
            }
            // We need now to retrieve the back buffers:
            // 1) We need a heap to store the views
            var handle = Device.RenderTargetViewAllocator.Allocate(bufferCount);
            
            BackBuffer = new Texture(Device);
            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(bufferIndex));

            CreateDepthStencilBuffer();
        }

        public void BeginDraw(CommandList commandList)
        {
            //commandList.Reset(this);
        }

        public void EndDraw(CommandList commandList)
        {
            compiledCommandLists.Add(commandList.Close());
            NativeCommandQueue.ExecuteCommandList(commandList.NativeCommandList);
        }

        public void Present()
        {
            NativeSwapChain.Present(0, PresentFlags.None);
            bufferIndex = NativeSwapChain.CurrentBackBufferIndex;

            BackBuffer.Resource.Dispose();
            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(bufferIndex));

            compiledCommandLists.ForEach(c => c.Release());
            compiledCommandLists.Clear();
        }

        /// <summary>
        /// Resizes the presentation.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Resize(int width, int height)
        {
            Description.Width = width;
            Description.Height = height;

            ResizeSwapChain();
            ResizeDepthStencilBuffer();
        }
        
        protected void ResizeSwapChain()
        {
            NativeSwapChain.ResizeBuffers(bufferCount,
                Description.Width, Description.Height,
                Format.B8G8R8A8_UNorm, SwapChainFlags.None);
        }

        protected void ResizeDepthStencilBuffer()
        {
            DisposeDepthStencilBuffer();
            CreateDepthStencilBuffer();
        }

        protected void DisposeDepthStencilBuffer()
        {
            DepthStencilBuffer.Dispose();
            DepthStencilBuffer = null;
        }

        protected void CreateDepthStencilBuffer()
        {
            DepthStencilBuffer = new Texture(Device);
            DepthStencilBuffer.CreateDepth(Description.Width, Description.Height);
        }

        internal CommandQueue NativeCommandQueue;
        internal SwapChain3 NativeSwapChain;

        private int bufferCount = 3;
        private int bufferIndex = 0;

        private readonly List<CompiledCommandList> compiledCommandLists = new List<CompiledCommandList>();
    }
}
