using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Numerics.Distributions
{
    /// <summary>
    /// Represents a continuous distribution
    /// </summary>
    public interface IContinuousDistribution
    {
        /// <summary>
        /// Gets the maximum value of the disttibution.
        /// </summary>
        double Maximum { get; }

        /// <summary>
        /// Gets the minimum valud of the distribution.
        /// </summary>
        double Minimum { get; }

        /// <summary>
        /// Calculates the density at the given point.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double Density(double x);

        /// <summary>
        /// Calculates the cumulative density at the given point.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double CumulativeDensity(double x);
    }
}
