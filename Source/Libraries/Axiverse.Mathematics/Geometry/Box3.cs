using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Box3
    {
        Vector3 Extents;

        Matrix4 Transform;

        public Box3(Bounds3 bounds)
        {
            var origin = bounds.Minimum + (bounds.Maximum - bounds.Minimum) / 2;
            Extents = bounds.Maximum - origin;
            Transform = Matrix4.Translation(origin);
        }
    }
}
