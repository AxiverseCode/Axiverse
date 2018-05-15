using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class HierarchalComponent : Component
    {
        public Entity Parent { get; set; }

        public EntityCollection Children { get; set; }

        public override Component Clone()
        {
            throw new NotImplementedException();
        }
    }
}
