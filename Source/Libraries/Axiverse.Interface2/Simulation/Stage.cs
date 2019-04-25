using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Simulation
{
    public abstract class Stage
    {
        public Simulator Simulation { get; set; }
        public Scene Scene { get; set; }

        public abstract void Process(Clock clock);
    }
}
