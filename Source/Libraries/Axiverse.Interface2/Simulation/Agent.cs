using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;
using Axiverse.Physics;

namespace Axiverse.Interface2.Simulation
{
    public class Agent : Component
    {
        public List<Operation> Operations { get; } = new List<Operation>();
        public RelativeFrame Frame { get; private set; }

        public void Update(Clock clock)
        {
            var physical = Entity.Get<Physical>();
            var body = physical.Body;
            var transform = Entity.Transform;

            Frame = RelativeFrame.FromPoint(physical.Body, Vector3.Zero);

            Vector3 steering = new Vector3();
            steering.X = (Frame.LinearPosition.X < 0) ? 3 : -3;
            steering.Z = (Frame.LinearPosition.Z < 0) ? 3 : -3;

            const int Thrust = 150;
            steering.Y = Thrust;
            if (Frame.LinearPosition.Y > 0)
            {
                steering.Y = Math.Min(Thrust, Frame.LinearPosition.Y + 20);
            }

            //Body.AccumulateLocalCentralForce(force);
            body.ClearForces();
            body.AccumulateLocalForce(steering, new Vector3(0, -2, 0));
            body.AccumulateGlobalCentralForce(new Vector3(y: -9.81f / 10));
            body.LinearDampening = new Vector3(0.5f);
            body.AngularDampening = new Vector3(0.5f);
        }
    }
}
