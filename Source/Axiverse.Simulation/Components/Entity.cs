using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class Entity
    {
        public Guid Identifier { get; }

        public String Name { get; set; }

        public Entity() : this(Guid.NewGuid())
        {

        }

        public Entity(Guid identifier)
        {
            Identifier = identifier;
        }

        public Component this[Type key]
        {
            get
            {
                return m_components[key];
            }
            set
            {
                if (!key.IsAssignableFrom(value.GetType()))
                {
                    throw new ArgumentException("Component must be assignable to type.");
                }
                if (value.Entity != this && value.Entity != null)
                {
                    throw new ArgumentException("Component cannot be assigned to another entity.");
                }
                m_components[key] = value;
            }
        }

        public int Count => m_components.Count;

        public T Component<T>() where T: Component
        {
            return this[typeof(T)] as T;
        }

        public void Add<T>(T value) where T : Component
        {
            if (value.Entity != this && value.Entity != null)
            {
                throw new ArgumentException("Component cannot be assigned to another entity.");
            }
            m_components.Add(typeof(T), value);
            value.Entity = this;
            OnComponentAdded(typeof(T), value);
        }

        public void Add(Type key, Component value)
        {
            if (!key.IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException("Component must be assignable to type");
            }
            if (value.Entity != this && value.Entity != null)
            {
                throw new ArgumentException("Component cannot be assigned to another entity.");
            }
            m_components.Add(key, value);
            value.Entity = this;
            OnComponentAdded(key, value);
        }

        public bool Remove(Type key)
        {
            if (m_components.TryGetValue(key, out var value))
            {
                value.Entity = null;
                m_components.Remove(key);
                OnComponentRemoved(key, value);
                return true;
            }
            return false;
        }

        public bool ContainsKey(Type key)
        {
            return m_components.ContainsKey(key);
        }

        public bool ContainsValue(Component value)
        {
            return m_components.ContainsValue(value);
        }

        public bool TryGetValue<T>(out T value) where T : Component
        {
            bool found = m_components.TryGetValue(typeof(T), out var result);
            value = result as T;
            return found;
        }

        public bool TryGetValue(Type key, out Component value)
        {
            return m_components.TryGetValue(key, out value);
        }

        protected virtual void OnComponentAdded(Type type, Component component)
        {
            // sync models

            foreach (var model in models)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (property.PropertyType == type && Attribute.IsDefined(property, typeof(ComponentAttribute)))
                    {
                        property.SetValue(model, component);
                    }
                }
            }
        }

        protected virtual void OnComponentRemoved(Type type, Component component)
        {
            // sync models
            foreach (var model in models)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (property.PropertyType == type && Attribute.IsDefined(property, typeof(ComponentAttribute)))
                    {
                        property.SetValue(model, null);
                    }
                }
            }
        }
        
        protected virtual void OnModelAdded(Model model)
        {
            // sync components
            foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(property, typeof(ComponentAttribute)))
                {
                    if (!typeof(Component).IsAssignableFrom(property.PropertyType))
                    {
                        throw new ArgumentException("All properties labeled with [Component] must be of a type inheriting from Component");
                    }

                    if (property.CanRead && property.GetValue(model) is Component value)
                    {
                        Add(property.PropertyType, value);
                    }
                    else if (property.CanWrite && TryGetValue(property.PropertyType, out var component))
                    {
                        property.SetValue(model, component);
                    }
                }
            }
        }

        private readonly Dictionary<Type, Component> m_components = new Dictionary<Type, Component>();
        private readonly List<Model> models = new List<Model>();
    }
}
