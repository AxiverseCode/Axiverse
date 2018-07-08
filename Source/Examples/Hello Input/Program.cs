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
        static readonly Guid xboxControllerProductGuid = new Guid("{02e3045e-0000-0000-0000-504944564944}");

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

            var spaceNavigator = joysticks.First(j => j.ProductGuid == xboxControllerProductGuid);
            var sn = new Joystick(directInput, spaceNavigator.InstanceGuid);
            sn.Properties.AxisMode = DeviceAxisMode.Relative;
            sn.Properties.BufferSize = 256;
            sn.Properties.Range = new InputRange(-1000, +1000);
            sn.Acquire();
            var dict = new Dictionary<JoystickOffset, int>();
            var deltas = new Dictionary<JoystickOffset, int>();

            int x, y, z;
            int rx, ry, rz;
            x = y = z = rx = ry = rz = 0;

            while (true)
            {
                var data = sn.GetBufferedData();

                foreach (var datum in data)
                {
                    switch (datum.Offset)
                    {
                        
                        case JoystickOffset.X: x = datum.Value; break;
                        case JoystickOffset.Y: y = datum.Value; break;
                        case JoystickOffset.Z: z = datum.Value; break;
                        case JoystickOffset.RotationX: rx = datum.Value; break;
                        case JoystickOffset.RotationY: ry = datum.Value; break;
                        case JoystickOffset.RotationZ: rz = datum.Value; break;
                    }
                    //Console.WriteLine($"{x} {y} {z} {rx} {ry} {rz}");


                    if (dict.ContainsKey(datum.Offset))
                    {
                        deltas[datum.Offset] = datum.Value - dict[datum.Offset];
                    }
                    else
                    {
                        deltas[datum.Offset] = 0;
                    }
                    dict.Clear();
                    
                    dict[datum.Offset] = datum.Value;
                    //Console.WriteLine("===");
                    //Console.WriteLine(datum.Offset);
                    //Console.WriteLine(datum.RawOffset);
                    //Console.WriteLine(datum.Value);
                    
                }

                foreach (var item in dict)
                {
                    Console.Write($"{item.Key} : {item.Value}\t");
                }
                //Console.WriteLine();

                Console.Clear();
                Console.WriteLine(sn.GetCurrentState());

                Thread.Sleep(10);
            }
            

            Console.ReadKey();
        }
    }
}
