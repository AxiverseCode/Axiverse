using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    internal class DirectInputSource : Source
    {
        public DirectInput DirectInput { get; set; }

        public Guid InstanceIdentifier { get; set; }
        public Guid DeviceIdentifier { get; set; }


        public Guid Identifier { get; set; }
        public Joystick Joystick { get; set; }

        public DirectInputSource(DirectInput directInput, Guid identifier)
        {
            DirectInput = directInput;
            Identifier = identifier;
            Joystick = new Joystick(directInput, identifier);

            // Set BufferSize in order to use buffered data.
            Joystick.Properties.AxisMode = DeviceAxisMode.Relative;
            Joystick.Properties.BufferSize = 128;

            var objects = Joystick.GetObjects();
            //objects[0].Aspect;
            //Joystick.Capabilities.

            // Acquire the joystick
            Joystick.Acquire();
        }

        public override Signal[] Poll()
        {
            try
            {
                Joystick.Poll();
                var data = Joystick.GetBufferedData();
                return data.Select(u =>
                {
                    return new Signal()
                    {
                        Source = this,
                        Offset = u.Offset,
                        Value = u.Value
                    };
                }).ToArray();

            }
            catch (Exception)
            {
                return Array.Empty<Signal>();
            }
        }
    }
}