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
        protected internal virtual void Initialize()
        {
            var m = typeof(Main);
        }

        /// <summary>
        /// Binds to the injector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        protected void Bind<T>(T value)
        {
            Injector.Bind(Key.From(typeof(T)), value);
        }
        
        /// <summary>
        /// Runs the application from the installation of the specified <see cref="Module"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public static void Run<T>(string[] args)
            where T : Module
        {
            var injector = Injector.Global;

            {
                var installer = new Installer(injector);
                installer.Install(typeof(T));
            }

            var main = injector.Resolve<Main>();
            main(args);
        }
    }
}
