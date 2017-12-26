using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Prototypes
{
    public abstract class EquiptmentPrototype
    {
        public string Name { get; set; }

        public abstract Equiptment Create();
    }
}
