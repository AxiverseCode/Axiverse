using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    public class DirectInputSource : IInputSource
    {
        public DirectInput DirectInput { get; set; }

        public List<Joystick> Joysticks { get; } = new List<Joystick>();

        public DirectInputSource()
        {
            DirectInput = new DirectInput();
            var instances = DirectInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices);

            foreach (var instance in instances)
            {
                var joystick = new Joystick(DirectInput, instance.InstanceGuid);
                joystick.Properties.AxisMode = DeviceAxisMode.Relative;
                joystick.Properties.BufferSize = 128;
                Joysticks.Add(joystick);
            }
        }

        public void Acquire()
        {
            foreach (var joystick in Joysticks)
            {
                joystick.Acquire();
            }
        }

        public void Release()
        {
            foreach (var joystick in Joysticks)
            {
                joystick.Unacquire();
            }
        }

        public void Update()
        {
            foreach (var joystick in Joysticks)
            {
                try
                {
                    joystick.Poll();
                    var data = joystick.GetBufferedData();

                    foreach (var update in data)
                    {

                    }
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
