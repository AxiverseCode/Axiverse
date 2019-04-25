using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entities;
using Axiverse.Physics;

namespace Axiverse.Interface2.Simulation
{
    public class Physical : Component
    {
        public Body Body { get; }

        public Physical()
        {
            Body = new Body();
        }
    }
}
