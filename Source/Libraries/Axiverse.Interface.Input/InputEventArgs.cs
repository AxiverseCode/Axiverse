using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class InputEventArgs
    {
        public List<AxisTransition> Axes { get; set; }
        public List<ButtonTransition> Buttons { get; set; }
        public List<KeyboardTransition> Keys { get; set; }
    }

    public class AxisTransition
    {
        public float Value { get; set; }
        public float Change { get; set; }
    }

    public class ButtonTransition
    {
        public bool Pressed { get; set; }
    }

    public class KeyboardTransition
    {

    }

    public class MouseTransition
    {

        public Vector2 Position { get; private set; }
    }
}
