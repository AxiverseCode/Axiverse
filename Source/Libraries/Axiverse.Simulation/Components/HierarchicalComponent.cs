using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public abstract class HierarchicalComponent<T> : Component
        where T : HierarchicalComponent<T>
    {
        public T Parent { get; internal set; }

        public HierarchicalComponentCollection<T> Children { get; }

        public HierarchicalComponent()
        {
            Children = new HierarchicalComponentCollection<T>((T)this);
        }
    }
}
