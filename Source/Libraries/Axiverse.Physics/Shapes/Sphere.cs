using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Shapes
{
    /// <summary>
    /// A sphere collision shape.
    /// </summary>
    public class Sphere : Shape
    {
        public float Radius;
        public Vector3 Position;




        public Bounds3 GetBounds()
        {
            return new Bounds3();
        }
    }
}
