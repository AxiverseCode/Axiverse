using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Distributions
{
    public interface IContinuousDistribution
    {
        double Maximum { get; }
        double Minimum { get; }

        double Density(double x);
        double CumulativeDensity(double x);
    }
}
