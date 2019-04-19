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
        public Transform Transform { get; set; }
        public Model Model { get; set; }

        public Dictionary<Type, Component> Components { get; } = new Dictionary<Type, Component>();

        public Entity()
        {
            Transform = new Transform();
            Add(Transform);
        }

        public void Add<T>(T component) where T: Component
        {
            component.Entity = this;
            Components.Add(typeof(T), component);
        }

        public T Get<T>() where T: Component
        {
            Components.TryGetValue(typeof(T), out var value);
            return (T)value;
        }
    }
}
