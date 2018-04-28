using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class RenderTarget
    {
        public static int FrameCount = 2;
        public SwapChain3 SwapChain;

        public int FrameIndex;
        public ViewportF Viewport;
        public SharpDX.Rectangle ScissorRectangle;

        public DescriptorHeap RenderTargetViewDescriptorHeap;
        public SharpDX.Direct3D12.Resource[] RenderTargets;
        public int RenderTargetViewDescriptorSize;

        public DescriptorHeap DepthStencilViewDescriptorHeap;
        public SharpDX.Direct3D12.Resource DepthTarget;

        public Renderer Renderer;
        public Device Device;

        public int Width;
        public int Height;
        public bool NeedsBufferResize;

        public RenderTarget(Renderer renderer)
        {
            Renderer = renderer;
            Device = renderer.Device;
        }

        public void Initialize(RenderForm form)
        {
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            Viewport.Width = Width;
            Viewport.Height = Height;
            Viewport.MaxDepth = 1;

            ScissorRectangle.Right = Width;
            ScissorRectangle.Bottom = Height;

            using (var factory = new Factory4())
            {

                var format = Format.R8G8B8A8_UNorm;
                format = Format.B8G8R8A8_UNorm;

                // swap chain
                var swapChainDescription = new SwapChainDescription
                {
                    BufferCount = FrameCount,
                    ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), format),
                    Usage = Usage.RenderTargetOutput,
                    SwapEffect = SwapEffect.FlipDiscard,
                    OutputHandle = form.Handle,
                    Flags = SwapChainFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    IsWindowed = true,
                };

                using (var tempSwapChain = new SwapChain(factory, Renderer.CommandQueue, swapChainDescription))
                {
                    SwapChain = tempSwapChain.QueryInterface<SwapChain3>();
                    FrameIndex = SwapChain.CurrentBackBufferIndex;

                }
            }

            // render target frames
            var renderTargetViewHeapDescription = new DescriptorHeapDescription
            {
                DescriptorCount = FrameCount,
                Flags = DescriptorHeapFlags.None,
                Type = DescriptorHeapType.RenderTargetView,
            };

            RenderTargetViewDescriptorHeap = Device.CreateDescriptorHeap(renderTargetViewHeapDescription);
            RenderTargetViewDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.RenderTargetView);
            RenderTargets = new SharpDX.Direct3D12.Resource[FrameCount];
            var renderTargetViewHandle = RenderTargetViewDescriptorHeap.CPUDescriptorHandleForHeapStart;

            for (int i = 0; i < FrameCount; i++)
            {
                RenderTargets[i] = SwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                Device.CreateRenderTargetView(RenderTargets[i], null, renderTargetViewHandle + (RenderTargetViewDescriptorSize * i));
            }

            // depth buffer

            //create depth buffer;
            DescriptorHeapDescription depthStencilViewDescriptorHeapDescription = new DescriptorHeapDescription()
            {
                DescriptorCount = FrameCount,
                Flags = DescriptorHeapFlags.None,
                Type = DescriptorHeapType.DepthStencilView
            };
            DepthStencilViewDescriptorHeap = Device.CreateDescriptorHeap(depthStencilViewDescriptorHeapDescription);

            ClearValue depthOptimizedClearValue = new ClearValue()
            {
                Format = Format.D32_Float,
                DepthStencil = new DepthStencilValue() { Depth = 1.0F, Stencil = 0 },
            };

            DepthTarget = Device.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                new ResourceDescription(ResourceDimension.Texture2D, 0, Width, Height, 1, 0, Format.D32_Float, 1, 0, TextureLayout.Unknown, ResourceFlags.AllowDepthStencil),
                ResourceStates.DepthWrite, depthOptimizedClearValue);

            Device.CreateDepthStencilView(DepthTarget, null, DepthStencilViewDescriptorHeap.CPUDescriptorHandleForHeapStart);

            // fence
            Fence = Device.CreateFence(0, FenceFlags.None);
            FenceValue = 1;
            FenceEvent = new AutoResetEvent(false);
        }

        public void Resize(RenderForm form)
        {
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            NeedsBufferResize = true;

            Resized?.Invoke(this, null);
        }

        public event EventHandler Resized;

        public void ResizeBuffers()
        {
            NeedsBufferResize = false;

            Viewport.Width = Width;
            Viewport.Height = Height;
            Viewport.MaxDepth = 1;

            ScissorRectangle.Right = Width;
            ScissorRectangle.Bottom = Height;

            for (int n = 0; n < FrameCount; n++)
            {
                RenderTargets[n].Dispose();
                RenderTargets[n] = null;
            }

            SwapChain.ResizeBuffers(FrameCount, Width, Height, Format.B8G8R8A8_UNorm, SwapChainFlags.None);

            // regester new back buffers
            var renderTargetViewHandle = RenderTargetViewDescriptorHeap.CPUDescriptorHandleForHeapStart;

            for (int i = 0; i < FrameCount; i++)
            {
                RenderTargets[i] = SwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                Device.CreateRenderTargetView(RenderTargets[i], null, renderTargetViewHandle + (RenderTargetViewDescriptorSize * i));
            }

            // recreate depth buffer
            DepthTarget.Dispose();
            ClearValue depthOptimizedClearValue = new ClearValue()
            {
                Format = Format.D32_Float,
                DepthStencil = new DepthStencilValue() { Depth = 1.0F, Stencil = 0 },
            };

            DepthTarget = Device.CreateCommittedResource(
                new HeapProperties(HeapType.Default),
                HeapFlags.None,
                new ResourceDescription(ResourceDimension.Texture2D, 0, Width, Height, 1, 0, Format.D32_Float, 1, 0, TextureLayout.Unknown, ResourceFlags.AllowDepthStencil),
                ResourceStates.DepthWrite, depthOptimizedClearValue);

            Device.CreateDepthStencilView(DepthTarget, null, DepthStencilViewDescriptorHeap.CPUDescriptorHandleForHeapStart);

            FrameIndex = SwapChain.CurrentBackBufferIndex;
        }

        public Fence Fence;
        public int FenceValue;
        public AutoResetEvent FenceEvent;


        public void Present()
        {
            SwapChain.Present(0, PresentFlags.None);
            FrameIndex = SwapChain.CurrentBackBufferIndex;
        }

        public void Dispose()
        {
            Fence.Dispose();

            foreach (var target in RenderTargets)
            {
                target.Dispose();
            }

            RenderTargetViewDescriptorHeap.Dispose();

            DepthTarget.Dispose();
            DepthStencilViewDescriptorHeap.Dispose();

            SwapChain.Dispose();
            Device.Dispose();
        }
    }
}
