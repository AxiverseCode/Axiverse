using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Represents a collection of components bound to an entity.
    /// </summary>
    public class ComponentCollection
    {
        public Component this[Type type]
        {
            get
            {
                return components[Key.From(type)];
            }
            set
            {
                var key = Key.From(type);
                Contract.Requires<InvalidCastException>(key.IsAssignableFrom(value));
                components[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of components 
        /// </summary>
        public int Count => components.Count;

        public ComponentCollection Clone()
        {
            var collection = new ComponentCollection();
            foreach (var pair in components)
            {
                var clone = pair.Value.Clone();
                Contract.Requires<InvalidCastException>(pair.Key.IsAssignableFrom(clone));
                collection.components.Add(pair.Key, clone);
            }
            return collection;
        }

        private readonly Dictionary<Key, Component> components = new Dictionary<Key, Component>();
    }
}
