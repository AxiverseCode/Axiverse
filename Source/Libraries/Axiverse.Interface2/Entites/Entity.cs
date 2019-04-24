using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Models;

namespace Axiverse.Interface2.Entites
{
    public class Entity
    {
        public string Name { get; set; }

        public Scene Scene { get; internal set; }

        public Transform Transform { get; private set; }

        public ComponentCollection Components { get; }

        public Entity(string name)
        {
            Components = new ComponentCollection(this);
            Name = name;
        }

        public Entity() : this("")
        {

        }

        public Entity(string name, bool transformable = true) : this(name)
        {
            if (transformable)
            {
                Add(new Transform());
            }
        }

        public Entity(string name, Transform transform) : this(name)
        {
            Add(transform);
        }

        public Entity Add<T>(T component) where T: Component
        {
            Components.Add(typeof(T), component);
            return this;
        }

        public T Get<T>() where T: Component
        {
            Components.TryGetValue(typeof(T), out var value);
            return (T)value;
        }

        protected internal virtual void OnComponentAdded(ComponentEventArgs args)
        {
            if (args.Binding.Equals(typeof(Transform)))
            {
                Requires.That<ArgumentException>(Transform == null);
                Transform = (Transform)args.Component;
            }
            ComponentAdded?.Invoke(this, args);
        }

        protected internal virtual void OnComponentRemoved(ComponentEventArgs args)
        {
            if (args.Component == Transform)
            {
                Requires.That<ArgumentException>(args.Binding.Equals(typeof(Transform)));
                Transform = null;
            }
            ComponentRemoved?.Invoke(this, args);
        }

        public event ComponentEventHandler ComponentAdded;
        public event ComponentEventHandler ComponentRemoved;
    }
}
