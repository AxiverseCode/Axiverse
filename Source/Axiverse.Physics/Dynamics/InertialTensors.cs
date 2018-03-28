using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Dynamics
{
    /// <summary>
    /// <a href="https://en.wikipedia.org/wiki/List_of_moments_of_inertia">List of moments of inertia</a>
    /// </summary>
    public static class InertialTensors
    {
        /// <summary>
        /// Creates an inertial tensor from a solid sphere.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public static Matrix3 FromSphere(float radius, float mass)
        {
            float a = 2f / 5f * mass * radius * radius;

            return new Matrix3(
                a, 0, 0,
                0, a, 0,
                0, 0, a);
        }

        /// <summary>
        /// Creates an inertial tensor from a hollow sphere.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public static Matrix3 FromHollowSphere(float radius, float mass)
        {
            float a = 2f / 3f * mass * radius * radius;

            return new Matrix3(
                a, 0, 0,
                0, a, 0,
                0, 0, a);
        }

        /// <summary>
        /// Creates an inertial tensor from a solid box.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="mass"></param>
        /// <returns></returns>
        public static Matrix3 FromBox(float x, float y, float z, float mass)
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float m = mass / 12f;

            return new Matrix3(
                m * (y2 + z2), 0, 0,
                0, m * (x2 + z2), 0,
                0, 0, m * (x2 + y2));
        }
    }
}
