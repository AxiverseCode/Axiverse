using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class Processor
    {
        public void Advance()
        {

        }
    }

    /// <summary>
    /// Processor for transforming entities with the given set of components.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class Processor<TComponent, TData> : Processor
    {
        public Processor(params Type[] requiredComponents)
        {
            required = requiredComponents;
        }

        private Type[] required;
    }
}
