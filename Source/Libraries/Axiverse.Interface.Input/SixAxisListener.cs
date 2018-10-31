using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class SixAxisListener : Listener
    {
        private Vector3 translation;
        public Vector3 Translation => translation;

        private Vector3 rotation;
        public Vector3 Rotation => rotation;

        public override void Process(Signal signal)
        {
            if (!(signal.Source is DirectInputSource))
                return;

            switch(signal.Offset)
            {
                case JoystickOffset.X: translation.X += signal.Value / 1000f; break;
                case JoystickOffset.Y: translation.Y += signal.Value / 1000f; break;
                case JoystickOffset.Z: translation.Z += signal.Value / 1000f; break;
                case JoystickOffset.RotationX: rotation.X += signal.Value / 1000f; break;
                case JoystickOffset.RotationY: rotation.Y += signal.Value / 1000f; break;
                case JoystickOffset.RotationZ: rotation.Z += signal.Value / 1000f; break;
            }
        }

        public void Acknowledge()
        {
            translation = Vector3.Zero;
            rotation = Vector3.Zero;
        }
    }
}
