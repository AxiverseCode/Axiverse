using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// A three dimensional ray.
    /// </summary>
    public struct Line3
    {
        /// <summary>
        /// The origin of the ray.
        /// </summary>
        public Vector3 Origin;

        /// <summary>
        /// The direction vector of the ray.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Constructs a line.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="origin"></param>
        public Line3(Vector3 direction, Vector3 origin = default(Vector3))
        {
            Direction = direction;
            Origin = origin;
        }

        public static bool TryFindClosestPoint(Line3 former, Line3 latter, out Vector3 formerPoint, out Vector3 latterPoint)
        {
            Vector3 cross = Vector3.Cross(former.Direction, latter.Direction);
            float length = cross.Length();
            if (length == 0)
            {
                formerPoint = Vector3.Zero;
                latterPoint = Vector3.Zero;
                return false;
            }

            Plane3 formerPlane = Plane3.FromPointTangents(former.Origin, cross, former.Direction);
            Plane3 latterPlane = Plane3.FromPointTangents(latter.Origin, cross, latter.Direction);

            var intersectsFormer = latterPlane.Intersect(former, out formerPoint);
            var intersectsLatter = formerPlane.Intersect(latter, out latterPoint);

            // Should always be true.
            return intersectsFormer && intersectsLatter;
        }

        public static float Distance(Line3 former, Line3 latter)
        {
            var cross = Vector3.Cross(former.Direction, latter.Direction);
            return Math.Abs(Vector3.Dot(latter.Origin - former.Origin, cross)) / cross.Length();
        }

        public static float DistanceSquared(Line3 former, Line3 latter)
        {
            var cross = Vector3.Cross(former.Direction, latter.Direction);
            float dot = Vector3.Dot(latter.Origin - former.Origin, cross);
            return dot * dot / cross.LengthSquared();
        }

        public float Distance(Vector3 vector)
        {
            float offset = Vector3.Dot(Direction, vector - Origin) / Vector3.Dot(Direction, Direction);
            return Vector3.Distance(vector, Origin + offset * Direction);
        }
    }
}
