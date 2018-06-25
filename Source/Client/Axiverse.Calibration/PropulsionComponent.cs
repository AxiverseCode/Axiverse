using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    public class PropulsionComponent : Component
    {
        /// <summary>
        /// Gets a list of the <see cref="PropulsionEmitter"/>s attached to the entity.
        /// </summary>
        public List<PropulsionEmitter> Emitters { get; set; }

        /// <summary>
        /// Gets or sets the drag coefficient.
        /// </summary>
        public float DragCoefficient { get; set; }
    }
}
