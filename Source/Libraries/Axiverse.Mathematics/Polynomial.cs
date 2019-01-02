using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public static class Polynomial
    {
        // Return true if r1 and r2 are real
        public static bool QuadraticFormula(
            float a, float b, float c,
            out float r1,  // first
            out float r2  // and second roots
            )

        {
            var q = b * b - 4 * a * c;

            if (q >= 0)
            {
                var sq = Functions.Sqrt(q);
                var d = 1 / (2 * a);
                r1 = (-b + sq) * d;
                r2 = (-b - sq) * d;
                return true;  // real roots
            }
            
            r1 = r2 = float.NaN;
            return false;  // complex roots
        }

    }
}
