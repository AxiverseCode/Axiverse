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

        public int BackBufferIndex { get; private set; } = 0;

        public int BackBufferCount { get; } = 3;

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
                    BufferCount = BackBufferCount,
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
                    BackBufferIndex = NativeSwapChain.CurrentBackBufferIndex;
                }
            }
            // We need now to retrieve the back buffers:
            // 1) We need a heap to store the views
            var handle = Device.RenderTargetViewAllocator.Allocate(BackBufferCount);

            BackBuffers = new Texture[BackBufferCount];
            for (int i = 0; i < BackBufferCount; i++)
            {
                BackBuffers[i] = new Texture(Device);
                BackBuffers[i].Initialize(NativeSwapChain.GetBackBuffer<Resource>(i));
            }

            BackBuffer = new Texture(Device);
            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(BackBufferIndex));

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
            BackBufferIndex = NativeSwapChain.CurrentBackBufferIndex;

            BackBuffer.Resource.Dispose();
            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(BackBufferIndex));

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

            foreach (var resource in Device.PresenterResources)
            {
                resource.Dispose();
            }
            for (int i = 0; i < BackBufferCount; i++)
            {
                BackBuffers[i].Dispose();
            }
            BackBuffer.Dispose();

            Device.PrintLiveObjects();

            NativeSwapChain.ResizeBuffers(
                BackBufferCount,
                Description.Width,
                Description.Height,
                Format.B8G8R8A8_UNorm,
                SwapChainFlags.None);

            BackBuffer.Initialize(NativeSwapChain.GetBackBuffer<Resource>(BackBufferIndex));
            for (int i = 0; i < BackBufferCount; i++)
            {
                BackBuffers[i].Initialize(NativeSwapChain.GetBackBuffer<Resource>(i));
            }
            foreach (var resource in Device.PresenterResources)
            {
                resource.Recreate();
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

        private readonly List<CompiledCommandList> compiledCommandLists = new List<CompiledCommandList>();
    }
}
