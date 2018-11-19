using SharpDX.DXGI;
using System.Collections.Generic;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;
    using System.Diagnostics;

    public class Presenter : GraphicsResource
    {
        public PresenterDescription Description { get; set; }

        public Texture BackBuffer { get; set; }

        public Texture[] BackBuffers { get; set; }

        public int BackBufferIndex => bufferIndex;

        public int BackBufferCount => bufferCount;

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
                    ModeDescription = new ModeDescription(width, height, new Rational(60, 1), Format.B8G8R8A8_UNorm),
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

            BackBuffers = new Texture[bufferCount];
            for (int i = 0; i < bufferCount; i++)
            {
                BackBuffers[i] = new Texture(Device);
                BackBuffers[i].Initialize(NativeSwapChain.GetBackBuffer<Resource>(i));
            }

            BackBuffer = new Texture(Device);
            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(bufferIndex));

            CreateDepthStencilBuffer();

            Device.PrintLiveObjects();
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

            ResizePending = true;
        }

        private bool ResizePending;

        public void TryResize()
        {
            if (!ResizePending)
            {
                return;
            }
            ResizePending = false;

            ResizeSwapChain();
            ResizeDepthStencilBuffer();
        }

        protected void ResizeSwapChain()
        {

            Device.PrintLiveObjects();

            Debug.WriteLine("====");
            for (int i = 0; i < BackBufferCount; i++)
            {
                BackBuffers[i].Dispose();
            }
            BackBuffer.Dispose();

            Device.PrintLiveObjects();

            NativeSwapChain.ResizeBuffers(
                bufferCount,
                Description.Width,
                Description.Height,
                Format.B8G8R8A8_UNorm,
                SwapChainFlags.None);

            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(bufferIndex));
            for (int i = 0; i < bufferCount; i++)
            {
                BackBuffers[i].Initialize(NativeSwapChain.GetBackBuffer<Resource>(i));
            }
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
