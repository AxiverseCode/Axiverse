using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Serialization
{
    /// <summary>
    /// Quantized Quaternion within the unit range [-1, 1] in 64 bits.
    /// </summary>
    public class UnitQuaternion64
    {
        short X;
        short Y;
        short Z;
        short W;

        public UnitQuaternion64(short x, short y, short z, short w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static implicit operator Quaternion(UnitQuaternion64 value)
        {
            return new Quaternion(
                (float)value.X / short.MaxValue,
                (float)value.Y / short.MaxValue,
                (float)value.Z / short.MaxValue,
                (float)value.W / short.MaxValue);
        }

        public static readonly UnitQuaternion64 Identity = new UnitQuaternion64(0, 0, 0, short.MaxValue);
        public static readonly UnitQuaternion64 MaxValue = new UnitQuaternion64(short.MaxValue, short.MaxValue, short.MaxValue, short.MaxValue);
        public static readonly UnitQuaternion64 MinValue = new UnitQuaternion64(short.MinValue, short.MinValue, short.MinValue, short.MinValue);
    }
}
