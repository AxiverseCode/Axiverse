using Axiverse.Simulation;
using Axiverse.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration.Simulation
{
    public class UniverseEntity : Entity
    {
        public CollectionComponent<AgentEntity> Agents { get; }

        public UniverseEntity()
        {
            Components.Add(Agents = new CollectionComponent<AgentEntity>());
        }

        public void Process()
        {

        }

        public void Interpolate()
        {

        }

        public void Step()
        {

        }
    }
}
