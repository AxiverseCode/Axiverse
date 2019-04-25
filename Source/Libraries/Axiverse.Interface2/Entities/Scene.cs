using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entities
{
    public class Scene
    {
        public EntityCollection Entities { get; }

        public Scene()
        {
            Entities = new EntityCollection(this); 
        }

        public List<T> GetComponents<T>() where T : Component
        {
            var list = new List<T>();
            foreach (var entity in Entities)
            {
                if (entity.Components.TryGetValue(typeof(T), out var value))
                {
                    list.Add((T)value);
                }
            }
            return list;
        }

        protected internal void OnEntityAdded(EntityEventArgs args)
        {
            args.Entity.ComponentAdded += HandleComponentAdded;
            args.Entity.ComponentRemoved += HandleComponentRemoved;
            EntityAdded?.Invoke(this, args);
        }

        protected internal void OnEntityRemoved(EntityEventArgs args)
        {
            args.Entity.ComponentAdded -= HandleComponentAdded;
            args.Entity.ComponentRemoved -= HandleComponentRemoved;
            EntityRemoved?.Invoke(this, args);
        }

        protected internal void OnComponentAdded(ComponentEventArgs args)
        {
            ComponentAdded?.Invoke(this, args);
        }

        protected internal void OnComponentRemoved(ComponentEventArgs args)
        {
            ComponentRemoved?.Invoke(this, args);
        }

        private void HandleComponentRemoved(object sender, ComponentEventArgs args)
        {
            OnComponentAdded(args);
        }

        private void HandleComponentAdded(object sender, ComponentEventArgs args)
        {
            OnComponentRemoved(args);
        }

        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;
        public event ComponentEventHandler ComponentAdded;
        public event ComponentEventHandler ComponentRemoved;
    }
}
