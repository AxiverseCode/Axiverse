using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Processor for transforming entities with the given set of components.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class EntityProcessor<TComponent, TData>
    {
        public EntityProcessor(params Type[] requiredComponents)
        {
            required = requiredComponents;
        }

        private Type[] required;
    }
}
