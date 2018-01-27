using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Serialization
{
    /// <summary>
    /// Quantized quaternion within the unit range [-1, 1] in 64 bits.
    /// </summary>
    public class UnitQuaternion64
    {
        short X;
        short Y;
        short Z;
        short W;

        /// <summary>
        /// Constructs a unit quaternion.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public UnitQuaternion64(short x, short y, short z, short w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Coverts a quantized quaternion into a floating point quaternion.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Quaternion(UnitQuaternion64 value)
        {
            return new Quaternion(
                (float)value.X / short.MaxValue,
                (float)value.Y / short.MaxValue,
                (float)value.Z / short.MaxValue,
                (float)value.W / short.MaxValue);
        }

        /// <summary>
        /// Converts a floating point quaternion into a quantized quaternion.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator UnitQuaternion64(Quaternion value)
        {
            return new UnitQuaternion64(
                (short)(value.X * short.MaxValue),
                (short)(value.Y * short.MaxValue),
                (short)(value.Z * short.MaxValue),
                (short)(value.W * short.MaxValue));
        }

        /// <summary>The identity quaternion.</summary>
        public static readonly UnitQuaternion64 Identity = new UnitQuaternion64(0, 0, 0, short.MaxValue);
        /// <summary>The maximum value quaternion.</summary>
        public static readonly UnitQuaternion64 MaxValue = new UnitQuaternion64(short.MaxValue, short.MaxValue, short.MaxValue, short.MaxValue);
        /// <summary>The minimum value quaternion.</summary>
        public static readonly UnitQuaternion64 MinValue = new UnitQuaternion64(short.MinValue, short.MinValue, short.MinValue, short.MinValue);
    }
}
