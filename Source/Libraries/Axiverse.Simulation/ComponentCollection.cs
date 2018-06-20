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
        /// <summary>
        /// Gets the entity the <see cref="ComponentCollection"/> is bound to.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Gets or sets a component by the type of the component.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Component this[Type type]
        {
            get
            {
                return components[Key.From(type)];
            }
            set
            {
                var key = Key.From(type);
                Preconditions.Requires<InvalidCastException>(key.IsAssignableFrom(value));
                components[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of components 
        /// </summary>
        public int Count => components.Count;

        /// <summary>
        /// Constructs an <see cref="ComponentCollection"/> bound to the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        public ComponentCollection(Entity entity)
        {
            Entity = entity;
        }

        public ComponentCollection Clone(Entity entity)
        {
            var collection = new ComponentCollection(entity);
            foreach (var pair in components)
            {
                var clone = pair.Value.Clone();
                Preconditions.Requires<InvalidCastException>(pair.Key.IsAssignableFrom(clone));
                collection.components.Add(pair.Key, clone);
            }
            return collection;
        }

        private readonly Dictionary<Key, Component> components = new Dictionary<Key, Component>();
    }
}
