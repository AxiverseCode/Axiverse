using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Physics.Dynamics
{
    public interface IConstraint
    {
        RigidBody BodyA { get; }
        RigidBody BodyB { get; }

        void Step(float timestep);

        void Solve();
    }
}
