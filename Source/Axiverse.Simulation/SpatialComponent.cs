using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    public class SpatialComponent : Component
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Position { get; set; }

        public override Component Clone()
        {
            return new SpatialComponent
            {
                Position = Position,
            };
        }
    }
}
