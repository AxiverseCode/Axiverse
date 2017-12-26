using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface
{
    public class Entity
    {
        public TransformComponent Transform { get; private set; }

        public Entity()
        {
            Transform = new TransformComponent();
        }

        public T GetComponent<T>() where T: class
        {
            return components[typeof(T)] as T;
        }

        public void AddComponent<T>(T component) where T : class
        {
            components.Add(typeof(T), component);
        }
        
        private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();
    }
}
