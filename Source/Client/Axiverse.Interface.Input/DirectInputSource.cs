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

        public Guid Identifier { get; set; }
        public Joystick Joystick { get; set; }

        public DirectInputSource(DirectInput directInput, Guid identifier)
        {
            DirectInput = directInput;
            Identifier = identifier;
            Joystick = new Joystick(directInput, identifier);

            // Set BufferSize in order to use buffered data.
            Joystick.Properties.BufferSize = 128;

            var objects = Joystick.GetObjects();
            //objects[0].Aspect;
            //Joystick.Capabilities.

            // Acquire the joystick
            Joystick.Acquire();
        }
        
        public override void Poll()
        {
            Joystick.Poll();
            var data = Joystick.GetBufferedData();

            foreach (var datum in data)
            {
                
            }

            throw new NotImplementedException();
        }
    }
}
