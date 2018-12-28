using Axiverse.Calibration;
using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Resources;
using Axiverse.Services.Proto;
using Grpc.Core;
using System;
using System.Windows.Forms;

namespace Calibration
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
            var calibrationProcess = new CalibrartionProcess(engine);
            engine.Process = calibrationProcess;

            engine.Initialize();
            calibrationProcess.OnInitialize();
            engine.Run();

            calibrationProcess.OnDispose();

        }

        static void Main(string[] args)
        {
            Run<Program>(args);
        }
    }
}
