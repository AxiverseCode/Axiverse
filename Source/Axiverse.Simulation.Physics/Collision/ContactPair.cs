using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Physics.Collision
{
    public class ContactPair
    {
        public RigidBody Former { get; set; }
        public RigidBody Latter { get; set; }
    }
}