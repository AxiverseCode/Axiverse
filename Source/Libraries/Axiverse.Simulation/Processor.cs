using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;
namespace Axiverse.Simulation
{
    /// <summary>
    /// Processor for entities with the given components.
    /// </summary>
    public class Processor : CollectorProcessor
    {
        /// <summary>
        /// Constructs a processor.
        /// </summary>
        public Processor(Type[] componentTypes) : base(componentTypes) { }

        /// <summary>
        /// Processes the entities.
        /// </summary>
        /// <param name="context"></param>
        public override void Process(SimulationContext context)
        {
            foreach (var entity in Entities.Values)
            {
                ProcessEntity(context, entity);
            }
        }

        /// <summary>
        /// Processes an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        public virtual void ProcessEntity(SimulationContext context, Entity entity)
        {

        }
        
        /// <summary>
        /// Adds an entity to the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Entity entity)
        {
            base.Add(entity);
            OnEntityAdded(entity);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Remove(Entity entity)
        {
            base.Remove(entity);
            OnEntityRemoved(entity);
        }

        /// <summary>
        /// When an entity is added.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnEntityAdded(Entity entity)
        {

        }

        /// <summary>
        /// When an entity is removed.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void OnEntityRemoved(Entity entity)
        {

        }
    }

    /// <summary>
    /// Processor for entities with the given components.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public class Processor<T1> : CollectorProcessor 
        where T1 : Component
    {
        /// <summary>
        /// Constructs a processor.
        /// </summary>
        public Processor() : base(typeof(T1)) { }

        /// <summary>
        /// Processes the entities.
        /// </summary>
        /// <param name="context"></param>
        public override void Process(SimulationContext context)
        {
            foreach (var entity in Entities.Values)
            {
                var components = GetComponents(entity);
                ProcessEntity(context, entity, components.Item1);
            }
        }

        /// <summary>
        /// Processes an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        public virtual void ProcessEntity(SimulationContext context, Entity entity, T1 component)
        {

        }
        
        /// <summary>
        /// Adds an entity to the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Entity entity)
        {
            base.Add(entity);
            var components = GetComponents(entity);
            OnEntityAdded(entity, components.Item1);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Remove(Entity entity)
        {
            base.Remove(entity);
            var components = GetComponents(entity);
            OnEntityRemoved(entity, components.Item1);
        }

        /// <summary>
        /// When an entity is added.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        protected virtual void OnEntityAdded(Entity entity, T1 component1)
        {

        }

        /// <summary>
        /// When an entity is removed.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        protected virtual void OnEntityRemoved(Entity entity, T1 component1)
        {

        }

        /// <summary>
        /// Gets the components from an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Tuple<T1> GetComponents(Entity entity)
        {
            return new Tuple<T1> (
                entity.Components.Get<T1>());
        }
    }

    /// <summary>
    /// Processor for entities with the given components.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Processor<T1, T2> : CollectorProcessor
        where T1 : Component
        where T2 : Component
    {
        /// <summary>
        /// Constructs a processor.
        /// </summary>
        public Processor() : base(typeof(T1), typeof(T2)) { }

        /// <summary>
        /// Processes the entities.
        /// </summary>
        /// <param name="context"></param>
        public override void Process(SimulationContext context)
        {
            foreach (var entity in Entities.Values)
            {
                var components = GetComponents(entity);
                ProcessEntity(context, entity, components.Item1, components.Item2);
            }
        }

        /// <summary>
        /// Processes an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        public virtual void ProcessEntity(
            SimulationContext context,
            Entity entity,
            T1 component1,
            T2 component2)
        {

        }

        /// <summary>
        /// Adds an entity to the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Entity entity)
        {
            base.Add(entity);
            var components = GetComponents(entity);
            OnEntityAdded(entity, components.Item1, components.Item2);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Remove(Entity entity)
        {
            base.Remove(entity);
            var components = GetComponents(entity);
            OnEntityRemoved(entity, components.Item1, components.Item2);
        }

        /// <summary>
        /// When an entity is added.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        protected virtual void OnEntityAdded(Entity entity, T1 component1, T2 component2)
        {

        }

        /// <summary>
        /// When an entity is removed.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        protected virtual void OnEntityRemoved(Entity entity, T1 component1, T2 component2)
        {

        }

        /// <summary>
        /// Gets the components from an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Tuple<T1, T2> GetComponents(Entity entity)
        {
            return new Tuple<T1, T2>(
                entity.Components.Get<T1>(),
                entity.Components.Get<T2>());
        }
    }

    /// <summary>
    /// Processor for entities with the given components.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class Processor<T1, T2, T3> : CollectorProcessor
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        /// <summary>
        /// Constructs a processor.
        /// </summary>
        public Processor() : base(typeof(T1), typeof(T2), typeof(T3)) { }

        /// <summary>
        /// Processes the entities.
        /// </summary>
        /// <param name="context"></param>
        public override void Process(SimulationContext context)
        {
            foreach (var entity in Entities.Values)
            {
                var components = GetComponents(entity);
                ProcessEntity(context, entity,
                    components.Item1,
                    components.Item2,
                    components.Item3);
            }
        }

        /// <summary>
        /// Processes an entity.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        /// <param name="component3"></param>
        public virtual void ProcessEntity(
            SimulationContext context,
            Entity entity,
            T1 component1,
            T2 component2,
            T3 component3)
        {

        }

        /// <summary>
        /// Adds an entity to the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Entity entity)
        {
            base.Add(entity);
            var components = GetComponents(entity);
            OnEntityAdded(entity, components.Item1, components.Item2, components.Item3);
        }

        /// <summary>
        /// Removes an entity from the processor.
        /// </summary>
        /// <param name="entity"></param>
        public override void Remove(Entity entity)
        {
            base.Remove(entity);
            var components = GetComponents(entity);
            OnEntityRemoved(entity, components.Item1, components.Item2, components.Item3);
        }

        /// <summary>
        /// When an entity is added.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        /// <param name="component3"></param>
        protected virtual void OnEntityAdded(Entity entity, T1 component1, T2 component2, T3 component3)
        {

        }

        /// <summary>
        /// When an entity is removed.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component1"></param>
        /// <param name="component2"></param>
        /// <param name="component3"></param>
        protected virtual void OnEntityRemoved(Entity entity, T1 component1, T2 component2, T3 component3)
        {

        }

        /// <summary>
        /// Gets the components from an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Tuple<T1, T2, T3> GetComponents(Entity entity)
        {
            return new Tuple<T1, T2, T3>(
                entity.Components.Get<T1>(),
                entity.Components.Get<T2>(),
                entity.Components.Get<T3>());
        }
    }
}
