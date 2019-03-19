using System;

using static System.Math;

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

        /// <summary>
        /// Calculates the bounding box of a quadratic bezier.
        /// </summary>
        /// <remarks>
        /// https://iquilezles.org/www/articles/bezierbbox/bezierbbox.htm
        /// </remarks>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Bounds3 BoundsQuadratic(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 min = Vector3.Minimum(p0, p2);
            Vector3 max = Vector3.Maximum(p0, p2);

            if (p1.X < min.X || p1.X > max.X ||
                p1.Y < min.Y || p1.Y > max.Y ||
                p1.Z < min.Z || p1.Z > max.Z)
            {
                Vector3 t = Functions.Clamp((p0 - p1) / (p0 - 2 * p1 + p2), 0, 1);
                Vector3 s = Vector3.One - t;
                Vector3 q = s * s * p0 + 2 * s * t * p1 + t * t * p2;

                min = Vector3.Minimum(min, q);
                max = Vector3.Maximum(max, q);
            }

            return new Bounds3(min, max);
        }

        /// <summary>
        /// Calculates the bounding box of a cubic bezier.
        /// </summary>
        /// <remarks>
        /// https://iquilezles.org/www/articles/bezierbbox/bezierbbox.htm
        /// </remarks>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Bounds3 BoundsCubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // extremes
            Vector3 min = Vector3.Minimum(p0, p3);
            Vector3 max = Vector3.Maximum(p0, p3);

            // note pascal triangle coefficnets
            Vector3 c = -1 * p0 + 1 * p1;
            Vector3 b = 1 * p0 - 2 * p1 + 1 * p2;
            Vector3 a = -1 * p0 + 3 * p1 - 3 * p2 + 1 * p3;

            Vector3 h = b * b - a * c;

            // real solutions
            if (h.X > 0 || h.Y > 0 || h.Z > 0)
            {
                Vector3 g = Functions.Sqrt(Functions.Abs(h));
                Vector3 t1 = Functions.Clamp((-b - g) / a, 0, 1);
                Vector3 s1 = Vector3.One - t1;
                Vector3 t2 = Functions.Clamp((-b + g) / a, 0, 1);
                Vector3 s2 = Vector3.One - t2;
                Vector3 q1 = s1 * s1 * s1 * p0 + 3 * s1 * s1 * t1 * p1 + 3 * s1 * t1 * t1 * p2 + t1 * t1 * t1 * p3;
                Vector3 q2 = s2 * s2 * s2 * p0 + 3 * s2 * s2 * t2 * p1 + 3 * s2 * t2 * t2 * p2 + t2 * t2 * t2 * p3;

                if (h.X > 0.0)
                {
                    min.X = Min(min.X, Min(q1.X, q2.X));
                    max.X = Max(max.X, Max(q1.X, q2.X));
                }
                if (h.Y > 0.0)
                {
                    min.Y = Min(min.Y, Min(q1.Y, q2.Y));
                    max.Y = Max(max.Y, Max(q1.Y, q2.Y));
                }
                if (h.Z > 0.0)
                {
                    min.Z = Min(min.Z, Min(q1.Z, q2.Z));
                    max.Z = Max(max.Z, Max(q1.Z, q2.Z));
                }
            }

            return new Bounds3(min, max);
        }

    }
}
