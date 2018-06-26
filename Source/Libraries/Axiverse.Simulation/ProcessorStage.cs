using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    // Stages
    // 0    - Critical First
    // 100  - Preprocessing
    // 200  - 

    public enum ProcessorStage
    {
        None = 0,
        Critical = 1,

        Preprocessing = 1000,

        Components = 2000,

        Physics = 3000,

        Reconciliation = 4000,

        Propagation = 5000,

        Final = 9000,
    }
}
