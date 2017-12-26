using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public abstract class Component
    {
        public Entity Entity { get; internal set; }

        public virtual Component Reader => this; 
    }
}
