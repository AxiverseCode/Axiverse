using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// A 3-dimmensional axis aligned bounding box.
    /// </summary>
    public struct Bounds3
    {
        /// <summary>
        /// The minimum corner of the bounding box.
        /// </summary>
        public Vector3 Minimum;

        /// <summary>
        /// The maximum corner of the bounding box.
        /// </summary>
        public Vector3 Maximum;

        /// <summary>
        /// Initializes a new instance of the Bounds3 class with the specified minimum and maximum vertices.
        /// </summary>
        /// <param name="minimum">The minimum vertex.</param>
        /// <param name="maximum">The maximum vertex.</param>
        public Bounds3(Vector3 minimum, Vector3 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Initializes a new instance of the Bounds3 class with the specified minimum and maximum vertices.
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="minZ"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="maxZ"></param>
        public Bounds3(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            Minimum = new Vector3(minX, minY, minZ);
            Maximum = new Vector3(maxX, maxY, maxZ);
        }

        /// <summary>
        /// Determines if the specified point is contained within this Bounds3 structure.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool Contains(Vector3 vector)
        {
            return (vector.X >= Minimum.X && vector.Y >= Minimum.Y && vector.Z >= Minimum.Z)
                && (vector.X <= Maximum.X && vector.Y <= Maximum.Y && vector.Z <= Maximum.Z);
        }

        /// <summary>
        /// Determines if the specified bounds intersects with this Bounds3 structure.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public bool Intersects(Bounds3 bounds)
        {
            // TODO: this is probably wrong
            throw new NotImplementedException();
            return (Minimum.X <= bounds.Maximum.X && Minimum.Y <= bounds.Maximum.Y && Minimum.Z <= bounds.Maximum.Z)
                && (Maximum.X >= bounds.Minimum.X && Maximum.Y >= bounds.Minimum.Y && Maximum.Z >= bounds.Minimum.Z);
        }

        /// <summary>
        /// Determines if the specified sphere intersects with this Bounds3 structure.
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns></returns>
        public bool Intersects(Sphere3 sphere)
        {
            return sphere.Intersects(this);
        }

        public static bool Intersects(ref Bounds3 left, ref Bounds3 right)
        {
            return left.Intersects(right);
        }
    }
}
