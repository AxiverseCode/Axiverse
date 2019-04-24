using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Axiverse.Mathematics.Fixed.FixedPoint;

namespace Axiverse.Mathematics.Fixed
{
    /// <summary>
    /// Internal testing version of 64 bit number using the same logic as the others.
    /// </summary>
    public struct UInt64_
    {
        internal uint A;
        internal uint B;

        public UInt64_(ulong value)
        {
            A = (uint)(value >> 32);
            B = (uint)value;
        }

        public override string ToString()
        {
            return "UInt64 :" + (ulong)this;
        }

        public static implicit operator UInt64_(ulong value)
        {
            return new UInt64_(value);
        }

        public static explicit operator ulong (UInt64_ value)
        {
            return (ulong)value.A << 32 | value.B;
        }

        public static void OnesComplement(ref UInt64_ value, out UInt64_ result)
        {
            result.A = ~value.A;
            result.B = ~value.B;
        }

        public static void TwosComplement(ref UInt64_ value, out UInt64_ result)
        {
            OnesComplement(ref value, out result);
            var one = (UInt64_)1;
            Add(ref result, ref one, out result);
        }

        public static void Add(ref UInt64_ left, ref UInt64_ right, out UInt64_ result)
        {
            uint carry = 0;
            result.B = AddWithCarry(left.B, right.B, ref carry);
            result.A = AddWithCarry(left.A, right.A, ref carry);

            // Throw an overflow error if checked.
            carry += uint.MaxValue;
        }

        public static void Subtract(ref UInt64_ left, ref UInt64_ right, out UInt64_ result)
        {
            TwosComplement(ref right, out result);
            Add(ref left, ref result, out result);
        }

        public static void Multiply(ref UInt64_ left, uint right, out UInt64_ result)
        {
            uint carry = 0;
            result.B = MultiplyWithCarry(left.B, right, ref carry);
            result.A = MultiplyWithCarry(left.A, right, ref carry);

            // Throw an overflow error if checked.
            carry += uint.MaxValue;
        }

        public static void Divide(ref UInt64_ left, uint right, out UInt64_ result)
        {
            uint remainder = 0;
            result.A = DivideWithRemainder(left.A, right, ref remainder);
            result.B = DivideWithRemainder(left.B, right, ref remainder);
        }

        public static void Modulo(ref UInt64_ left, ref uint right, out uint result)
        {
            uint remainder = 0;
            UInt64_ division = default;
            division.A = DivideWithRemainder(left.A, right, ref remainder);
            division.B = DivideWithRemainder(left.B, right, ref remainder);
            result = remainder;
        }
    }
}
