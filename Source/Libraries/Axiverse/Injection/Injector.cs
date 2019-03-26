using System;
using System.Diagnostics.Contracts;

namespace Axiverse.Injection
{
    /// <summary>
    /// 
    /// </summary>
    public class Injector : IBindingProvider
    {
        private BindingDictionary Bindings { get; } = new BindingDictionary();
        
        /// <summary>
        /// Gets or sets whether absent keys should be automatically activated.
        /// </summary>
        public bool Activate { get; set; }

        /// <summary>
        /// Gets or sets a fallback injector to use if a binding is not found in this injector.
        /// </summary>
        public Injector Fallback { get; set; }

        /// <summary>
        /// Constructs an injector with the injector bound.
        /// </summary>
        public Injector()
        {
            Bind(this);
        }

        /// <summary>
        /// Binds the specified value using the default typed binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Bind<T>(T value)
        {
            Bindings.Add(value);
        }

        /// <summary>
        /// Binds the specified value using the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Bind(Key key, object value)
        {
            Bindings.Add(key, value);
        }

        /// <summary>
        /// Binds the specified value using the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Bind<T>(string name, T value)
        {
            Bindings.Add(Key.From(typeof(T), name), value);
        }

        /// <summary>
        /// Resolves a binding using the default typed key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(Key.From(typeof(T)));
        }

        /// <summary>
        /// Resolves a binding using the given key. The type of the key must match the templated
        /// type of the method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Resolve<T>(Key key)
        {
            Contract.Requires<InvalidCastException>(typeof(T).IsAssignableFrom(key.Type));
            return (T)Resolve(key);
        }

        /// <summary>
        /// Resolves a binding using the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Resolve(Key key)
        {
            // Try resolving from bound objects.
            if (Bindings.TryGetValue(key, out var binding))
            {
                return binding;
            }

            // Try resolving from fallback injector.
            if (Fallback != null && 
                (binding = Fallback.Resolve(key)) != null)
            {
                return binding;
            }

            // If cascade is enabled try to inject the type.
            if (Activate)
            {
                var constructed = Binder.Activate(key.Type, this);
                Bind(key, constructed);
                return constructed;
            }

            return null;
        }

        /// <summary>
        /// Resolves the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object IBindingProvider.this[Key key] => Resolve(key);

        /// <summary>
        /// Gets the global injector for the application.
        /// </summary>
        public static Injector Global { get; } = new Injector();
    }
}
