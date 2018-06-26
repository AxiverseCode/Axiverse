using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    // Stages
    // 0    - Critical First
    // 100  - Preprocessing
    // 200  - 

    public enum ProcessorStage
    {
        None = 0,
        Critical = 1,

        Preprocessing = 1000,

        Components = 2000,

        Physics = 3000,

        Reconciliation = 4000,

        Propagation = 5000,

        Final = 9000,
    }


    /// <summary>
    /// Represents a processor or system for entities.
    /// </summary>
    public class Processor
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
        /// Gets whether this processor is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Constructs a processor listening to entities with the specified component types.
        /// </summary>
        /// <param name="componentTypes"></param>
        public Processor(params Type[] componentTypes)
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
        public void Add(Entity entity)
        {
            Entities.Add(entity.Identifier, entity);
            OnEntityAdded(entity);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Entity entity)
        {
            Entities.Remove(entity.Identifier);
            OnEntityRemoved(entity);
        }

        public virtual void Process()
        {
            if (Enabled)
            {
                foreach (var entity in Entities.Values)
                {
                    ProcessEntity(entity);
                }
            }
        }

        public virtual void ProcessEntity(Entity entity)
        {

        }

        protected virtual void OnEntityAdded(Entity entity)
        {

        }

        protected virtual void OnEntityRemoved(Entity entity)
        {

        }
    }

    public class Processor<T1> : Processor 
        where T1 : Component
    {
        public Processor() : base(typeof(T1))
        {

        }

        public override void ProcessEntity(Entity entity)
        {
            var component1 = entity.Components.Get<T1>();
            ProcessEntity(entity, component1);
        }

        public virtual void ProcessEntity(Entity entity, T1 component)
        {

        }

        protected virtual void OnEntityAdded(Entity entity, T1 component1)
        {

        }

        protected virtual void OnEntityRemoved(Entity entity, T1 component1)
        {

        }

        protected override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);
            var component1 = entity.Components.Get<T1>();
            OnEntityAdded(entity, component1);
        }

        protected override void OnEntityRemoved(Entity entity)
        {
            base.OnEntityRemoved(entity);
            var component1 = entity.Components.Get<T1>();
            OnEntityRemoved(entity, component1);
        }
    }

    public class Processor<T1, T2> : Processor
        where T1 : Component
        where T2 : Component
    {
        public Processor() : base(typeof(T1), typeof(T2))
        {

        }

        public override void ProcessEntity(Entity entity)
        {
            var component1 = entity.Components.Get<T1>();
            var component2 = entity.Components.Get<T2>();
            ProcessEntity(entity, component1, component2);
        }

        public virtual void ProcessEntity(Entity entity, T1 component1, T2 component2)
        {

        }

        protected virtual void OnEntityAdded(Entity entity, T1 component1, T2 component2)
        {

        }

        protected virtual void OnEntityRemoved(Entity entity, T1 component1, T2 component2)
        {

        }

        protected override void OnEntityAdded(Entity entity)
        {
            base.OnEntityAdded(entity);
            var component1 = entity.Components.Get<T1>();
            var component2 = entity.Components.Get<T2>();
            OnEntityAdded(entity, component1, component2);
        }

        protected override void OnEntityRemoved(Entity entity)
        {
            base.OnEntityRemoved(entity);
            var component1 = entity.Components.Get<T1>();
            var component2 = entity.Components.Get<T2>();
            OnEntityRemoved(entity, component1, component2);
        }
    }
}
