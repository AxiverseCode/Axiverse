using Axiverse.Interface.Graphics;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Compositing
{
    /// <summary>
    /// Composites different entities from the scene and renders them
    /// </summary>
    public class Compositor
    {
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> used to render.
        /// </summary>
        public GraphicsDevice Device { get; set; }


        public GraphicsDevice2D Device2D { get; set; }

        public Window Window { get; set; }

        /// <summary>
        /// Gets the <see cref="SwapChain"/> that the compositor is targeting for final display.
        /// </summary>
        public Presenter Presenter { get; set; }

        /// <summary>
        /// Gets or sets the current renderer.
        /// </summary>
        public Renderer Renderer { get; set; }

        public CommandList DefaultCommandList { get; }

        private Queue<GraphicsResource> uploadedResources = new Queue<GraphicsResource>();

        // runs each scenerenderer

        public Compositor(GraphicsDevice device, Presenter presenter)
        {
            Device = device;
            Presenter = presenter;

            Renderer = new ForwardRenderer(device);
            DefaultCommandList = CommandList.Create(device);
        }

        public void Process(Scene scene, CameraComponent camera)
        {
            var context = new RenderContext()
            {
                CommandList = DefaultCommandList,
                Scene = scene,
                Camera = camera,
            };

            Renderer.Collect(context);

            Renderer.Prepare(context);

            // Clear and set

            Prerender(context);
            Renderer.Render(context);
            Postrender(context);

            // Render and present

            context.CommandList.Wait();
            Renderer.Release(context);
            Release();
        }

        public void Prerender(RenderContext context)
        {
            context.CommandList.Reset(Presenter);
            Presenter.TryResize();

            Presenter.BeginDraw(context.CommandList);
            context.CommandList.ResourceTransition(Presenter.BackBuffer, ResourceState.Present, ResourceState.RenderTarget);

            context.CommandList.SetRenderTargets(Presenter.BackBuffer, Presenter.DepthStencilBuffer);
            context.CommandList.SetViewport(0, 0, Presenter.Description.Width, Presenter.Description.Height);
            context.CommandList.SetScissor(0, 0, Presenter.Description.Width, Presenter.Description.Height);
            context.CommandList.ClearDepth(Presenter.DepthStencilBuffer, 1.0f);
            context.CommandList.ClearTargetColor(Presenter.BackBuffer, 0.2f, 0.2f, 0.2f, 1.0f);

            while (Device.UploadQueue.Count > 0)
            {
                var resource = Device.UploadQueue.Dequeue();
                resource.Upload(context.CommandList);
                uploadedResources.Enqueue(resource);
            }
        }

        public void Postrender(RenderContext context)
        {
            //context.CommandList.ResourceTransition(Presenter.BackBuffer, ResourceState.RenderTarget, ResourceState.Present);

            Presenter.EndDraw(context.CommandList);

            Device2D.Draw(Window);

            context.CommandList.FinishFrame(Presenter);
            Presenter.Present();
        }

        public void Release()
        {
            while (uploadedResources.Count > 0)
            {
                uploadedResources.Dequeue().DisposeUpload();
            }
        }
    }
}
