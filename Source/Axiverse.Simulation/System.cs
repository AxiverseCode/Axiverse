using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.SimpleEntity
{
    /// <summary>
    /// A system which acts over entities
    /// </summary>
    public abstract class System
    {
        public Type[] RelevantTypes { get; }

        protected System(params Type[] relevantTypes)
        {
            RelevantTypes = relevantTypes;
        }

        public virtual void Process(EntityCollection entities)
        {
            foreach (var entity in entities)
            {
               // if (entity)
            }
        }

        public abstract void Process(Entity entity);
    }
}
