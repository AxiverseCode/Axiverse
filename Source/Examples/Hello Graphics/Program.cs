using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Resources;

namespace HelloGraphics
{
    [Dependency(typeof(ResourceModule))]
    [Dependency(typeof(EngineModule))]
    public class Program : ProgramModule
    {
        [Bind]
        Engine engine;

        [Inject]
        public Program(Library library)
        {
            library.BasePath = @"..\..\..\..\..\";
        }

        public override void Execute(string[] args)
        {
            engine.Initialize();
            engine.Run();
        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
