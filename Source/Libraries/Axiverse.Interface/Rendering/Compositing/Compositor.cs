using Axiverse.Interface.Graphics;
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

        /// <summary>
        /// Gets the <see cref="SwapChain"/> that the compositor is targeting for final display.
        /// </summary>
        public Presenter Presenter { get; set; }

        /// <summary>
        /// Gets or sets the current renderer.
        /// </summary>
        public Renderer Renderer { get; set; }

        // runs each scenerenderer

        public Compositor()
        {
            Renderer = new ForwardRenderer();
        }

        public void Process()
        {
            var context = new RenderContext();

            Renderer.Collect(context);

            Renderer.Prepare(context);

            // Clear and set

            Prerender(context);
            Renderer.Render(context);
            Postrender(context);

            // Render and present

            Renderer.Release(context);
        }

        public void Prerender(RenderContext context)
        {
            Presenter.BeginDraw(context.CommandList);
            context.CommandList.ResourceTransition(Presenter.BackBuffer, ResourceState.Present, ResourceState.RenderTarget);

            context.CommandList.SetRenderTargets(Presenter.BackBuffer, Presenter.DepthStencilBuffer);
            context.CommandList.SetViewport(0, 0, Presenter.Description.Width, Presenter.Description.Height);
            context.CommandList.SetScissor(0, 0, Presenter.Description.Width, Presenter.Description.Height);
            context.CommandList.ClearDepth(Presenter.DepthStencilBuffer, 1.0f);
            context.CommandList.ClearTargetColor(Presenter.BackBuffer, 0.2f, 0.2f, 0.2f, 1.0f);
        }

        public void Postrender(RenderContext context)
        {
            context.CommandList.ResourceTransition(Presenter.BackBuffer, ResourceState.RenderTarget, ResourceState.Present);

            Presenter.EndDraw(context.CommandList);
            context.CommandList.FinishFrame(Presenter);
            Presenter.Present();
        }
    }
}
