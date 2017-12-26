using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public abstract class Equiptment : IEntityDynamic
    {
        public Entity Entity { get; set; }
        
        public virtual void Step(float delta)
        {

        }
    }
}
