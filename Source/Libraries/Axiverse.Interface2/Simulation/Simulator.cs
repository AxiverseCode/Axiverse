using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Simulation
{
    public class Simulator
    {
        public Scene Scene { get; set; }
        public List<Stage> Stages { get; } = new List<Stage>();

        public void Update(Clock clock)
        {
            foreach (var stage in Stages)
            {
                stage.Process(clock);
            }
        }
    }
}
