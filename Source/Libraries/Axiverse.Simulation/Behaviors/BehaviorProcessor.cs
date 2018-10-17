using Axiverse.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Behaviors
{
    public class BehaviorProcessor : Processor<BehaviorComponent, PhysicsComponent>
    {
        private float acceleration = 0.1f;
        private float maxAngularVelocity = 0.5f;

        float radius = 50.0f;

        public IEnumerable<Body> Neighbors(float distance, Body body)
        {
            return Entities
                .Select(kv => kv.Value.Components.Get<PhysicsComponent>().Body)
                .Where(b => b != body && (b.LinearPosition - body.LinearPosition).LengthSquared() < distance * distance);
        }

        public override void ProcessEntity(SimulationContext context, Entity entity, BehaviorComponent component1, PhysicsComponent component2)
        {
            Body body = component2.Body;
            var neighbors = Neighbors(radius, body);
            
            var desire = Vector3.Zero;
            desire += Steering.Cohesion(radius, body.LinearPosition, neighbors);
            desire += Steering.Separation(radius, body.LinearPosition, neighbors);
            desire += Steering.Traveling(neighbors);
            desire += Steering.Arrival(Vector3.Zero, body, 1, 1);

            var angularTarget = Steering.Alignment(body.AngularPosition, neighbors);
            var angularDistance = Quaternion.Distance(body.AngularPosition, angularTarget);
            //Console.Write(angularDistance.ToString("n2") + ", ");

            if (angularDistance > 0)
            {
                var scaled = Math.Min(1, maxAngularVelocity * context.DeltaTime / angularDistance);
                body.AngularPosition = Quaternion.Lerp(body.AngularPosition, angularTarget, scaled);
            }

            body.LinearVelocity += desire * context.DeltaTime;

            body.ClearForces();
            body.AngularPosition = Quaternion.LookAt(-body.LinearVelocity, body.AngularPosition.Transform(Vector3.Up));
        }

        public override void Process(SimulationContext context)
        {
            base.Process(context);
            //Console.WriteLine();
        }
    }
}
