using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class Model
    {
        public Entity Entity { get; set; }

        [Component]
        public SpatialComponent Spatial => spatial.Reader;

        public Model()
        {
            spatial.Position = new Vector3();
        }

        [Processor]
        public void OnCollectGraphics(Message message)
        {

        }

        public void ProcessPoo([Writer]SpatialComponent spatial, [Reader]int reddit)
        {

        }

        private SpatialComponent.Writer spatial = new SpatialComponent.Writer();
    }
}
