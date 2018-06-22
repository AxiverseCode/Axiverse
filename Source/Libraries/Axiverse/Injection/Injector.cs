using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    public class Injector
    {
        public BindingDictionary Bindings { get; } = new BindingDictionary();
        
        /// <summary>
        /// Gets or sets whether injects should automatically cascade into creating other injects.
        /// </summary>
        public bool Cascade { get; set; }

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

        public void Bind<T>(T value)
        {
            Bindings.Add(value);
        }

        public void Bind(Key key, object value)
        {
            Bindings.Add(key, value);
        }

        public void Bind<T>(string name, T value)
        {
            Bindings.Add(Key.From(typeof(T), name), value);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(Key.From(typeof(T)));
        }

        public T Resolve<T>(Key key)
        {
            Contract.Requires<InvalidCastException>(typeof(T).IsAssignableFrom(key.Type));
            return (T)Resolve(key);
        }

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
            if (Cascade)
            {
                // TODO(axiverse): install on arbitrary key?
                // return Inject(key.Type);
            }

            return null;
        }

        /// <summary>
        /// Gets the global injector for the application.
        /// </summary>
        public static Injector Global { get; } = new Injector();
    }
}
