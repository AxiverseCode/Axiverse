using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    public struct Triangle
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
        public Triangle(Vector3 a = default(Vector3), Vector3 b = default(Vector3), Vector3 c = default(Vector3))
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
