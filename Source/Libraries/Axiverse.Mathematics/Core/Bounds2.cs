using System;
using System.Collections.Generic;

namespace Axiverse
{
    /// <summary>
    /// A 2-dimmensional axis aligned bounding box.
    /// </summary>
    public struct Bounds2
    {
        /// <summary>
        /// The minimum corner of the bounding box.
        /// </summary>
        public Vector2 Minimum;

        /// <summary>
        /// The maximum corner of the bounding box.
        /// </summary>
        public Vector2 Maximum;

        /// <summary>
        /// Gets the size of the bounding box.
        /// </summary>
        public Vector2 Size => Maximum - Minimum;

        /// <summary>
        /// Gets the center of the bounding box.
        /// </summary>
        public Vector2 Center => (Maximum + Minimum) / 2;

        /// <summary>
        /// Initializes a new instance of the Bounds2 class with the specified minimum and maximum vertices.
        /// </summary>
        /// <param name="minimum">The minimum vertex.</param>
        /// <param name="maximum">The maximum vertex.</param>
        public Bounds2(Vector2 minimum, Vector2 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Initializes a new instance of the Bounds2 class with the specified minimum and maximum vertices.
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public Bounds2(float minX, float minY, float maxX, float maxY)
        {
            Minimum = new Vector2(minX, minY);
            Maximum = new Vector2(maxX, maxY);
        }

        /// <summary>
        /// Determines if the specified point is contained within this Bounds2 structure.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool Contains(Vector2 vector) => Contains(ref this, ref vector);

        /// <summary>
        /// Determines if the specified bounds intersects with this Bounds2 structure.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public bool Intersects(Bounds2 bounds) => Intersects(ref this, ref bounds);

        /// <summary>
        /// Creates a bounding box from two vectors regardless of ordering.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Bounds2 FromVectors(Vector2 left, Vector2 right)
        {
            return new Bounds2(
                Math.Min(left.X, right.X), Math.Max(left.X, right.X),
                Math.Min(left.Y, right.Y), Math.Max(left.Y, right.Y));
        }

        /// <summary>
        /// Creates a bounding box containing all vectors.
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns></returns>
        public static Bounds2 FromVectors(IEnumerable<Vector2> vectors)
        {
            var minimum = Vector2.Minimum(vectors);
            var maximum = Vector2.Maximum(vectors);

            return new Bounds2(minimum, maximum);
        }

        /// <summary>
        /// Creates a bounding box from the center point and given size.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bounds2 FromCenter(Vector2 center, Vector2 size)
        {
            Vector2 extent = size / 2;
            return new Bounds2(center - size, center + size);
        }

        /// <summary>
        /// Normalizes the bounds and ensures that the maximum values and minimum values are in the right place.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bounds2 Normalize(Bounds2 value)
        {
            return new Bounds2(
                Math.Min(value.Minimum.X, value.Maximum.X), Math.Max(value.Minimum.X, value.Maximum.X),
                Math.Min(value.Minimum.Y, value.Maximum.Y), Math.Max(value.Minimum.Y, value.Maximum.Y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static bool Contains(ref Bounds2 bounds, ref Vector2 vector)
        {
            return
                (vector.X >= bounds.Minimum.X && vector.X <= bounds.Maximum.X) &&
                (vector.Y >= bounds.Minimum.Y && vector.Y <= bounds.Maximum.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Intersects(ref Bounds2 left, ref Bounds2 right)
        {
            return
                ((left.Minimum.X <= right.Minimum.X && right.Minimum.X <= left.Maximum.X) || (right.Minimum.X <= left.Minimum.X && left.Minimum.X <= right.Maximum.X)) &&
                ((left.Minimum.Y <= right.Minimum.Y && right.Minimum.Y <= left.Maximum.Y) || (right.Minimum.Y <= left.Minimum.Y && left.Minimum.Y <= right.Maximum.Y));
        }

        private bool IntersectsLine(ref Vector2 origin, ref Vector2 direction, ref float near, ref float far)
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
            
            return true;
        }
    }
}
