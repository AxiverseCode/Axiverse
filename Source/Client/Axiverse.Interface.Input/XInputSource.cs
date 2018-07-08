using SharpDX.DirectInput;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    public class XInputSource : Source
    {
        public Controller Controller { get; set; }

        public int Index { get; set; }

        public State State { get; set; }

        public XInputSource(UserIndex userIndex)
        {
            Index = (int)userIndex;
            Controller = new Controller(userIndex);
        }

        public override Signal[] Poll()
        {
            List<Signal> signals = new List<Signal>();

            if (Controller.IsConnected)
            {
                var state = Controller.GetState();
                if (state.PacketNumber != State.PacketNumber)
                {
                    AxisSignal(XInputOffsets.LeftX, State.Gamepad.LeftThumbX, state.Gamepad.LeftThumbX, signals);
                    AxisSignal(XInputOffsets.LeftY, State.Gamepad.LeftThumbY, state.Gamepad.LeftThumbY, signals);
                    AxisSignal(XInputOffsets.LeftTrigger, State.Gamepad.LeftTrigger, state.Gamepad.LeftTrigger, signals);
                    AxisSignal(XInputOffsets.RightX, State.Gamepad.RightThumbX, state.Gamepad.RightThumbX, signals);
                    AxisSignal(XInputOffsets.RightY, State.Gamepad.RightThumbY, state.Gamepad.RightThumbY, signals);
                    AxisSignal(XInputOffsets.RightTrigger, State.Gamepad.RightTrigger, state.Gamepad.RightTrigger, signals);
                    //Console.WriteLine(state.Gamepad.LeftThumbX);

                }
                State = state;
            }

            return signals.ToArray();
        }

        public void AxisSignal(XInputOffsets offset, short oldValue, short newValue, List<Signal> target)
        {
            if (oldValue != newValue)
            {
                target.Add(new Signal()
                {
                    Source = this,
                    Offset = (JoystickOffset)offset,
                    Value = newValue
                });
            }
        }

        public enum XInputOffsets
        {
            LeftX,
            LeftY,
            LeftTrigger,
            RightX,
            RightY,
            RightTrigger,
        }
    }
}
