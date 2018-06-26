using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class TransformProcessor : Processor<TransformComponent>
    {
        public override void ProcessEntity(SimulationContext context, Entity entity, TransformComponent component)
        {
            component.LocalTransform = Matrix4.Transformation(component.Scaling, component.Rotation, component.Translation);
            component.GlobalTransform = component.LocalTransform;
        }
    }
}
