using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// A two dimensional ray.
    /// </summary>
    public struct Ray2
    {
        /// <summary>
        /// The origin of the ray.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The direction vector of the ray.
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// Constructs a two-dimensional ray.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        public Ray2(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
