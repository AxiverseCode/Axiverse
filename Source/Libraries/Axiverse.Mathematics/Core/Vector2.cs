using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Axiverse
{
    /// <summary>
    /// Represents a 2-dimensional Cartesian vector
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        #region Members

        /// <summary>Gets or sets the X component.</summary>
        public float X;

        /// <summary>Gets or sets the Y component.</summary>
        public float Y;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component at the given index.
        /// </summary>
        /// <param name="index">The index of the component.</param>
        /// <returns></returns>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        public Vector2(float value)
        {
            X = Y = value;
        }

        public Vector2(float x = 0f, float y = 0f)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Instance Methods

        #region Instance Methods - Setters

        public Vector2 Set(float x, float y)
        {
            X = x;
            Y = y;
            return this;
        }

        public Vector2 SetLength(float l)
        {
            return Normalize().Multiply(l);
        }

        public Vector2 Copy(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
            return this;
        }

        #endregion

        #region Instance Methods - Arithmatic

        public Vector2 Add(Vector2 v1, Vector2 v2)
        {
            X = v1.X + v2.X;
            Y = v1.Y + v2.Y;
            return this;
        }

        public Vector2 Add(Vector2 v)
        {
            X += v.X;
            Y += v.Y;
            return this;
        }

        public Vector2 Add(float s)
        {
            X += s;
            Y += s;
            return this;
        }

        public Vector2 Subtract(Vector2 v1, Vector2 v2)
        {
            X = v1.X - v2.X;
            Y = v1.Y - v2.Y;
            return this;
        }

        public Vector2 Subtract(Vector2 v)
        {
            X -= v.X;
            Y -= v.Y;
            return this;
        }

        public Vector2 Subtract(float s)
        {
            X -= s;
            Y -= s;
            return this;
        }

        public Vector2 Multiply(Vector2 a, Vector2 b)
        {
            X = a.X * b.X;
            Y = a.Y * b.Y;
            return this;
        }

        public Vector2 Multiply(Vector2 v)
        {
            X *= v.X;
            Y *= v.Y;
            return this;
        }

        public Vector2 Multiply(float s)
        {
            X *= s;
            Y *= s;

            return this;
        }

        public Vector2 Divide(Vector2 v)
        {
            X /= v.X;
            Y /= v.Y;
            return this;
        }

        public Vector2 Divide(float s)
        {
            X /= s;
            Y /= s;
            return this;
        }

        public Vector2 Negate()
        {
            return Multiply(-1);
        }

        #endregion

        #region Instance Methods - Mathematics

        /// <summary>
        /// Returns this vector after normalizing. The length of the vector
        /// will be 1.
        /// </summary>
        public Vector2 Normalize()
        {
            return Divide(this.Length());
        }

        /// <summary>
        /// Returns the dot product between this and the target vector.
        /// </summary>
        /// <param name="v">A vector to calculate the dot product with</param>
        public float Dot(Vector2 v)
        {
            return X * v.X + Y * v.Y;
        }

        /// <summary>
        /// Returns the length of the vector. For length comparisons, use
        /// LengthSquared for better performance.
        /// </summary>
        /// <see cref="Axiverse.Mathematics.Vector2.LengthSquared"/>
        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Returns the square of the length of the vector. Runs faster than
        /// computing the length.
        /// </summary>
        /// <returns>The square.</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// Returns the manhatten length of the vector.
        /// </summary>
        /// <returns>The length.</returns>
        public float ManhattanLength()
        {
            return Math.Abs(X) + Math.Abs(Y);
        }

        /// <summary>
        /// Returns the distance between this and the specified vector.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="v">V.</param>
        public float DistanceTo(Vector2 v)
        {
            return (float)Math.Sqrt(DistanceToSquared(v));
        }

        /// <summary>
        /// Returns the square of the distance between this and the specified
        /// vector.
        /// </summary>
        /// <returns>The to squared.</returns>
        /// <param name="v">V.</param>
        public float DistanceToSquared(Vector2 v)
        {
            float dx = v.X - X;
            float dy = v.Y - Y;

            return dx * dx + dy * dy;
        }

        public float ManhattanDistanceTo(Vector2 v)
        {
            float dx = v.X - X;
            float dy = v.Y - Y;

            return Math.Abs(dx) + Math.Abs(dy);
        }

        /// <summary>
        /// Determines whether this instance is zero.
        /// </summary>
        /// <returns><c>true</c> if this instance is zero; otherwise, <c>false</c>.</returns>
        public bool IsZero()
        {
            return (LengthSquared() < 0.0001 /* almostZero */);
        }

        /// <summary>
        /// Returns this vector with each component set to the maximum value of that
        /// component in the given list of vectors.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        public Vector2 Max(IEnumerable<Vector2> a)
        {
            X = float.NegativeInfinity;
            Y = float.NegativeInfinity;

            foreach (var v in a)
            {
                X = Math.Max(v.X, X);
                Y = Math.Max(v.Y, Y);
            }

            return this;
        }

        /// <summary>
        /// Returns this vector with each component set to the minimum value of that
        /// component in the given list of vectors.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        public Vector2 Min(IEnumerable<Vector2> a)
        {
            X = float.PositiveInfinity;
            Y = float.PositiveInfinity;

            foreach (var v in a)
            {
                X = Math.Min(v.X, X);
                Y = Math.Min(v.Y, Y);
            }

            return this;
        }

        public Vector2 Midpoint(Vector2 l, Vector2 r)
        {
            X = (l.X + r.X) / 2;
            Y = (l.Y + r.Y) / 2;
            return this;
        }

        #endregion

        #region Instance Methods - Overrides

        public override bool Equals(object obj)
        {
            if (obj is Vector2 v)
            {
                return (v.X == X && v.Y == Y);
            }
            return false;

        }

        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        #endregion

        #endregion

        #region Static Methods

        #region Static Methods - Operators

        public static Vector2 operator +(Vector2 a)
        {
            return new Vector2(a.X, a.Y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.X, -a.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(a * b.X, a * b.Y);
        }

        public static Vector2 operator *(Vector2 b, float a)
        {
            return new Vector2(a * b.X, a * b.Y);
        }

        public static Vector2 operator /(float a, Vector2 b)
        {
            return new Vector2(a / b.X, a / b.Y);
        }

        public static Vector2 operator /(Vector2 b, float a)
        {
            return new Vector2(a / b.X, a / b.Y);
        }

        public static bool operator ==(Vector2 former, Vector2 latter)
        {
            return (former.X == latter.X) && (former.Y == latter.Y);
        }

        public static bool operator !=(Vector2 former, Vector2 latter)
        {
            return (former.X == latter.X) && (former.Y == latter.Y);
        }

        #endregion

        public static Vector2 Minimum(Vector2 former, Vector2 latter)
        {
            return new Vector2(
                Math.Min(former.X, latter.X),
                Math.Min(former.Y, latter.Y)
                );
        }

        public static Vector2 Minimum(IEnumerable<Vector2> vectors)
        {
            var minimum = PositiveInfinity;
            foreach (var vector in vectors)
            {
                minimum = Minimum(minimum, vector);
            }
            return minimum;
        }

        public static Vector2 Maximum(Vector2 former, Vector2 latter)
        {
            return new Vector2(
                Math.Max(former.X, latter.X),
                Math.Max(former.Y, latter.Y)
                );
        }

        public static Vector2 Maximum(IEnumerable<Vector2> vectors)
        {
            var maximum = PositiveInfinity;
            foreach (var vector in vectors)
            {
                maximum = Maximum(maximum, vector);
            }
            return maximum;
        }

        #region Static Methods - Conversion

        /// <summary>
        /// Converts a string representation of a 2 dimensional vector to a Vector2
        /// </summary>
        /// <param name="s">S.</param>
        public static Vector2 Parse(string s)
        {
            var pattern = @"^[\(\{\[\s]*([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\)\}\]\s]*$";
            var match = Regex.Match(s, pattern, RegexOptions.IgnoreCase);

            var vector = new Vector2();
            vector.X = float.Parse(match.Groups[0].Value);
            vector.Y = float.Parse(match.Groups[1].Value);

            return vector;
        }

        #endregion

        #endregion


        /// <summary>The up vector. [0, 1]</summary>
        public static readonly Vector2 Up = new Vector2(0, 1);

        /// <summary>The down vector. [0, -1]</summary>
        public static readonly Vector2 Down = new Vector2(0, -1);

        /// <summary>The left vector. [-1. 0]</summary>
        public static readonly Vector2 Left = new Vector2(-1, 0);

        /// <summary>The right vector. [1, 0]</summary>
        public static readonly Vector2 Right = new Vector2(1, 0);

        /// <summary>The zero vector.</summary>
        public static readonly Vector2 Zero = new Vector2(0, 0);

        /// <summary>The one vector.</summary>
        public static readonly Vector2 One = new Vector2(1, 1);

        /// <summary>The X unit vector.</summary>
        public static readonly Vector2 UnitX = new Vector2(1, 0);

        /// <summary>The Y unit vector.</summary>
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        /// <summary>The not-a-number vector.</summary>
        public static readonly Vector2 NaN = new Vector2(float.NaN, float.NaN);

        /// <summary>The negative infinity vector.</summary>
        public static readonly Vector2 NegativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>The positive infinity vector.</summary>
        public static readonly Vector2 PositiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    }
}