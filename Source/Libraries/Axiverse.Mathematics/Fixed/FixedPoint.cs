using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Fixed
{
    internal static class FixedPoint
    {
        public static uint AddWithCarry(uint left, uint right, ref uint carry)
        {
            ulong result = (ulong)carry + left + right;
            carry = (byte)(result >> 32);
            return (uint)result;
        }

        public static uint MultiplyWithCarry(uint left, uint right, ref uint carry)
        {
            ulong result = (ulong)left * right + carry;
            carry = (uint)(result >> 32);
            return (uint)result;
        }

        public static uint DivideWithRemainder(uint left, uint right, ref uint remainder)
        {
            // https://www.codeproject.com/Tips/785014/UInt-Division-Modulus
            ulong t = (ulong)remainder << 32 | left;
            remainder = (uint)(t % right);
            return (uint)(t / right);
        }
    }
}
