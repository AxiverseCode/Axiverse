using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Simulation
{
    public class LogicStage : Stage
    {
        public ComponentObserver agentObserver;

        public LogicStage(Scene scene)
        {
            Scene = scene;
            agentObserver = new ComponentObserver(scene, typeof(Agent), typeof(Physical), typeof(Transform));
        }

        public override void Process(Clock clock)
        {
            foreach (var entity in agentObserver.Entities)
            {
                entity.Get<Agent>().Update(clock);
            }
        }
    }
}
