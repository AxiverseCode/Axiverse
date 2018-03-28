using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Dynamics
{
    public interface IConstraint
    {
        Body BodyA { get; }
        Body BodyB { get; }

        void Step(float timestep);

        void Solve();
    }
}
