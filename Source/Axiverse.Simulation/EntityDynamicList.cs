using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class EntityDynamicList<T> : IEntityDynamic, IEnumerable<T> where T : IEntityDynamic
    {
        private readonly List<T> list = new List<T>();



        public int Count => list.Count;

        public Entity Entity { get; set; }

        public EntityDynamicList(Entity entity)
        {
            Entity = entity;
        }

        public void Add(T item)
        {
            list.Add(item);
            item.Entity = Entity;
        }

        public bool Remove(T item)
        {
            item.Entity = null;
            return list.Remove(item);
        }

        public void Step(float delta)
        {
            foreach (T value in this)
            {
                value.Step(delta);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)list).GetEnumerator();
        }
    }
}
