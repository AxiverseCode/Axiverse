using System;

namespace Axiverse.Mathematics.Numerics.Interpolation
{
    public static class Bezier
    {
        // https://github.com/HTD/FastBezier/blob/master/Program.cs
        // http://www.gamedev.net/topic/551455-length-of-a-generalized-quadratic-bezier-curve-in-3d/


        public static Vector3 InterpolateQuadratic(Vector3 A, Vector3 B, Vector3 C, float t)
        {
            return (1f - t) * (1f - t) * A + 2f * t * (1f - t) * B + t * t * C;
        }

        /// <summary>
        /// Gets the calculated length.
        /// </summary>
        /// <remarks>
        /// Integral calculation by Dave Eberly, slightly modified for the edge case with colinear control point.
        /// See: http://www.gamedev.net/topic/551455-length-of-a-generalized-quadratic-bezier-curve-in-3d/
        /// </remarks>
        public static float LengthQuadratic(Vector3 A, Vector3 B, Vector3 C)
        {
            if (A == C)
            {
                return (A == B) ? 0f : (A - B).Length();
            }
            if (B == A || B == C)
            {
                return (A - C).Length();
            }

            Vector3 A0 = B - A;
            Vector3 A1 = A - 2f * B + C;

            if (A1 != Vector3.Zero)
            {
                var c = 4f * A1.Dot(A1);
                var b = 8f * A0.Dot(A1);
                var a = 4f * A0.Dot(A0);
                var q = 4f * a * c - b * b;
                var twoCpB = 2f * c + b;
                var sumCBA = c + b + a;
                var l0 = (0.25f / c) * (twoCpB * Functions.Sqrt(sumCBA) - b * Functions.Sqrt(a));

                if (q == 0f)
                {
                    return l0;
                }

                var l1 = (q / (8f * Math.Pow(c, 1.5f)))
                    * (Math.Log(2f * Functions.Sqrt(c * sumCBA) + twoCpB) - Math.Log(2f * Functions.Sqrt(c * a) + b));
                return l0 + (float)l1;
            }
            else
            {
                return 2f * A0.Length();
            }
        }
    }
}
