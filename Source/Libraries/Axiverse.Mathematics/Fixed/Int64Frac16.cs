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
    public struct Int64Frac16
    {
        // https://github.com/jpernst/FixedPointy
        // https://sestevenson.wordpress.com/fixed-point-division-of-two-q15-numbers/
        // https://github.com/ricksladkey/dirichlet-numerics/blob/master/Dirichlet.Numerics/UInt128.cs


        internal const int FractionalBits = 16;
        internal const int IntegerBits = sizeof(long) * 8 - FractionalBits;

        internal const int FractionalMask = (int)(ulong.MaxValue >> IntegerBits);
        internal const int IntegerMask = (int)(-1 & ~FractionalMask);
        internal const int FractionalRange = FractionalMask + 1;
        internal const int MinInteger = int.MinValue >> FractionalBits;
        internal const int MaxInteger = int.MaxValue >> FractionalBits;




        #region Static 

        public static readonly Int64Frac16 Zero = new Int64Frac16(0);
        public static readonly Int64Frac16 One = new Int64Frac16(FractionalRange);
        public static readonly Int64Frac16 MinValue = new Int64Frac16(long.MinValue);
        public static readonly Int64Frac16 MaxValue = new Int64Frac16(long.MaxValue);
        public static readonly Int64Frac16 Epsilon = new Int64Frac16(1);

        #region Static - Casting

        public static explicit operator double(Int64Frac16 value)
        {
            return (double)(value.Data >> FractionalBits) + (value.Data & FractionalMask) / (double)FractionalRange;
        }

        public static explicit operator float(Int64Frac16 value)
        {
            return (float)(double)value;
        }

        public static explicit operator long(Int64Frac16 value)
        {
            if (value.Data > 0)
                return value.Data >> FractionalBits;
            else
                return (value.Data + FractionalMask) >> FractionalBits;
        }

        public static implicit operator Int64Frac16(int value)
        {
            return new Int64Frac16(value << FractionalBits);
        }

        public static implicit operator Int64Frac16(long value)
        {
            return new Int64Frac16(value << FractionalBits);
        }

        #endregion

        #region Static - Operators

        
		public static bool operator == (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data == rhs.Data;
		}

		public static bool operator != (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data != rhs.Data;
		}

		public static bool operator > (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data > rhs.Data;
		}

		public static bool operator >= (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data >= rhs.Data;
		}

		public static bool operator < (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data < rhs.Data;
		}

		public static bool operator <= (Int64Frac16 lhs, Int64Frac16 rhs) {
			return lhs.Data <= rhs.Data;
		}

		public static Int64Frac16 operator + (Int64Frac16 value) {
			return value;
		}

		public static Int64Frac16 operator - (Int64Frac16 value) {
			return new Int64Frac16(-value.Data);
		}

		public static Int64Frac16 operator + (Int64Frac16 lhs, Int64Frac16 rhs) {
			return new Int64Frac16(lhs.Data + rhs.Data);
		}

		public static Int64Frac16 operator - (Int64Frac16 lhs, Int64Frac16 rhs) {
			return new Int64Frac16(lhs.Data - rhs.Data);
		}

		public static Int64Frac16 operator * (Int64Frac16 lhs, Int64Frac16 rhs) {
            // TODO: This can overflow.
			return new Int64Frac16((int)(((long)lhs.Data * (long)rhs.Data + (FractionalRange >> 1)) >> FractionalBits));
		}

		public static Int64Frac16 operator / (Int64Frac16 lhs, Int64Frac16 rhs) {
            // TODO: This can overflow.
			return new Int64Frac16((int)((((long)lhs.Data << (FractionalBits + 1)) / (long)rhs.Data + 1) >> 1));
		}

        public static Int64Frac16 operator %(Int64Frac16 left, Int64Frac16 right)
        {
            return new Int64Frac16(left.Data % right.Data);
        }

        public static Int64Frac16 operator <<(Int64Frac16 left, int shift)
        {
            return new Int64Frac16(left.Data << shift);
        }

        public static Int64Frac16 operator >>(Int64Frac16 left, int shift)
        {
            return new Int64Frac16(left.Data >> shift);
        }

        #endregion


        #endregion

        public readonly long Data;

        public Int64Frac16(long data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            return (obj is Int64Frac16 fix && fix.Data == Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

    }
}
