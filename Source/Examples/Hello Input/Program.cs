using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RawDevice = SharpDX.RawInput.Device;

namespace Hello_Input
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawInput = RawDevice.GetDevices();

            var directInput = new DirectInput();

            var devices = directInput.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AllDevices);
            foreach (var device in devices)
            {
                Console.WriteLine(device);
            }

            var joysticks = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices);
            var jinstances = joysticks.Select(j => new Joystick(directInput, j.InstanceGuid)).ToArray();

            Console.ReadKey();
        }
    }
}
