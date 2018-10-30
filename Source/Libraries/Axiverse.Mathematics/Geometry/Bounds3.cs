using System;

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
        /// Gets the size of the bounding box.
        /// </summary>
        public Vector3 Size => Maximum - Minimum;

        /// <summary>
        /// Gets the center of the bounding box.
        /// </summary>
        public Vector3 Center => (Maximum + Minimum) / 2;

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
        public bool Contains(Vector3 vector) => Contains(ref this, ref vector);

        /// <summary>
        /// Determines if the specified bounds intersects with this Bounds3 structure.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public bool Intersects(Bounds3 bounds) => Intersects(ref this, ref bounds);

        /// <summary>
        /// Creates a bounding box from two vectors regardless of ordering.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Bounds3 FromVectors(Vector3 left, Vector3 right)
        {
            return new Bounds3(
                Math.Min(left.X, right.X), Math.Max(left.X, right.X),
                Math.Min(left.Y, right.Y), Math.Max(left.Y, right.Y),
                Math.Min(left.Z, right.Z), Math.Max(left.Z, right.Z));
        }

        /// <summary>
        /// Creates a bounding box from the center point and given size.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bounds3 FromCenter(Vector3 center, Vector3 size)
        {
            Vector3 extent = size / 2;
            return new Bounds3(center - size, center + size);
        }

        /// <summary>
        /// Normalizes the bounds and ensures that the maximum values and minimum values are in the right place.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bounds3 Normalize(Bounds3 value)
        {
            return new Bounds3(
                Math.Min(value.Minimum.X, value.Maximum.X), Math.Max(value.Minimum.X, value.Maximum.X),
                Math.Min(value.Minimum.Y, value.Maximum.Y), Math.Max(value.Minimum.Y, value.Maximum.Y),
                Math.Min(value.Minimum.Z, value.Maximum.Z), Math.Max(value.Minimum.Z, value.Maximum.Z));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static bool Contains(ref Bounds3 bounds, ref Vector3 vector)
        {
            return
                (vector.X >= bounds.Minimum.X && vector.X <= bounds.Maximum.X) &&
                (vector.Y >= bounds.Minimum.Y && vector.Y <= bounds.Maximum.Y) &&
                (vector.Z >= bounds.Minimum.Z && vector.Z <= bounds.Maximum.Z);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Intersects(ref Bounds3 left, ref Bounds3 right)
        {
            return
                ((left.Minimum.X <= right.Minimum.X && right.Minimum.X <= left.Maximum.X) || (right.Minimum.X <= left.Minimum.X && left.Minimum.X <= right.Maximum.X)) &&
                ((left.Minimum.Y <= right.Minimum.Y && right.Minimum.Y <= left.Maximum.Y) || (right.Minimum.Y <= left.Minimum.Y && left.Minimum.Y <= right.Maximum.Y)) &&
                ((left.Minimum.Z <= right.Minimum.Z && right.Minimum.Z <= left.Maximum.Z) || (right.Minimum.Z <= left.Minimum.Z && left.Minimum.Z <= right.Maximum.Z));
        }
    }
}
