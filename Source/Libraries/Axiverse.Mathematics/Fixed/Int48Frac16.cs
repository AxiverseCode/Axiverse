using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Fixed
{
    /// <summary>
    /// Fixed point number with a 48 bit integer and a 16 bit fraction.
    /// </summary>
    public struct Int48Frac16
    {
        // https://github.com/jpernst/FixedPointy

        internal const int FractionalBits = 16;
        internal const int IntegerBits = sizeof(long) * 8 - FractionalBits;

        internal const int FractionalMask = (int)(ulong.MaxValue >> IntegerBits);
        internal const int IntegerMask = (int)(-1 & ~FractionalMask);
        internal const int IntegerRange = FractionalMask + 1;
        internal const int MinInteger = int.MinValue >> FractionalBits;
        internal const int MaxInteger = int.MaxValue >> FractionalBits;




        #region Static 

        public static readonly Int48Frac16 Zero = new Int48Frac16(0);
        public static readonly Int48Frac16 One = new Int48Frac16(IntegerRange);
        public static readonly Int48Frac16 MinValue = new Int48Frac16(long.MinValue);
        public static readonly Int48Frac16 MaxValue = new Int48Frac16(long.MaxValue);
        public static readonly Int48Frac16 Epsilon = new Int48Frac16(1);

        #region Static - Operators

        public static Int48Frac16 operator %(Int48Frac16 left, Int48Frac16 right)
        {
            return new Int48Frac16(left.Data % right.Data);
        }

        public static Int48Frac16 operator <<(Int48Frac16 left, int shift)
        {
            return new Int48Frac16(left.Data << shift);
        }

        public static Int48Frac16 operator >>(Int48Frac16 left, int shift)
        {
            return new Int48Frac16(left.Data >> shift);
        }

        #endregion


        #endregion

        public readonly long Data;

        public Int48Frac16(long data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            return (obj is Int48Frac16 fix && fix.Data == Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

    }
}
