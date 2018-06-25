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
        // Sources -> Devices which can generate input
        // Listeners -> Game representations of things that can receive input

        // Router provides mappings from different inputs from each source and maps them to the listeners
        // which are interested. This is persisted as devices are added or removed

        public List<Source> Sources { get; set; }
        public List<Listener> Listeners { get; set; }
        
        public Router()
        {

        }

        public void Poll()
        {
            Sources.ForEach(s => s.Poll());
        }

        /// <summary>
        /// Queries all devices and updates mappings as necessary.
        /// </summary>
        public void Refresh()
        {

        }
    }
}
