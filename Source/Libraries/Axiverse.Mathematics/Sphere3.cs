using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// Represents an object with constant radius in a three-dimensional space.
    /// </summary>
    public struct Sphere3
    {
        /// <summary>
        /// Gets or sets the center position of the sphere.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets the radius of the sphere.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Gets or sets the X component of the center position of the sphere.
        /// </summary>
        public float X { get { return Position.X; } set { Position.X = value; } }

        /// <summary>
        /// Gets or sets the Y component of the center position of the sphere.
        /// </summary>
        public float Y { get { return Position.Y; } set { Position.Y = value; } }

        /// <summary>
        /// Gets or sets the Z component of the center position of the sphere.
        /// </summary>
        public float Z { get { return Position.Z; } set { Position.Z = value; } }

        /// <summary>
        /// Constructs a sphere with the given center position and radius.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public Sphere3(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// Determines whether the vector is within or on the border of the sphere.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool Contains(Vector3 vector)
        {
            return Vector3.DistanceSquared(ref Position, ref vector) <= Radius * Radius;
        }

        /// <summary>
        /// Determines whether the specified sphere intersects this sphere.
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns></returns>
        public bool Intersects(Sphere3 sphere)
        {
            var distanceSquared = Vector3.DistanceSquared(ref Position, ref sphere.Position);
            var jointRadius = (Radius + sphere.Radius);
            return distanceSquared <= jointRadius * jointRadius;
        }

        /// <summary>
        /// Determines whether the specified bounds instersects this sphere.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public bool Intersects(Bounds3 bounds)
        {
            Functions.Clamp(ref Position, ref bounds.Minimum, ref bounds.Maximum, out var closest);
            var distance = Vector3.Distance(Position, closest);
            return distance < Radius;
        }
    }
}
