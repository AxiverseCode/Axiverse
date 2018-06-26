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
    public class ComponentDictionary
    {
        /// <summary>
        /// Gets the entity the <see cref="ComponentDictionary"/> is bound to.
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
                Requires.AssignableFrom(key, value);
                components[key] = value;
                OnComponentAdded(key, value);
            }
        }

        /// <summary>
        /// Gets the number of components.
        /// </summary>
        public int Count => components.Count;

        /// <summary>
        /// Gets the key collection.
        /// </summary>
        public Dictionary<Key, Component>.KeyCollection Keys => components.Keys;

        /// <summary>
        /// Gets the value collection.
        /// </summary>
        public Dictionary<Key, Component>.ValueCollection Values => components.Values;

        /// <summary>
        /// Constructs an <see cref="ComponentDictionary"/> bound to the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        public ComponentDictionary(Entity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Determinds if the collection contains the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool ContainsKey<T>()
        {
            return components.ContainsKey(Key.From<T>());
        }

        public bool ContainsKey(Type type)
        {
            return ContainsKey(Key.From(type));
        }

        public bool ContainsKey(Key key)
        {
            return components.ContainsKey(key);
        }

        public bool ContainsValue(Component value)
        {
            return components.ContainsValue(value);
        }

        /// <summary>
        /// Sets the component bound to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : Component
        {
            return (T)components[Key.From<T>()];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(out T component) where T : Component
        {
            bool result = components.TryGetValue(Key.From<T>(), out var value);
            component = value as T;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(Key key, out T component) where T:Component
        {
            Requires.AssignableFrom<T>(key);
            bool result = components.TryGetValue(key, out var value);
            component = value as T;
            return result;
        }

        /// <summary>
        /// Gets a component bound to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public T Add<T>(T component) where T : Component
        {
            Key key = Key.From<T>();
            Requires.AssignableFrom(key, component);
            components.Add(key, component);
            OnComponentAdded(key, component);
            return component;
        }

        public void Add(Key key, Component component)
        {
            Requires.AssignableFrom(key, component);
            components.Add(key, component);
            OnComponentAdded(key, component);
        }

        public bool Remove<T>() where T : Component
        {
            if (TryGetValue<T>(out var component))
            {
                Key key = Key.From<T>();
                components.Remove(key);
                OnComponentRemoved(key, component);
                return true;
            }
            return false;
        }

        public ComponentDictionary Clone(Entity entity)
        {
            var collection = new ComponentDictionary(entity);
            foreach (var pair in components)
            {
                var clone = pair.Value.Clone();
                Requires.That<InvalidCastException>(pair.Key.IsAssignableFrom(clone));
                collection.components.Add(pair.Key, clone);
            }
            return collection;
        }

        protected void OnComponentAdded(Key key, Component component)
        {
            if (Entity != null)
            {
                Entity.OnComponentAdded(new ComponentEventArgs(key, component));
            }
        }

        protected void OnComponentRemoved(Key key, Component component)
        {
            if (Entity != null)
            {
                Entity.OnComponentRemoved(new ComponentEventArgs(key, component));
            }
        }

        private readonly Dictionary<Key, Component> components = new Dictionary<Key, Component>();
    }
}
