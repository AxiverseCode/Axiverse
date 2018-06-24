using System;
using System.Collections.Generic;

namespace Axiverse.Injection
{
    /// <summary>
    /// Represents a collection of key/value pairs.
    /// </summary>
    public class BindingDictionary : IBindingProvider
    {
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[Key key]
        {
            get
            {
                return bindings[key];
            }
            set
            {
                Requires.AssignableFrom(key, value);
                bindings[key] = value;
            }
        }

        /// <summary>
        /// Adds a binding to the given value keyed on its type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Add<T>(T value)
        {
            bindings.Add(Key.From(typeof(T)), value);
        }

        /// <summary>
        /// Adds a binding to the value with the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidCastException">
        /// Throws if the value can't be cast to the type specified by the key.
        /// </exception>
        public void Add(Key key, object value)
        {
            Requires.AssignableFrom(key, value);
            bindings.Add(key, value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="BindingDictionary"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(Key key)
        {
            return bindings.Remove(key);
        }

        /// <summary>
        /// Gets the element with the specified key from the <see cref="BindingDictionary"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(Key key)
        {
            Requires.AssignableFrom<T>(key);
            return (T)bindings[key];
        }

        public T Get<T>()
        {
            return (T)bindings[Key.From<T>()];
        }

        public T Or<T>(Key key, T @default)
        {
            if (TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return @default;
        }

        public T OrDefault<T>(Key key) where T : class
        {
            if (TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default(T);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(Key key, out object value)
        {
            return bindings.TryGetValue(key, out value);
        }

        /// <summary>
        /// Determines whether the <see cref="BindingDictionary"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Key key)
        {
            return bindings.ContainsKey(key);
        }

        private readonly Dictionary<Key, object> bindings = new Dictionary<Key, object>();
    }
}
