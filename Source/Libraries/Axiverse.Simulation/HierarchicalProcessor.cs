using Axiverse.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class HierarchicalProcessor<T> : Processor<T>
        where T : HierarchicalComponent<T>
    {
        public override void Process(SimulationContext context)
        {
            foreach (var entity in Entities.Values)
            {
                var components = GetComponents(entity);
                if (components.Item1.Parent == null)
                {
                    ProcessTraverse(context, entity, components.Item1);
                }
            }
        }

        public virtual void ProcessTraverse(SimulationContext context, Entity entity, T component)
        {
            ProcessEntity(context, entity, component);

            foreach (var child in component.Children)
            {
                ProcessTraverse(context, child.Entity, child);
            }

            ProcessedEntity(context, entity, component);
        }

        /// <summary>
        /// Processes an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        public virtual void ProcessedEntity(SimulationContext context, Entity entity, T component)
        {

        }
    }
}
