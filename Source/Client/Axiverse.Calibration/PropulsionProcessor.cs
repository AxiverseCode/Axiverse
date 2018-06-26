using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    public class PropulsionProcessor : Processor<PhysicsComponent, PropulsionComponent>
    {
        public override ProcessorStage Stage => ProcessorStage.Components;

        public override void ProcessEntity(Entity entity, PhysicsComponent physics, PropulsionComponent propulsion)
        {
            foreach (var emitter in propulsion.Emitters)
            {
                physics.Body.ApplyForce(emitter.ThrustVector, emitter.Offset);
            }
        }
    }
}
