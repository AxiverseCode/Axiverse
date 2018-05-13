using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Serialization
{
    /// <summary>
    /// Functions for quantization of an angle from [-pi, pi)
    /// </summary>
    public static class QuantizedAngle
    {
        public static double Normalize(double theta)
        {
            return theta - TWO_PI * Math.Floor((theta + Math.PI) / TWO_PI);
        }

        public static sbyte QuantizeByte(double theta)
        {
            double mapped = Normalize(theta) / Math.PI * (-sbyte.MinValue);
            return (sbyte)Math.Round(mapped);
        }

        /// <summary>
        /// Quantizes an angle from [-pi, pi] into a signed byte with the specified density.
        /// 
        /// Density is the power to which the angle is raised before it's quantized. To see a graph
        /// of the distribution at different densities, plot y = π × 128^(-n) × x^n
        /// 
        /// A density of 1 represents linear quantization. Higher densities centralize more values
        /// closer to the origin, while lower densities centralize more values around the conjugate
        /// origin.
        /// 
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public static sbyte QuantizeSByte(double theta, double density)
        {
            // y = π ^ (-1/n) (128^n x)^(1/n)

            theta = Normalize(theta);
            double piScale = Math.Pow(Math.PI, -1 / density);
            double quantizeScale = Math.Pow(-sbyte.MinValue, density);

            double magnitude = piScale * Math.Pow(quantizeScale * Math.Abs(theta), 1 / density);
            return (sbyte)Math.Round(Math.Sign(theta) * magnitude);
        }

        /// <summary>
        /// 
        /// 
        /// theta = pi * 128 ^ (-d) * quantized ^ d
        /// </summary>
        /// <param name="qtheta"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public static double Dequantize(sbyte qtheta, double density)
        {
            // y = π × 128^(-n) × x^n
            double quantizeScale = Math.Pow(-sbyte.MinValue, -density);
            double magnitude = Math.PI * quantizeScale * Math.Pow(Math.Abs(qtheta), density);

            return Math.Sin(qtheta) * magnitude;
        }


        /// <summary>
        /// 2 pi.
        /// </summary>
        public const double TWO_PI = 6.283185307179586476925286766;
    }
}
