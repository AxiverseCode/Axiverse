using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Producers;


namespace Axiverse.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvanceProducer
    {
        [Produces]
        SpatialComponent ProduceSpatialComponent([Current]SpatialComponent spatial)
        {
            var next = new SpatialComponent();
            next.Position = spatial.Position + new Vector3(0, 0, 1);
            return next;
        }

        [Produces]
        [Partial]
        List<Entity> ProduceNewEntites()
        {
            return null;
        }
    }
}
