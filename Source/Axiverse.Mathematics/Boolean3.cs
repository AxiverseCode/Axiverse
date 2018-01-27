using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// Represents a 3-dimmensional boolean.
    /// </summary>
    public struct Boolean3
    {
        /// <summary>Gets or sets the X component.</summary>
        public bool X;
        /// <summary>Gets or sets the Y component.</summary>
        public bool Y;
        /// <summary>Gets or sets the Z component.</summary>
        public bool Z;

        /// <summary>
        /// Initializes a Boolean3.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Boolean3(bool x, bool y, bool z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Determines if any of the components are equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Any(bool value)
        {
            return (X == value) || (Y == value) || (Z == value);
        }

        /// <summary>
        /// Determines if all of the components are equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool All(bool value)
        {
            return (X == value) && (Y == value) && (Z == value);
        }

        /// <summary>
        /// Determines if the majority of the components are true.
        /// </summary>
        /// <returns></returns>
        public bool Majority()
        {
            return ((X ? 1 : 0) + (Y ? 1 : 0) + (Z ? 1 : 0)) > 1;
        }

        /// <summary>
        /// Determines if the minority of the components are true.
        /// </summary>
        /// <returns></returns>
        public bool Minority()
        {
            return ((X ? 1 : 0) + (Y ? 1 : 0) + (Z ? 1 : 0)) <= 1;
        }

        /// <summary>
        /// Performs a boolean and between two Boolean3 structures.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Boolean3 operator &(Boolean3 left, Boolean3 right)
        {
            return new Boolean3(left.X && right.X,
                                left.Y && right.Y,
                                left.Z && right.Z);
        }

        /// <summary>
        /// Performs a boolean or between two Boolean3 structures.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Boolean3 operator |(Boolean3 left, Boolean3 right)
        {
            return new Boolean3(left.X || right.X,
                                left.Y || right.Y,
                                left.Z || right.Z);
        }
    }
}
