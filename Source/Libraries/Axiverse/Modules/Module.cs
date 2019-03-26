using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;

namespace Axiverse.Modules
{
    /// <summary>
    /// Represents a loadable module.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// Gets the <see cref="Injector"/> to install the module onto.
        /// </summary>
        public Injector Injector { get; internal set; }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        protected internal abstract void Initialize();

        /// <summary>
        /// Binds to the injector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        protected void Bind<T>(T value)
        {
            Injector.Bind(Key.From(typeof(T)), value);
        }

        protected T Bind<T>()
        {
            var value = Binder.Activate<T>(Injector);
            Injector.Bind(Key.From(typeof(T)), value);
            return value;
        }

        protected T Activate<T>()
        {
            return Binder.Activate<T>(Injector);
        }
    }
}
