using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics;

namespace Axiverse.Simulation
{
    public class PhysicsProcessor : Processor<PhysicsComponent>
    {
        public override ProcessorStage Stage => ProcessorStage.Physics;

        public World World { get; } = new World();

        public override void Process(SimulationContext context)
        {
            // World.Step(context.DeltaTime);
            base.Process(context);
        }

        protected override void OnEntityAdded(Entity entity, PhysicsComponent component)
        {
            World.Bodies.Add(component.Body);
            base.OnEntityAdded(entity, component);
        }

        protected override void OnEntityRemoved(Entity entity, PhysicsComponent component)
        {
            World.Bodies.Remove(component.Body);
            base.OnEntityRemoved(entity, component);
        }
    }
}
