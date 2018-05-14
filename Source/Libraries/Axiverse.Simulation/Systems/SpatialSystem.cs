using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Systems
{
    public class SpatialSystem : System
    {
        public override void Process(Entity entity, float dt)
        {
            // https://en.wikipedia.org/wiki/Semi-implicit_Euler_method
            var spatial = entity.Spatial;

            spatial.Position += spatial.Velocity * dt;
        }
    }
}
