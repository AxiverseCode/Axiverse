using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    public class Processor
    {
        public Type[] ComponentTypes { get; }

        public Dictionary<Guid, Entity> Entities { get; } = new Dictionary<Guid, Entity>();

        public Processor(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        public bool ContainsKey(Guid identifier)
        {
            return Entities.ContainsKey(identifier);
        }

        public bool Matches(Entity entity)
        {
            return ComponentTypes.All(t => entity.Components.ContainsKey(t));
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity.Identifier, entity);
        }

        public void Remove(Entity entity)
        {
            Entities.Remove(entity.Identifier);
        }
    }
}
