using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entities
{
    public class ComponentObserver
    {
        public List<Entity> Entities { get; } = new List<Entity>();
        public Scene Scene { get; private set; }
        public Type[] Bindings { get; private set; }

        public ComponentObserver(Scene scene, params Type[] bindings)
        {
            Scene = scene;
            Bindings = bindings;

            scene.ComponentAdded += HandleComponentAdded;
            scene.ComponentRemoved += HandleComponentRemoved;
            scene.EntityAdded += HandleEntityAdded;
            scene.EntityRemoved += HandleEntityRemoved;

            foreach (var entity in scene.Entities)
            {
                bool complete = true;
                foreach (var binding in Bindings)
                {
                    if (!entity.Components.TryGetValue(binding, out var value))
                    {
                        complete = false;
                        break;
                    }
                }

                if (complete)
                {
                    Entities.Add(entity);
                }
            }
        }

        protected void OnEntityAdded(EntityEventArgs args)
        {
            EntityAdded?.Invoke(this, args);
        }

        protected void OnEntityRemoved(EntityEventArgs args)
        {
            EntityRemoved?.Invoke(this, args);
        }

        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;

        private void HandleEntityRemoved(object sender, EntityEventArgs args)
        {
            var removed = Entities.Remove(args.Entity);
            if (removed)
            {
                OnEntityRemoved(args);
            }
        }

        private void HandleEntityAdded(object sender, EntityEventArgs args)
        {
            foreach (var binding in Bindings)
            {
                if (!args.Entity.Components.TryGetValue(binding, out var value))
                {
                    return;
                }
            }

            Entities.Add(args.Entity);
            OnEntityAdded(args);
        }

        private void HandleComponentRemoved(object sender, ComponentEventArgs args)
        {
            if (Bindings.Contains(args.Binding))
            {
                Entities.Remove(args.Entity);
            }
        }

        private void HandleComponentAdded(object sender, ComponentEventArgs args)
        {
            if (Bindings.Contains(args.Binding))
            {
                foreach (var binding in Bindings)
                {
                    if (!args.Entity.Components.TryGetValue(binding, out var value))
                    {
                        return;
                    }
                }

                Entities.Add(args.Entity);
            }
        }
    }
}
