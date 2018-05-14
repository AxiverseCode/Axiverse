using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
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

        public virtual void Process(EntityCollection entities, float dt)
        {
            foreach (var entity in entities)
            {
                // if (entity)
                Process(entity, dt);
            }
        }

        public abstract void Process(Entity entity, float dt);
    }
}
