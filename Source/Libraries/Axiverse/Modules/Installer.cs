using Axiverse.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Axiverse.Modules
{
    public class Installer
    {
        /// <summary>
        /// Gets the <see cref="Injector"/> this being installed into.
        /// </summary>
        public Injector Injector { get; private set; }

        /// <summary>
        /// Creates an <see cref="Installer"/> with the global injector.
        /// </summary>
        public Installer()
        {
            Injector = Injector.Global;
        }

        /// <summary>
        /// Constructs an <see cref="Installer"/> with the specified injector.
        /// </summary>
        /// <param name="injector"></param>
        public Installer(Injector injector)
        {
            Injector = injector;
        }

        /// <summary>
        /// Installs a module type
        /// </summary>
        /// <param name="type"></param>
        public void Install(Type type)
        {
            Requires.AssignableFrom<Module>(type);
            if (modules.Contains(type))
            {
                return;
            }

            var dependencies = GetDependencies(type);
            foreach (var dependency in dependencies)
            {
                Install(dependency);
            }

            var module = Injection.Binder.Activate(type, Injector.Bindings) as Module;
            module.Injector = Injector;
            module.Initialize();

            modules.Add(type);
        }

        /// <summary>
        /// Gets the dependency types for a module type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Type[] GetDependencies(Type type)
        {
            return type.GetCustomAttributes<DependencyAttribute>(true)
                .Select(a => a.ModuleType)
                .ToArray();
        }


        private readonly HashSet<Type> modules = new HashSet<Type>();
    }
}
