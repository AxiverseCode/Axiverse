using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    public struct Index3
    {
        /// <summary>Gets or sets the A index.</summary>
        public int A;

        /// <summary>Gets or sets the B index.</summary>
        public int B;

        /// <summary>Gets or sets the C index.</summary>
        public int C;

        public Index3(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public override string ToString()
        {
            return $"{A}, {B}, {C}";
        }

        public static Index3 operator + (Index3 left, Index3 right)
        {
            return new Index3(left.A + right.A, left.B + right.B, left.C + right.C);
        }

        public static implicit operator Vector3 (Index3 value)
        {
            return new Vector3(value.A, value.B, value.C);
        }
    }
}
