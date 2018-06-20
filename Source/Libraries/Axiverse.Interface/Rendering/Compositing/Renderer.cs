using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Compositing
{
    public class Renderer
    {
        // set pipeline state

        // iterate through all nodes

        // extract models

        // extract meshes

        // - apply meshdraw
        // - apply model bindings to cbuffers
        // - apply material bindings to cbuffers
        // - apply mesh bindings to cbuffers
        // - draw

        public void Collect()
        {

        }

        public void Prepare()
        {

        }

        public void Render()
        {

        }

        public void DrawModel(Model model)
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
