using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class Signal
    {
        public Source Source { get; set; }

        public JoystickOffset Offset { get; set; }

        public int Value { get; set; }
    }
}
