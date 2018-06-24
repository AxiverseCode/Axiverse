using Axiverse.Interface.Graphics;
using Axiverse.Interface.Rendering.Compositing;
using Axiverse.Interface.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering
{
    /// <summary>
    /// The context used during a render.
    /// </summary>
    public class RenderContext
    {
        /// <summary>
        /// The compositor rendering the scene.
        /// </summary>
        public Compositor Compositor { get; set; }

        /// <summary>
        /// Gets the scene that is being rendered.
        /// </summary>
        public Scene Scene { get; set; }

        public CommandList CommandList { get; set; }
    }
}
