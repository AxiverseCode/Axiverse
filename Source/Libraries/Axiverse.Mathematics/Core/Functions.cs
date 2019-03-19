using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
namespace Axiverse
{
    /// <summary>
    /// Mathematics functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Calculates the absolute value of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 Abs(Vector3 value)
        {
            return new Vector3(
                Math.Abs(value.X),
                Math.Abs(value.Y),
                Math.Abs(value.Z));
        }

        /// <summary>
        /// Calculates the sine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Sin(float value)
        {
            return (float)Math.Sin(value);
        }

        /// <summary>
        /// Calculates the sine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Sin(double value)
        {
            return Math.Sin(value);
        }

        /// <summary>
        /// Calculates the cosine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Cos(float value)
        {
            return (float)Math.Cos(value);
        }

        /// <summary>
        /// Calculates the cosine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Cos(double value)
        {
            return Math.Cos(value);
        }

        /// <summary>
        /// Calculates the arccosine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Acos(float value)
        {
            return (float)Math.Acos(value);
        }

        /// <summary>
        /// Calculates the arccosine function of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Acos(double value)
        {
            return Math.Acos(value);
        }

        /// <summary>
        /// Calculates the square root of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

   
        /// <summary>
        /// Calculates the square root of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 Sqrt(Vector3 value)
        {
            return new Vector3(
                Sqrt(value.X),
                Sqrt(value.Y),
                Sqrt(value.Z));
        }

        /// <summary>
        /// Calculates the square root of the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Sqrt(double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Clamps the value between the maximum and minimum.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static float Clamp(float value, float minimum, float maximum)
        {
            return Math.Max(minimum, Math.Min(value, maximum));
        }

        /// <summary>
        /// Clamps each component of the value between the corresponding component of the minimum
        /// and maximum.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 value, float minimum, float maximum)
        {
            var x = Clamp(value.X, minimum, maximum);
            var y = Clamp(value.Y, minimum, maximum);
            var z = Clamp(value.Z, minimum, maximum);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Clamps each component of the value between the corresponding component of the minimum
        /// and maximum.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 value, Vector3 minimum, Vector3 maximum)
        {
            var x = Clamp(value.X, minimum.X, maximum.X);
            var y = Clamp(value.Y, minimum.Y, maximum.Y);
            var z = Clamp(value.Z, minimum.Z, maximum.Z);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Clamps each component of the value between the corresponding component of the minimum
        /// and maximum.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static void Clamp(ref Vector3 value, ref Vector3 minimum, ref Vector3 maximum, out Vector3 result)
        {
            result.X = Clamp(value.X, minimum.X, maximum.X);
            result.Y = Clamp(value.Y, minimum.Y, maximum.Y);
            result.Z = Clamp(value.Z, minimum.Z, maximum.Z);
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static float ToDegrees(float radians)
        {
            return radians * RadiansToDegreesRatio;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static float ToRadians(float degrees)
        {
            return degrees * DegreesToRadiansRatio;
        }

        /// <summary>
        /// The default random number generator.
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        /// The degrees to radians conversion ratio.
        /// </summary>
        public static readonly float DegreesToRadiansRatio = 0.0174533f;

        /// <summary>
        /// The radians to degrees conversion ratio.
        /// </summary>
        public static readonly float RadiansToDegreesRatio = 57.2958f;

        /// <summary>
        /// The ratio of diameter to circumference of a circle.
        /// </summary>
        public static readonly float Pi = (float)Math.PI;

        /// <summary>
        /// The ratio or radius to circumference of a circle.
        /// </summary>
        public static readonly float Tau = (float)(2 * Math.PI);

        /// <summary>
        /// The epsilon value to determine equivalence.
        /// </summary>
        public static readonly float ZeroEpsilon = 1e-10f;

        [StructLayout(LayoutKind.Explicit)]
        private struct Cross32
        {
            [FieldOffset(0)]
            public uint UInt;

            [FieldOffset(0)]
            public float Float;
        }

        private static float InverseSqrt(float f)
        {
            // http://rrrola.wz.cz/inv_sqrt.html
            // https://news.ycombinator.com/item?id=17487475
            var c = new Cross32 { Float = f };
            c.UInt = 0x5F1FFFF9 - (c.UInt >> 1);
            return 0.703952253f * c.Float * (2.38924456f - f * c.Float * c.Float);
        }

        /// <summary>
        /// Returns a number with the ab
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static float CopySign(float value, float sign)
        {
            return sign < 0 && value > 0 ? -value : value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsillon"></param>
        /// <returns></returns>
        public static bool Equals(IEnumerable<float> a, IEnumerable<float> b, float epsillon)
        {
            return a.Zip(b, (i, j) => Math.Abs(j - i) <= epsillon).All(v => v);
        }
    }
}
