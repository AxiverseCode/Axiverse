using Axiverse.Interface.Rendering;
using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class RenderableComponent : Component
    {
        public RenderObject Renderable { get; set; }
    }
}
