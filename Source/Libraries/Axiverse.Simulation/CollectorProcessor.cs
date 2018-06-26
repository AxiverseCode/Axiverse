using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Represents a processor or system for entities.
    /// </summary>
    public class CollectorProcessor : IProcessor
    {
        /// <summary>
        /// Gets the stage of the processor determining the order of execution.
        /// </summary>
        public virtual ProcessorStage Stage => ProcessorStage.None;

        /// <summary>
        /// Gets the types of components which are required by this porcessor.
        /// </summary>
        public Type[] ComponentTypes { get; }

        /// <summary>
        /// Gets the dictionary of entities being watched and processed by this processor.
        /// </summary>
        public Dictionary<Guid, Entity> Entities { get; } = new Dictionary<Guid, Entity>();

        /// <summary>
        /// Constructs a processor listening to entities with the specified component types.
        /// </summary>
        /// <param name="componentTypes"></param>
        public CollectorProcessor(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        /// <summary>
        /// Determines whether the processor contains an entity with the given identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool ContainsKey(Guid identifier)
        {
            return Entities.ContainsKey(identifier);
        }

        /// <summary>
        /// Determins whether an entity has the required component types specified by the processor.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Matches(Entity entity)
        {
            return ComponentTypes.All(t => entity.Components.ContainsKey(t));
        }

        /// <summary>
        /// Adds an entity to the processor.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(Entity entity)
        {
            Entities.Add(entity.Identifier, entity);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(Entity entity)
        {
            Entities.Remove(entity.Identifier);
        }

        public virtual void Process(SimulationContext context)
        {

        }
    }
}
