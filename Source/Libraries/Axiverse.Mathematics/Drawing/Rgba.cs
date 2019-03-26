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
    public struct Rgba
    {
        /// <summary>Gets or sets the R component of the color.</summary>
        public float R;

        /// <summary>Gets or sets the G component of the color.</summary>
        public float G;

        /// <summary>Gets or sets the B component of the color.</summary>
        public float B;

        /// <summary>Gets or sets the A component of the color.</summary>
        public float A;

        /// <summary>
        /// Constructs a color.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Rgba(float r = 0, float g = 0, float b = 0, float a = 0)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
