using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entites
{
    public class Scene
    {
        public List<Entity> Entities { get; } = new List<Entity>();

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

        protected void OnEntityAdded()
        {

        }

        protected void OnEntityRemoved()
        {

        }

        protected void OnComponentAdded()
        {

        }

        protected void OnComponentRemoved()
        {

        }

        public event EntityEventHandler EntityAdded;
        public event EntityEventHandler EntityRemoved;
        public event ComponentEventHandler ComponentAdded;
        public event ComponentEventHandler ComponentRemoved;
    }
}
