using System;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Description for the <see cref="Presenter"/>.
    /// </summary>
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
