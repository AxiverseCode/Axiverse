using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public interface IProcessor
    {
        ProcessorStage Stage { get; }

        Type[] ComponentTypes { get; }

        bool ContainsKey(Guid identifier);

        bool Matches(Entity entity);

        void Add(Entity entity);

        void Remove(Entity entity);

        void Process(SimulationContext context);
    }
}
