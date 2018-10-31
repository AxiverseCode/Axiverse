using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    public class PropulsionEmitter
    {
        /// <summary>
        /// Gets or sets the offset
        /// </summary>
        public Vector3 Offset { get; set; }

        /// <summary>
        /// Gets or sets the current thrust vector of the emitter. Measured in Newtons
        /// </summary>
        public Vector3 ThrustVector { get; set; }

        /// <summary>
        /// Calculates the impulse delivered by this emitter over the given time span.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public Vector3 GetImpulse(float deltaTime)
        {
            return ThrustVector * deltaTime;
        }
    }
}
