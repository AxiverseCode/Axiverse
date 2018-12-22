using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class CollectionComponent<T> : Component
        where T : Entity
    {
        private List<T> items = new List<T>();

        public int Count => items.Count;

        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }
    }
}
