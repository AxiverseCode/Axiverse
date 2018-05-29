using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation;
using Axiverse.Simulation.Components;

namespace Axiverse.Interface.Scenes
{
    public class Entity : Simulation.Entity
    {
        HierarchalComponent Hierarchy { get; }

        ModelComponent Renderable { get; }

        public Entity()
        {
            Hierarchy = new HierarchalComponent();
            Renderable = new ModelComponent();

            Set(Hierarchy);
            Set(Renderable);
        }
    }
}
