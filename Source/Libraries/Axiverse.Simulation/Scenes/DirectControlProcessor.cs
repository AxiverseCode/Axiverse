using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class DirectControlProcessor : Processor<DirectControlComponent, PhysicsComponent>
    {
        float maximumTranslational = 100;
        float maximumRotational = 5;

        public override void ProcessEntity(SimulationContext context, Entity entity, DirectControlComponent controller, PhysicsComponent physicsComponent)
        {
            var body = physicsComponent.Body;

            if (context.DeltaTime == 0)
            {
                return;
            }

            if (controller.Translational.Any(v => Math.Abs(v) > 0.1f))
            {
                body.ApplyLocalCentralImpulse((controller.Translational * maximumTranslational).ClampLength(maximumTranslational), context.DeltaTime);
            }
            else
            {
                body.ApplyGlobalCentralImpulse(-body.LinearVelocity.ClampLength(maximumTranslational), context.DeltaTime);
            }


            if (controller.Steering.Any(v => Math.Abs(v) > 0.1f))
            {
                body.ApplyLocalTorqueImpulse((controller.Steering * maximumRotational).ClampLength(maximumRotational), context.DeltaTime);
            }
            else
            {
                body.ApplyGlobalTorqueImpulse(-body.AngularVelocity.ClampLength(maximumRotational), context.DeltaTime);
            }

            // Console.WriteLine("DC Angular velocity length: {0}", body.AngularVelocity.Length());
            // Console.WriteLine(body.totalForce);

            base.ProcessEntity(context, entity, controller, physicsComponent);
        }
    }
}
