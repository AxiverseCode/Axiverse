using Axiverse.Mathematics;
using System;
using System.Collections.Generic;

namespace Axiverse
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

        public bool Contains(Bounds3 bounds) => Contains(ref this, ref bounds);

        public override string ToString()
        {
            return $"⌊{Minimum.X}, {Minimum.Y}, {Minimum.Z}⌋ ⌈{Maximum.X}, {Maximum.Y}, {Maximum.Z}⌉";
        }

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
                Math.Min(left.X, right.X), Math.Min(left.Y, right.Y), Math.Min(left.Z, right.Z),
                Math.Max(left.X, right.X), Math.Max(left.Y, right.Y), Math.Max(left.Z, right.Z));
        }

        /// <summary>
        /// Creates a bounding box containing all vectors.
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Bounds3 FromVectors(IEnumerable<Vector3> vectors)
        {
            var minimum = Vector3.Minimum(vectors);
            var maximum = Vector3.Maximum(vectors);

            return new Bounds3(minimum, maximum);
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

        public static bool Contains(ref Bounds3 container, ref Bounds3 bounds)
        {
            return
                (bounds.Minimum.X >= container.Minimum.X && bounds.Maximum.X <= container.Maximum.X) &&
                (bounds.Minimum.Y >= container.Minimum.Y && bounds.Maximum.Y <= container.Maximum.Y) &&
                (bounds.Minimum.Z >= container.Minimum.Z && bounds.Maximum.Z <= container.Maximum.Z);
        }

        public static Bounds3 Expand(Bounds3 bounds, float scale, Vector3 center)
        {
            var minimum = (bounds.Minimum - center) * scale + center;
            var maximum = (bounds.Maximum - center) * scale + center;
            return new Bounds3(minimum, maximum);
        }

        public static Bounds3 ExpandFromOrigin(Bounds3 bounds, float scale)
        {
            return new Bounds3(bounds.Minimum * scale, bounds.Maximum * scale);
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

        public bool Intersects(Line3 line)
        {
            float near = float.NegativeInfinity;
            float far = float.PositiveInfinity;

            return IntersectsLine(ref line.Origin, ref line.Direction, ref near, ref far);
        }

        public bool Intersects(Segment3 segment)
        {
            Vector3 direction = segment.V - segment.U;
            var length = direction.Normalize();

            float near = 0;
            float far = length;

            return IntersectsLine(ref segment.U, ref direction, ref near, ref far);
        }

        public bool Intersects(Ray3 ray)
        {
            float near = 0;
            float far = float.PositiveInfinity;

            return IntersectsLine(ref ray.Origin, ref ray.Direction, ref near, ref far);
        }

        private bool IntersectsLine(ref Vector3 origin, ref Vector3 direction, ref float near, ref float far)
        {
            https://github.com/juj/MathGeoLib/blob/master/src/Geometry/AABB.cpp

            // X axis.
            if (Math.Abs(direction.X) != 0)
            {
                float former = (Minimum.X - origin.X) / direction.X;
                float latter = (Maximum.X - origin.X) / direction.X;

                if (former < latter)
                {
                    near = Math.Max(former, near);
                    far = Math.Min(latter, far);
                }
                else // Swap former and latter.
                {
                    near = Math.Max(latter, near);
                    far = Math.Min(former, far);
                }

                if (near > far)
                {
                    return false;
                }
            }
            else if (origin.X < Minimum.X || origin.X > Maximum.X)
            {
                return false;
            }

            // Y axis.
            if (Math.Abs(direction.Y) != 0)
            {
                float former = (Minimum.Y - origin.Y) / direction.Y;
                float latter = (Maximum.Y - origin.Y) / direction.Y;

                if (former < latter)
                {
                    near = Math.Max(former, near);
                    far = Math.Min(latter, far);
                }
                else // Swap former and latter.
                {
                    near = Math.Max(latter, near);
                    far = Math.Min(former, far);
                }

                if (near > far)
                {
                    return false;
                }
            }
            else if (origin.Y < Minimum.Y || origin.Y > Maximum.Y)
            {
                return false;
            }

            // Z axis.
            if (Math.Abs(direction.Z) != 0)
            {
                float former = (Minimum.Z - origin.Z) / direction.Z;
                float latter = (Maximum.Z - origin.Z) / direction.Z;

                if (former < latter)
                {
                    near = Math.Max(former, near);
                    far = Math.Min(latter, far);
                }
                else // Swap former and latter.
                {
                    near = Math.Max(latter, near);
                    far = Math.Min(former, far);
                }

                if (near > far)
                {
                    return false;
                }
            }
            else if (origin.Z < Minimum.Z || origin.Z > Maximum.Z)
            {
                return false;
            }

            return true;
        }
    }
}
