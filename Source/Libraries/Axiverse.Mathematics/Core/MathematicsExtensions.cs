using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static class MathematicsExtensions
    {
        /// <summary>
        /// Returns a random floating point number that is greater or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Returns a random <see cref="Vector2"/> whose components are greater or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Vector2 NextVector2(this Random random)
        {
            return new Vector2(random.NextFloat(), random.NextFloat());
        }

        /// <summary>
        /// Returns a random <see cref="Vector3"/> whose components are greater or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Vector3 NextVector3(this Random random)
        {
            return new Vector3(random.NextFloat(), random.NextFloat(), random.NextFloat());
        }

        /// <summary>
        /// Returns a random <see cref="Vector4"/> whose components are greater or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Vector4 NextVector4(this Random random)
        {
            return new Vector4(random.NextFloat(), random.NextFloat(), random.NextFloat(), random.NextFloat());
        }
    }
}
