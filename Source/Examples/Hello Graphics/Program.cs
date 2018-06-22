using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Resources;

namespace HelloGraphics
{
    class Program
    {
        static void Main(string[] args)
        {
            Injector.Global.Bind(new Main(Run));
            // Set the load root to Axiverse\
            Injector.Global.Bind(new Library(@"..\..\..\..\..\"));
            Module.Run<Module>(args);
        }

        public static void Run(string[] args)
        {
            Engine engine = new Engine();
            engine.Initialize();

            engine.Run();
        }
    }
}
