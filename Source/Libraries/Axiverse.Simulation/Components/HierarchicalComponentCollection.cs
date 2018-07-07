using Axiverse.Collections;
using Axiverse.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HierarchicalComponentCollection<T> : TrackedList<T>
        where T : HierarchicalComponent<T>
    {
        public T Component { get; }

        public HierarchicalComponentCollection(T component)
        {
            Requires.IsNotNull(component);
            Component = component;
        }

        public void Add(Entity entity)
        {
            var component = entity.Components.Get<T>();
            Add(component);
        }

        public override void OnItemAdded(T item)
        {
            Requires.IsNull(item.Parent);
            item.Parent = Component;
        }

        public override void OnItemRemoved(T item)
        {
            item.Parent = null;
        }
    }
}
