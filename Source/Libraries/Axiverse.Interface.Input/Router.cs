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
        public event EventHandler DeviceAdded;
        public event EventHandler DeviceRemoved;

        static readonly Guid spaceNavigatorProductGuid = new Guid("{c626046d-0000-0000-0000-504944564944}");
        static readonly Guid _3dConnexionKmjEmultorProductGuid = new Guid("{046dbeef-0000-0000-0000-504944564944}");
        static readonly Guid xboxControllerProductGuid = new Guid("{02e3045e-0000-0000-0000-504944564944}");
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

        public List<Source> Sources { get; set; } = new List<Source>();
        public List<Listener> Listeners { get; set; } = new List<Listener>();

        public Router()
        {
            directInput = new DirectInput();
            var joysticks = directInput
                .GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AllDevices)
                .Where(j => j.ProductGuid != xboxControllerProductGuid && j.ProductGuid != _3dConnexionKmjEmultorProductGuid);
            foreach (var joystick in joysticks)
            {
                Sources.Add(new DirectInputSource(directInput, joystick.InstanceGuid));
            }

            Sources.Add(new XInputSource(SharpDX.XInput.UserIndex.One));
        }

        public void Poll()
        {
            foreach (var source in Sources)
            {
                foreach (var signal in source.Poll())
                {
                    foreach (var listener in Listeners)
                    {
                        listener.Process(signal);
                    }
                }
            }
        }

        /// <summary>
        /// Queries all devices and updates mappings as necessary.
        /// </summary>
        public void Refresh()
        {

        }

        private DirectInput directInput;
    }
}
