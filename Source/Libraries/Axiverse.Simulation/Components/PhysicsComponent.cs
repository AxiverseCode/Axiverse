using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics;

namespace Axiverse.Simulation
{
    public class PhysicsComponent : Component
    {
        public Body Body { get; }

        public PhysicsComponent(Body body)
        {
            Body = body;
        }
    }
}
