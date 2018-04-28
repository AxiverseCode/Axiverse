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

        public bool Any()
        {
            return X || Y || Z;
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

        public bool All()
        {
            return X && Y && Z;
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
        /// Gets the signed unit vector based on each boolean component.
        /// </summary>
        /// <returns>Signed unit vector.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(
                X ? 1 : -1,
                Y ? 1 : -1,
                Z ? 1 : -1);
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

        /// <summary>
        /// Performs a boolean exclusive or between two Boolean3 structures.
        /// 
        /// A | B | A ^ B
        /// --+---+------
        /// 0 | 0 |   0
        /// 0 | 1 |   1
        /// 1 | 0 |   1
        /// 1 | 1 |   0
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Boolean3 operator ^(Boolean3 left, Boolean3 right)
        {
            return new Boolean3(left.X ^ right.X,
                                left.Y ^ right.Y,
                                left.Z ^ right.Z);
        }

        /// <summary>
        /// Returns the negation of a Boolean3 structure.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean3 operator !(Boolean3 value)
        {
            return new Boolean3(!value.X, !value.Y, !value.Z);
        }

        /// <summary>
        /// Casts a Boolean3 into a integer.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator int(Boolean3 value)
        {
            return (value.X ? 4 : 0)
                | (value.Y ? 2 : 0)
                | (value.Z ? 1 : 0);
        }

        /// <summary>
        /// Casts an integer into a Boolean3.
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator Boolean3(int value)
        {
            return new Boolean3((value & 4) != 0, (value & 2) != 0, (value & 1) != 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly Boolean3[] Domain = new Boolean3[] {
            (Boolean3)0, (Boolean3)1, (Boolean3)2, (Boolean3)3,
            (Boolean3)4, (Boolean3)5, (Boolean3)6, (Boolean3)7};
    }
}
