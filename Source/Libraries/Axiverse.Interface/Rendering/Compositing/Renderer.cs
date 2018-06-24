using Axiverse.Interface.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Compositing
{
    /// <summary>
    /// Renderer
    /// </summary>
    public abstract class Renderer
    {
        public Scene Scene { get; set; }
        // set pipeline state

        // iterate through all nodes

        // extract models

        // extract meshes

        // - apply meshdraw
        // - apply model bindings to cbuffers
        // - apply material bindings to cbuffers
        // - apply mesh bindings to cbuffers
        // - draw

        public virtual void Collect(RenderContext context)
        {

        }

        public virtual void Prepare(RenderContext context)
        {

        }

        public virtual void Render(RenderContext context)
        {

        }

        public virtual void Release(RenderContext context)
        {

        }

        public virtual void DrawModel(Model model)
        {
            foreach (var mesh in model.Meshes)
            {
                // apply model bindings to cbuffers (need to do this ever time in case bindings 
                // aren't always overwritten.

                // apply mesh bindings to cbuffers

                if (mesh.MaterialIndex >= 0)
                {
                    var material = model.Materials[mesh.MaterialIndex];
                    // apply material bindings to cbuffers
                }

                // draw
            }
        }
    }
}
