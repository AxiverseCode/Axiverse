using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
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

        public Vector3 Normal => new Vector3(A, B, C);

        public Plane3(float a, float b, float c, float d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Plane3(Vector3 normal, float d)
        {
            A = normal.X;
            B = normal.Y;
            C = normal.Z;
            D = d;
        }

        public Plane3 Normalized()
        {
            float distance = Functions.Sqrt(A * A + B * B + C * C);
            return new Plane3(A / distance, B / distance, C / distance, D / distance);
        }

        public bool Intersect(Line3 line, out Vector3 point)
        {
            return Intersect(line.Origin, line.Origin + line.Direction, out point);
        }

        public bool Intersect(Vector3 lineFormer, Vector3 lineLatter, out Vector3 point)
        {
            var direction = lineLatter - lineFormer;
            float denominator = A * direction.X + B * direction.Y + C * direction.Z;

            if (denominator == 0)
            {
                point = Vector3.NaN;
                return false;
            }

            float offset = (A * lineFormer.X + B * lineFormer.Y + C * lineFormer.Z + D) / denominator;
            point = lineFormer + offset * direction;
            return true;
        }

        /// <summary>
        /// Computes the ratio of the the two points where the line segment intersects the plane.
        /// If the value is less than 0 or greater than 1, the intersection point is outside the
        /// line segment. If the line is parallel to the plane, NaN is returned.
        /// </summary>
        /// <param name="lineFormer"></param>
        /// <param name="lineLatter"></param>
        /// <returns></returns>
        public float IntersectRatio(Vector3 lineFormer, Vector3 lineLatter)
        {
            var direction = lineLatter - lineFormer;
            float denominator = A * direction.X + B * direction.Y + C * direction.Z;

            if (denominator == 0)
            {
                return float.NaN;
            }

            return (A * lineFormer.X + B * lineFormer.Y + C * lineFormer.Z + D) / -denominator;
        }

        /// <summary>
        /// Fines the line intersection of two planes.
        /// </summary>
        /// <param name="former"></param>
        /// <param name="latter"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool Intersection(Plane3 former, Plane3 latter, out Line3 line)
        {
            float denominator = former.A * latter.B - former.B * latter.A;

            if (denominator == 0)
            {
                // TODO(axiverse): switch axis
                line = default(Line3);
                return false;
            }

            line.Origin = new Vector3(
                (latter.D * former.B - former.D * latter.B) / denominator,
                (former.D * latter.A - latter.D * former.A) / denominator,
                0);

            var normal = Vector3.Cross(former.Normal, latter.Normal);
            var length = normal.Length();

            if (length == 0)
            {
                line = default(Line3);
                return false;
            }

            line.Direction = normal / length;
            return true;
        }

        public Vector3 ClosestPoint(Vector3 vector)
        {
            return vector - Normal * Distance(vector);
        }

        public float Distance(Vector3 vector)
        {
            return A * vector.X + B * vector.Y + C * vector.Z + D;
        }

        public float AbsoluteDistance(Vector3 vector)
        {
            return Math.Abs(Distance(vector));
        }

        public static Plane3 FromPointNormal(Vector3 point, Vector3 normal)
        {
            var normalized = normal.Normal();
            var d = -Vector3.Dot(point, normalized);
            return new Plane3(normalized, d);
        }

        public static Plane3 FromPointTangents(Vector3 point, Vector3 tangent, Vector3 bitangent)
        {
            var normal = Vector3.Cross(tangent, bitangent);
            return FromPointNormal(point, normal);
        }


        public static Plane3 FromTriangle(Triangle3 triangle)
        {
            return FromTriangle(triangle.A, triangle.B, triangle.C);
        }

        public static Plane3 FromTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            return FromPointTangents(a, b - a, c - a);
        }
    }
}
