using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RawDevice = SharpDX.RawInput.Device;

namespace Hello_Input
{
    class Program
    {
        static readonly Guid spaceNavigatorProductGuid = new Guid("{c626046d-0000-0000-0000-504944564944}");

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

            var spaceNavigator = joysticks.First(j => j.ProductGuid == spaceNavigatorProductGuid);
            var sn = new Joystick(directInput, spaceNavigator.InstanceGuid);
            sn.Properties.BufferSize = 256;
            sn.Acquire();
            while (true)
            {
                //var data = sn.GetBufferedData();

                //foreach (var datum in data)
                //{
                //    Console.WriteLine("===");
                //    Console.WriteLine(datum.Offset);
                //    Console.WriteLine(datum.RawOffset);
                //    Console.WriteLine(datum.Value);
                //}

                Console.WriteLine(sn.GetCurrentState());

                Thread.Sleep(10);
            }
            

            Console.ReadKey();
        }
    }
}
