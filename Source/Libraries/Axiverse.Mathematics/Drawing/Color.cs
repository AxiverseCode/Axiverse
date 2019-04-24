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

        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.Template = null;
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

        private static Color NameColor(string name, uint value)
        {
            var known = KnownColor.CreateStatic(name, FromBgra(value));
            return new Color(known);
        }
    }
}
