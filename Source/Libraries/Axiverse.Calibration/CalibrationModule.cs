using Axiverse.Injection;
using Axiverse.Interface.Engine;
using Axiverse.Modules;
using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    [Dependency(typeof(EngineModule))]
    public class CalibrationModule : Module
    {
        [Inject]
        public CalibrationModule(Universe universe)
        {
            universe.Add(new ControllerProcessor());
            universe.Add(new PilotingProcessor());
        }

        protected override void Initialize()
        {

        }
    }
}
