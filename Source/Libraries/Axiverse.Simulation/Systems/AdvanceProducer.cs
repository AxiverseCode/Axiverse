using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Producers;
using Axiverse.Simulation.Components;

namespace Axiverse.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvanceProducer
    {
        [Produces]
        SpatialComponent ProduceSpatialComponent(
            // player inputs as well
            [StepTime]float delta,
            [Current]SpatialComponent spatial,
            [Current]PhysicsComponent physics)
        {
            physics.Body.Integrate(delta);

            var next = new SpatialComponent();
            next.Position = physics.Body.LinearPosition;
            return next;
        }
    }
}
