using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Triangle3
    {
        /// <summary>
        /// Vertex A of the triangle.
        /// </summary>
        public Vector3 A;

        /// <summary>
        /// Vertex B of the triangle.
        /// </summary>
        public Vector3 B;

        /// <summary>
        /// Vertex C of the triangle.
        /// </summary>
        public Vector3 C;

        /// <summary>
        /// Gets the normal based on TODO(axiverse): define winding.
        /// </summary>
        public Vector3 Normal => (B - A) % (C - A);

        /// <summary>
        /// Constructs a new triangle.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Triangle3(Vector3 a = default(Vector3), Vector3 b = default(Vector3), Vector3 c = default(Vector3))
        {
            A = a;
            B = b;
            C = c;
        }

        public bool Intersect(Ray3 ray, out Vector3 intersection)
        {
            // https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
            var ba = B - A;
            var ca = C - A;

            var h = Vector3.Cross(ray.Direction, ca);
            var a = Vector3.Dot(ba, h);

            if (a == 0)
            {
                // Ray is parallel.
                intersection = Vector3.Zero;
                return false;
            }

            var f = 1 / a;
            var s = ray.Origin - A;
            var u = f * Vector3.Dot(s, h);
            if (u.WithinInclusive(0, 1))
            {
                // Outside of triangle.
                intersection = Vector3.Zero;
                return false;
            }

            var q = Vector3.Cross(s, ba);
            var v = f * Vector3.Dot(ray.Direction, q);
            if (v.WithinInclusive(0, 1))
            {
                // Outside of triangle.
                intersection = Vector3.Zero;
                return false;
            }

            float t = f * Vector3.Dot(ca, q);
            if (t < 0)
            {
                // Behind the ray.
                intersection = Vector3.Zero;
                return false;
            }

            intersection = ray.Origin * t * ray.Direction;
            return true;
        }
    }
}
