using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Simulation;
using Axiverse.Interface.Rendering;

namespace Axiverse.Interface.Scenes
{
    public class RenderableComponent : Simulation.Component
    {
        public Matrix4 Transform { get; set; }

        public Model Model { get; set; }

        public override Component Clone()
        {
            throw new NotImplementedException();
        }
    }
}
