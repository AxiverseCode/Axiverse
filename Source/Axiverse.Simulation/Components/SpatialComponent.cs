using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    public class SpatialComponent : Component
    {
        private readonly Writer writer;
        public Vector3 Position => writer.Position;

        public SpatialComponent(Writer writer)
        {
            this.writer = writer;
        }

        public class Writer
        {
            public SpatialComponent Reader { get; }

            public Writer()
            {
                Reader = new SpatialComponent(this);
            }

            public Vector3 Position;
        }
    }

}
