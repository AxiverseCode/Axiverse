using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public struct Color
    {
        /// <summary>
        /// Gets the red value.
        /// </summary>
        public float Red { get; }

        /// <summary>
        /// Gets the green value.
        /// </summary>
        public float Green { get; }

        /// <summary>
        /// Gets the blue value.
        /// </summary>
        public float Blue { get; }

        /// <summary>
        /// Gets the opacity.
        /// </summary>
        public float Opacity { get; }

        public Color(float gray) : this(gray, 1.0f)
        {

        }

        public Color(float gray, float opacity) : this(gray, gray, gray, opacity)
        {

        }

        public Color(float red, float green, float blue) : this(red, green, blue, 1.0f)
        {

        }

        public Color(float red, float green, float blue, float opacity)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Opacity = opacity;
        }

        public static Color FromHex(uint hex)
        {
            var r = (hex >> 16 & 0xff) / 255.0f;
            var g = (hex >> 8 & 0xff) / 255.0f;
            var b = (hex >> 0 & 0xff) / 255.0f;

            return new Color(r, g, b, 1);
        }

        public static bool operator ==(Color former, Color latter)
        {
            return former.Red == latter.Red
                && former.Green == latter.Green
                && former.Blue == latter.Blue
                && former.Opacity == latter.Opacity;
        }

        public static bool operator !=(Color former, Color latter)
        {
            return !(former == latter);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color color) {
                return color == this;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode() ^ Opacity.GetHashCode();
        }
    }

    public static class Colors
    {
        public static Color Red = Color.FromHex(0xeb213b);
        public static Color RR = Color.FromHex(0xee503b);
        public static Color Orange = Color.FromHex(0xf59331);
        public static Color Yellow = Color.FromHex(0xfdd331);

        public static Color Green = Color.FromHex(0x8dc549);
        public static Color Blue = Color.FromHex(0x1e5da8);

        public static Color Purple = Color.FromHex(0x5d338f);

    }
}
