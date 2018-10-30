using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Tetrahedron3
    {
        public Vector3 A;

        public Vector3 B;

        public Vector3 C;

        public Vector3 D;

        public Tetrahedron3(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }


        public bool Contains(Vector3 vector)
        {
            // Check that point P is on the same side as the remaining vertex for each face.
            return 
                SameSide(ref A, ref B, ref C, ref D, ref vector) &&
                SameSide(ref B, ref C, ref D, ref A, ref vector) &&
                SameSide(ref C, ref D, ref A, ref B, ref vector) &&
                SameSide(ref D, ref A, ref B, ref C, ref vector);
        }

        /// <summary>
        /// Checks if the point P is on the same side of plane ABC as point D.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool SameSide(ref Vector3 a,  ref Vector3 b,  ref Vector3 c, ref Vector3 d, ref Vector3 p)
        {
            var normal = Vector3.Cross(b - a, c - a);
            var dotD = Vector3.Dot(normal, d - a);
            var dotP = Vector3.Dot(normal, p - a);

            return Math.Sign(dotD) == Math.Sign(dotP);
        }

        public float Determinant3(Matrix4 matrix)
        {
            // http://steve.hollasch.net/cgindex/geometry/ptintet.html
            throw new NotImplementedException();
        }
    }
}
