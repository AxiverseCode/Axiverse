using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RawInputDevice = SharpDX.RawInput.Device;

namespace Axiverse.Interface.Input
{
    public class Router
    {
        // Sources provide input
        // Consolidate in the router
        // Handles routing to the controllers which are bound.


        // Axis input
        // Button input
        // Key input (Overcapture)
        // Midi Input

        // Sources -> Devices which can generate input
        // Listeners -> Game representations of things that can receive input

        // Router provides mappings from different inputs from each source and maps them to the listeners
        // which are interested. This is persisted as devices are added or removed

        public List<Source> Sources { get; set; }
        public List<Listener> Listeners { get; set; }

        static readonly Guid spaceNavigatorProductGuid = new Guid("{c626046d-0000-0000-0000-504944564944}");
        private Joystick joystick;
        public JoystickState PreviousState = new JoystickState();
        public RouterState State;

        public Router()
        {
            // SpaceMouse
            // Axis 0 - X
            // Axis 1 - Y
            // Axis 2 - Z (Up negative)
            // Axis 3 - Pitch
            // Axis 4 - Roll
            // Axis 5 - Yaw

            State = new RouterState();
            directInput = new DirectInput();
            var joysticks = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices);
            var spaceNavigator = joysticks.FirstOrDefault(j => j.ProductGuid == spaceNavigatorProductGuid);

            if (spaceNavigator != null)
            {
                joystick = new Joystick(directInput, spaceNavigator.InstanceGuid);
                joystick.Acquire();
            }
        }

        protected void Enumerate()
        {
            var devices = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
            var match = new Dictionary<Guid, DirectInputSource>(directInputSources);

            foreach (var device in devices)
            {

            }

        }


        public void Poll()
        {
            //Sources.ForEach(s => s.Poll());

            if (joystick == null)
                return;

            var state = joystick.GetCurrentState();

            State = new RouterState
            {
                X = (state.X - PreviousState.X) / (float)short.MaxValue,
                Y = (state.Y - PreviousState.Y) / (float)short.MaxValue,
                Z = (state.Z - PreviousState.Z) / (float)short.MaxValue,
                RotationX = (state.RotationX - PreviousState.RotationX) / (float)short.MaxValue,
                RotationY = (state.RotationY - PreviousState.RotationY) / (float)short.MaxValue,
                RotationZ = (state.RotationZ - PreviousState.RotationZ) / (float)short.MaxValue,
            };

            PreviousState = state;
            Console.WriteLine(State.X);
        }

        /// <summary>
        /// Queries all devices and updates mappings as necessary.
        /// </summary>
        public void Refresh()
        {

        }



        protected virtual void OnKeyboardInput()
        {

        }

        protected virtual void OnAxisInput()
        {

        }

        protected virtual void OnButtonInput()
        {

        }

        protected virtual void OnMidiEvent()
        {

        }

        private DirectInput directInput;
        private Dictionary<Guid, DirectInputSource> directInputSources = new Dictionary<Guid, DirectInputSource>();
    }

    public class RouterState
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }
}
