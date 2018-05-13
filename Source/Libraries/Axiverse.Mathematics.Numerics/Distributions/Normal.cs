using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Numerics.Distributions
{
    /// <summary>
    /// Represents a normal distribution
    /// </summary>
    public class Normal
    {


        /// <summary>
        /// Calculates the cumulative density at the given point.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public double CumulativeDensity(double d)
        {
            const double A1 = 0.31938153;
            const double A2 = -0.356563782;
            const double A3 = 1.781477937;
            const double A4 = -1.821255978;
            const double A5 = 1.330274429;
            const double RSQRT2PI = 0.39894228040143267793994605993438;

            double
            K = 1.0 / (1.0 + 0.2316419 * Math.Abs(d));

            double
            cnd = RSQRT2PI * Math.Exp(-0.5 * d * d) *
                  (K * (A1 + K * (A2 + K * (A3 + K * (A4 + K * A5)))));

            if (d > 0)
                cnd = 1.0 - cnd;

            return cnd;
        }
    }
}
