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
        /// Gets or sets the position of the component.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the component.
        /// </summary>
        public Vector3 Velocity { get; set; }

        public override Component Clone()
        {
            return new SpatialComponent
            {
                Position = Position,
            };
        }
    }
}
