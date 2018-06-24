using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class PresenterDescription
    {
        /// <summary>
        /// Gets the window handle.
        /// </summary>
        public IntPtr WindowHandle { get; set; }

        /// <summary>
        /// Gets the width of the back buffer.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets the height of the back buffer.
        /// </summary>
        public int Height { get; set; }
    }
}
