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
    public struct Ray3
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
        /// Constructs a three-dimensional ray.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        public Ray3(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"Origin { Origin }, Direction { Direction }";
        }

        public float Distance(Vector3 point)
        {
            float distance = Vector3.Distance(Origin, point);
            float angle = Vector3.Angle(Direction, point - Origin);
            return distance * Functions.Sin(angle);
        }

        public static Ray3 FromScreen(float x, float y, Viewport viewport, Matrix4 worldViewProjection)
        {
            var near = new Vector3(x, y, viewport.Near);
            var far = new Vector3(x, y, viewport.Far);

            near = Vector3.Unproject(
                near,
                viewport.X,
                viewport.Y,
                viewport.Width,
                viewport.Height,
                viewport.Near,                       
                viewport.Far,
                worldViewProjection);
            far = Vector3.Unproject(
                far,
                viewport.X,
                viewport.Y,
                viewport.Width,
                viewport.Height,
                viewport.Near,
                viewport.Far,
                worldViewProjection);

            Vector3 direction = far - near;
            direction.Normalize();

            return new Ray3(near, direction);
        }
    }
}
