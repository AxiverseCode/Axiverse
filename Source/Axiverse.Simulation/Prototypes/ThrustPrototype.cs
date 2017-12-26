using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Prototypes
{
    public class ThrustPrototype : EquiptmentPrototype
    {
        public override Equiptment Create()
        {
            var value = new ThrustEquiptment();

            return value;
        }
    }
}
