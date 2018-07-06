using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Ray3
    {
        public Vector3 Origin;

        public Vector3 Direction;

        public Ray3(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public static Ray3 FromScreen(int x, int y, Viewport viewport, Matrix4 worldViewProjection)
        {
            var near = new Vector3(x, y, 0);
            var far = new Vector3(x, y, 1);

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
