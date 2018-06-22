using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProgramModule : Module
    {
        protected internal override void Initialize()
        {

        }

        public abstract void Execute(string[] args);

        /// <summary>
        /// Runs the application from the installation of the specified <see cref="Module"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public static void Run<T>(string[] args)
            where T : ProgramModule
        {
            var injector = Injector.Global;
            ProgramModule module = null;

            {
                var installer = new Installer(injector);
                module = installer.Install(typeof(T)) as ProgramModule;
            }

            module?.Execute(args);
        }
    }
}
