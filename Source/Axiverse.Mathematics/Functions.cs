using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    public class Functions
    {
        public static float Sin(float value)
        {
            return (float)Math.Sin(value);
        }

        public static float Cos(float value)
        {
            return (float)Math.Cos(value);
        }

        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        public static float Clamp(float value, float minimum, float maximum)
        {
            return Math.Max(minimum, Math.Min(value, maximum));
        }

        public static Vector3 Clamp(Vector3 value, Vector3 minimum, Vector3 maximum)
        {
            var x = Clamp(value.X, minimum.X, maximum.X);
            var y = Clamp(value.Y, minimum.Y, maximum.Y);
            var z = Clamp(value.Z, minimum.Z, maximum.Z);

            return new Vector3(x, y, z);
        }

        public static void Clamp(ref Vector3 value, ref Vector3 minimum, ref Vector3 maximum, out Vector3 result)
        {
            result.X = Clamp(value.X, minimum.X, maximum.X);
            result.Y = Clamp(value.Y, minimum.Y, maximum.Y);
            result.Z = Clamp(value.Z, minimum.Z, maximum.Z);
        }

        public static readonly float DegreesToRadians = 0.0174533f;
        public static readonly float RadiansToDegrees = 57.2958f;
    }
}
