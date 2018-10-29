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
        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public float Radius;

        /// <summary>
        /// The center position of the sphere.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Constructs a sphere.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        public Sphere(float radius)
        {
            Radius = radius;
            Position = Vector3.Zero;
        }

        /// <summary>
        /// Constructs a sphere.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        public Sphere(float radius, Vector3 position)
        {
            Radius = radius;
            Position = position;
        }

        /// <summary>
        /// Computes the axis aligned bounding box given the specified transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public override Bounds3 CalculateBounds(Matrix4 transform)
        {
            var center = Matrix4.Transform(Position, transform);
            var offset = new Vector3(Radius);

            return new Bounds3(center - offset, center + offset);
        }
    }
}
