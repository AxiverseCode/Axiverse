using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Drawing
{
    /// <summary>
    /// Represents a color which is defined by its R, G, B, and A components.
    /// </summary>
    public struct Rgba32
    {
        /// <summary>Gets or sets the R component of the color.</summary>
        public byte R;

        /// <summary>Gets or sets the G component of the color.</summary>
        public byte G;

        /// <summary>Gets or sets the B component of the color.</summary>
        public byte B;

        /// <summary>Gets or sets the A component of the color.</summary>
        public byte A;

        /// <summary>
        /// Constructs a color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Rgba32(byte r = 0, byte g = 0, byte b = 0, byte a = 0)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
