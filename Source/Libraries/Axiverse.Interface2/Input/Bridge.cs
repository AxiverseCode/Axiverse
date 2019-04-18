using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Input
{
    public class Bridge
    {
        private readonly List<KeyboardEventArgs> keyboardEvents = new List<KeyboardEventArgs>();
        private readonly List<MouseEventArgs> mouseEvents = new List<MouseEventArgs>();
        private readonly List<JoypadEventArgs> joypadEventArgs = new List<JoypadEventArgs>();

        public List<IInputSource> Sources { get; } = new List<IInputSource>();

        public DirectInputSource DirectInput { get; set; }

        public Bridge()
        {
            
        }

        /// <summary>
        /// Refreshes the bridge to detect for device changes.
        /// </summary>
        public void Refresh()
        {

        }

        public void Update()
        {
            foreach (var source in Sources)
            {
                source.Update();
            }
        }

        // The bridge delivers keyboard, mouse, and joypad events. The mapper changes these to axis and button events.
        // Keyboard and mouse should be put through the standard. This is only a secondary mapper. The keyboard and mouse are handled by the UI first.
    }
}
