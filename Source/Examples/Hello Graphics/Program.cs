using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Resources;

namespace HelloGraphics
{
    [Dependency(typeof(ResourceModule))]
    public class Program : Module
    {
        [Inject]
        public Program(Library library)
        {
            library.BasePath = @"..\..\..\..\..\";
        }

        protected override void Initialize()
        {
            Injector.Global.Bind(new Main(Run));
        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }

        public static void Run(string[] args)
        {
            Engine engine = new Engine();
            engine.Initialize();

            engine.Run();
        }
    }
}
