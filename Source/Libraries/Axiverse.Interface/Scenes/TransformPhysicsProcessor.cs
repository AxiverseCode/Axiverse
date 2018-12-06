using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class TransformPhysicsProcessor : Processor<TransformComponent, PhysicsComponent>
    {
        public override void ProcessEntity(SimulationContext context, Entity entity, TransformComponent transform, PhysicsComponent physics)
        {
            transform.Rotation = physics.Body.AngularPosition;
            transform.Translation = physics.Body.LinearPosition;
            entity.Spatial.Position = transform.Translation;
        }
    }
}
