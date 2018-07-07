using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class TransformProcessor : HierarchicalProcessor<TransformComponent>
    {
        public override void ProcessEntity(SimulationContext context, Entity entity, TransformComponent component)
        {
            component.LocalTransform = Matrix4.Transformation(component.Scaling, component.Rotation, component.Translation);
            if (component.Parent == null)
            {
                component.GlobalTransform = component.LocalTransform;
            }
            else
            {
                
                switch (component.Inheritance)
                {
                    case TransformInheritance.All:
                        component.GlobalTransform = component.Parent.GlobalTransform * component.LocalTransform;
                        break;
                    case TransformInheritance.Translation:
                        var global = component.LocalTransform;
                        global.Row(3, global.Row(3) + component.Parent.GlobalTransform.Row(3));
                        global.M44 = 1;
                        component.GlobalTransform = global;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
