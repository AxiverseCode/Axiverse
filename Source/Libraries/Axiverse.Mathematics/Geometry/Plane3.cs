using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// 
    /// Ax + By + Cz + D = 0;
    /// </summary>
    class Plane3
    {
        public float A;

        public float B;

        public float C;

        public float D;

        public Plane3(Vector3 normal, float d)
        {
            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = d;
        }

        public static Plane3 FromPointNormal(Vector3 point, Vector3 normal)
        {
            var normalized = normal.Normal();
            var d = -Vector3.Dot(point, normalized);
            return new Plane3(normalized, d);
        }
    }
}
