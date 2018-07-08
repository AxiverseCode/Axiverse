using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Axiverse.Interface.Input.XInputSource;

namespace Axiverse.Interface.Input
{
    public class TwoAxisListener : Listener
    {
        private Vector3 position;
        public Vector3 Position => position;

        private Vector3 position2;
        public Vector3 Position2 => position2;

        public override void Process(Signal signal)
        {
            if (!(signal.Source is XInputSource))
                return;

            switch (signal.Offset)
            {
                case (JoystickOffset)XInputOffsets.LeftX: position.X = signal.Value / (float)short.MaxValue; break;
                case (JoystickOffset)XInputOffsets.LeftY: position.Y = signal.Value / (float)short.MaxValue; break;
                case (JoystickOffset)XInputOffsets.LeftTrigger: position.Z = signal.Value / (float)short.MaxValue; break;
                case (JoystickOffset)XInputOffsets.RightX: position2.X = signal.Value / (float)short.MaxValue; break;
                case (JoystickOffset)XInputOffsets.RightY: position2.Y = signal.Value / (float)short.MaxValue; break;
                case (JoystickOffset)XInputOffsets.RightTrigger: position2.Z = signal.Value / (float)short.MaxValue; break;
            }
        }

    }
}
