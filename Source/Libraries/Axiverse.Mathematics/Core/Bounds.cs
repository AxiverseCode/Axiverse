using System;

namespace Axiverse
{
    /// <summary>
    /// Represents a boundary.
    /// </summary>
    public struct Bounds
    {
        /// <summary>
        /// The minimum value of the bounding box.
        /// </summary>
        public float Minimum;

        /// <summary>
        /// The maximum value of the bounding box.
        /// </summary>
        public float Maximum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public Bounds(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Determines if the Bound contains the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(float value)
        {
            return value >= Minimum && value <= Maximum;
        }

        /// <summary>
        /// Determines if the given Bound intersects with this Bound.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Intersects(Bounds value)
        {
            return !(value.Minimum > Maximum || value.Maximum < Minimum);
        }

        /// <summary>
        /// Computes the union with the given Bound.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bounds Union(Bounds value)
        {
            return new Bounds(Math.Min(Minimum, value.Minimum), Math.Max(Maximum, value.Maximum));
        }

        /// <summary>
        /// Computes the intersection to the given Bound.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bounds Intersection(Bounds value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clamps the given value to within the given boundaries.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float Clamp(float value)
        {
            return Math.Max(Minimum, Math.Min(Maximum, value));
        }

        /// <summary>The not-a-number bounds.</summary>
        public static readonly Bounds NaN = new Bounds(float.NaN, float.NaN);
    }
}
