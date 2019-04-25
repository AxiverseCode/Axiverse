using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Drawing
{
    public partial struct Color
    {
        private float r;
        public float R
        {
            get => !float.IsNaN(r) ? r : Template.Color.r;
            set
            {
                if (Template != null)
                {
                    throw new InvalidOperationException();
                }
                r = value;
            }
        }

        private float g;
        public float G
        {
            get => !float.IsNaN(g) ? g : Template.Color.g;
            set
            {
                if (Template != null)
                {
                    throw new InvalidOperationException();
                }
                g = value;
            }
        }

        private float b;
        public float B
        {
            get => !float.IsNaN(b) ? b : Template.Color.b;
            set
            {
                if (Template != null)
                {
                    throw new InvalidOperationException();
                }
                b = value;
            }
        }

        private float a;
        public float A
        {
            get => !float.IsNaN(a) ? a : Template.Color.a;
            set
            {
                if (Template != null)
                {
                    throw new InvalidOperationException();
                }
                a = value;
            }
        }

        public bool IsAuthoritative => Template == null || Template.IsStatic;

        public KnownColor Template { get; }

        public Color(KnownColor template)
        {
            Template = template;
            if (template.IsStatic)
            {
                r = template.Color.r;
                g = template.Color.g;
                b = template.Color.b;
                a = template.Color.a;
            }
            else
            {
                r = g = b = a = float.NaN;
            }
        }

        public Color(byte r, byte g, byte b, byte a = 0xff)
            : this(ToFloat(r), ToFloat(g), ToFloat(b), ToFloat(a))
        {

        }

        public Color(float gray, float a = 1f) : this(gray, gray, gray)
        {

        }

        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.Template = null;
        }

        public override bool Equals(object obj)
        {
            return obj is Color color &&
                (color.Template == Template ||
                (color.r == r && color.g == g && color.b == b && color.a == a));
        }

        public uint ToBgra()
        {
            // Argb in little endian
            uint value = ToByte(b);
            value |= (uint)ToByte(g) << 8;
            value |= (uint)ToByte(r) << 16;
            value |= (uint)ToByte(a) << 24;
            return value;
        }
        public uint ToArgb()
        {
            uint value = ToByte(a);
            value |= (uint)ToByte(r) << 8;
            value |= (uint)ToByte(g) << 16;
            value |= (uint)ToByte(b) << 24;
            return value;
        }

        public static Color FromRgb(int rgb)
        {
            byte r = (byte)(rgb >> 16);
            byte g = (byte)(rgb >> 8);
            byte b = (byte)(rgb >> 0);
            return new Color(r, g, b);
        }

        public static Color FromRgba(uint rgba)
        {
            byte r = (byte)(rgba >> 24);
            byte g = (byte)(rgba >> 16);
            byte b = (byte)(rgba >> 8);
            byte a = (byte)(rgba >> 0);
            return new Color(r, g, b, a);
        }

        public static Color FromBgra(uint bgra)
        {
            byte b = (byte)(bgra >> 24);
            byte g = (byte)(bgra >> 16);
            byte r = (byte)(bgra >> 8);
            byte a = (byte)(bgra >> 0);
            return new Color(r, g, b, a);
        }

        public static explicit operator Rgba32(Color color)
        {
            return new Rgba32(ToByte(color.r), ToByte(color.g), ToByte(color.b), ToByte(color.a));
        }

        public static implicit operator Color(Rgba32 value)
        {
            return new Color(value.R, value.G, value.B, value.A);
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        private static float ToFloat(byte value)
        {
            return value / 255f;
        }

        private static byte ToByte(float component)
        {
            var value = (int)(component * 255f);
            return ToByte(value);
        }

        public static byte ToByte(int value)
        {
            return (byte)(value < 0 ? 0 : value > 255 ? 255 : value);
        }

        public static Color OfConstant(string name, uint value)
        {
            var known = KnownColor.OfConstant(name, FromBgra(value));
            return new Color(known);
        }

        public static Color OfName(string name, int value)
        {
            var known = KnownColor.OfName(name, FromRgb(value));
            return new Color(known);
        }



        public Color ToAuthoritative()
        {
            if (IsAuthoritative)
            {
                return this;
            }
            return new Color(R, G, B, A);
        }

        public override int GetHashCode()
        {
            if (!IsAuthoritative)
            {
                return Template.GetHashCode();
            }

            var hashCode = -490236692;
            hashCode = hashCode * -1521134295 + r.GetHashCode();
            hashCode = hashCode * -1521134295 + g.GetHashCode();
            hashCode = hashCode * -1521134295 + b.GetHashCode();
            hashCode = hashCode * -1521134295 + a.GetHashCode();
            return hashCode;
        }
    }
}
