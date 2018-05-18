using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;
using Axiverse.Modules;
using Axiverse.Interface.Engine;
using Axiverse.Resources;
namespace HelloGraphics
{
    class Program : IProgram
    {
        static void Main(string[] args)
        {
            Injector.Global.Bind<IProgram>(new Program());
            // Set the load root to Axiverse\
            Injector.Global.Bind(new Library(@"..\..\..\..\..\"));
            ModuleProgram.Run(args);

        }

        public void Run(string[] args)
        {
            Engine engine = new Engine();
            engine.Initialize();

            engine.Run();
        }
    }
}
