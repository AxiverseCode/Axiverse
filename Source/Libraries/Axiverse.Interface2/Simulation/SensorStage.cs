using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Simulation
{
    public class SensorStage : Stage
    {
        private ComponentObserver emitterObserver;
        private ComponentObserver sensorObserver;

        public SensorStage(Scene scene)
        {
            emitterObserver = new ComponentObserver(scene, typeof(Emitter), typeof(Transform));
            sensorObserver = new ComponentObserver(scene, typeof(Sensor), typeof(Transform));
        }

        public override void Process(Clock clock)
        {

        }
    }
}
