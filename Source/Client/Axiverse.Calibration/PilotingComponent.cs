using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    /// <summary>
    /// Component which handles translating standard piloting vectors (pitch, roll, yaw, thrust) into thrust vectors.
    /// </summary>
    public class PilotingComponent : Component
    {
        public float DeltaPitch { get; set; }

        public float DeltaRoll { get; set; }

        public float DeltaYaw { get; set; }

        public Vector3 MaximumRotation { get; set; }

        public Vector3 TargetThrust { get; set; }
        // deltas

        // targets

        public Vector3 Thrust { get; set; }

        public Vector3 MaximumThrust { get; set; }
    }
}
