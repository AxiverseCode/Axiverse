using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Shapes
{
    /// <summary>
    /// A collision shape.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Computes the axis aligned bounding box given the specified transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public abstract Bounds3 CalculateBounds(Matrix4 transform);
    }
}
