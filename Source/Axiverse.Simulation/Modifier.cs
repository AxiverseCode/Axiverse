using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public abstract class Modifier : IEntityDynamic
    {
        public Entity Entity { get; set; }

        public void Step(float delta)
        {

        }
    }
}
