using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// 
    /// </summary>
    public class BindingDictionary : IBindingProvider
    {
        public object this[Key key]
        {
            get
            {
                return bindings[key];
            }
            set
            {
                Preconditions.Requires<InvalidCastException>(key.Type.IsAssignableFrom(value.GetType()));
                bindings[key] = value;
            }
        }

        public void Add<T>(T value)
        {
            bindings.Add(Key.From(typeof(T)), value);
        }

        public void Add(Key key, object value)
        {
            Preconditions.Requires<InvalidCastException>(key.Type.IsAssignableFrom(value.GetType()));
            bindings.Add(key, value);
        }

        public bool Remove(Key key)
        {
            return bindings.Remove(key);
        }

        public T Get<T>(Key key) where T : class
        {
            return bindings[key] as T;
        }

        public bool TryGetValue(Key key, out object value)
        {
            return bindings.TryGetValue(key, out value);
        }

        public bool ContainsKey(Key key)
        {
            return bindings.ContainsKey(key);
        }

        private readonly Dictionary<Key, object> bindings = new Dictionary<Key, object>();
    }
}
