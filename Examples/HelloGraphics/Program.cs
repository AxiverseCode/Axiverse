using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;
using Axiverse.Interface.Engine.Rendering;

namespace HelloGraphics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a window
            RenderForm form = new RenderForm()
            {
                Width       = 1024,
                Height      = 720,
                BackColor   = System.Drawing.Color.Black,
                Text        = "Axiverse | HelloGraphics",
            };
            form.Show();

            // Init the rendering device
            RenderDevice device = new RenderDevice();
            device.Init();
            // Init a swap chain
            SwapChain chain = new SwapChain();
            chain.Init(form, device);
            // Init a graphics context
            RenderContext context = new RenderContext();
            context.Init(device.NativeDevice);

            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var backBuffer = chain.StartFrame();
                    var backBufferHandle = chain.GetCurrentColorHandle();
                    context.Reset();
                    context.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);
                    {
                        context.SetColorTarget(backBufferHandle);
                        context.SetViewport(0, 0, 1024, 720);
                        context.SetScissor(0, 0, 1024, 720);
                        context.ClearTargetColor(backBufferHandle, 1.0f, 0.0f, 1.0f, 1.0f);
                    }
                    context.ResourceTransition(backBuffer, SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);
                    context.Close();
                    chain.ExecuteCommandList(context.GetNativeContext());
                    chain.Present();
                }
            }
        }
    }
}
