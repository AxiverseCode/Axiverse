using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Prototypes
{
    public class NavigationPrototype : EquiptmentPrototype
    {
        public override Equiptment Create()
        {
            var value = new NavigationEquiptment();

            return value;
        }
    }
}
