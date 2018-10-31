using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// A three-dimensional line segment between two points.
    /// </summary>
    public struct Segment3
    {
        /// <summary>
        /// The U point of the line segment.
        /// </summary>
        public Vector3 U;

        /// <summary>
        /// The V point of the line segment.
        /// </summary>
        public Vector3 V;

        public Segment3(Vector3 u, Vector3 v)
        {
            U = u;
            V = v;
        }
    }
}
