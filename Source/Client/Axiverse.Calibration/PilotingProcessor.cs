using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    public class PilotingProcessor : Processor<PilotingComponent, PhysicsComponent>
    {
        // Move to preprocessing once we use propulsoncomponent
        public override ProcessorStage Stage => ProcessorStage.Components;

        public override void ProcessEntity(Entity entity, PilotingComponent piloting, PhysicsComponent physics)
        {
            // physics.Body.ApplyTorqueImpulse();
        }
    }
}
