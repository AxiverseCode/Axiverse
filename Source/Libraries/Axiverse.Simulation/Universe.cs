using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Components;
using Axiverse.Simulation.Systems;

namespace Axiverse.Simulation
{
    public class Universe
    {
        public Entity this[Guid identifier]
        {
            get => entities[identifier];
        }

        public int Count => entities.Count;

        public Universe()
        {
            for (int i = 0; i < 5; i++)
            {
                var entity = new Entity();
                entity.Components.Add(new NavigationComponent());
                entities.Add(entity.Identifier, entity);
            }

            systems.Add(new NavigationSystem());
            systems.Add(new SpatialSystem());
        }

        public void Post(Guid target)
        {

        }

        public void Step(float dt)
        {
            foreach (var system in systems)
            {
                foreach (var entity in entities.Values)
                {
                    system.Process(entity, dt);
                }
            }

            foreach (var entity in entities.Values)
            {
                //entity.Model?.Process(entity);
                //Console.WriteLine(entity);
            }
        }

        public void Add(Entity entity)
        {
            Requires.That<InvalidOperationException>(!entity.IsAttached);

            entity.Universe = this;
            entities.Add(entity.Identifier, entity);
            OnEntityAdded(entity);
        }

        public bool Remove(Entity entity)
        {
            var result = entities.Remove(entity.Identifier);
            if (result)
            {
                OnEntityRemoved(entity);
            }
            return result;
        }

        protected void OnEntityAdded(Entity entity)
        {
            entity.ComponentAdded += OnComponentAdded;
            foreach (var processor in processors.Values)
            {
                if (processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }

            EntityAdded?.Invoke(this, new EntityEventArgs(entity));
        }

        protected void OnEntityRemoved(Entity entity)
        {
            entity.ComponentRemoved += OnComponentRemoved;
            foreach (var processor in processors.Values)
            {
                if (processor.ContainsKey(entity.Identifier))
                {
                    processor.Remove(entity);
                }
            }

            EntityRemoved?.Invoke(this, new EntityEventArgs(entity));
        }

        private void OnComponentAdded(object sender, ComponentEventArgs e)
        {
            var entity = sender as Entity;

            foreach (var processor in processors.Values)
            {
                if (!processor.ContainsKey(entity.Identifier) && processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs e)
        {
            var entity = sender as Entity;

            foreach (var processor in processors.Values)
            {
                if (processor.ContainsKey(entity.Identifier) && !processor.Matches(entity))
                {
                    processor.Remove(entity);
                }
            }
        }

        public void Add(Processor processor)
        {
            Processors.Add(processor.Stage, processor);

            foreach (var entity in entities.Values)
            {
                if (processor.Matches(entity))
                {
                    processor.Add(entity);
                }
            }
        }

        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;

        public SortedList<int, Processor> Processors => processors;
        
        private readonly SortedList<int, Processor> processors = new SortedList<int, Processor>();
        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
        private readonly List<System> systems = new List<System>();
    }
}
