using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Axiverse
{
    /// <summary>
    /// Represents a 3-dimensional Cartesian vector
    /// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEnumerable<float>
    {
        #region Members

        /// <summary>Gets or sets the X component.</summary>
        public float X;

        /// <summary>Gets or sets the Y component.</summary>
        public float Y;

        /// <summary>Gets or sets the Z component.</summary>
        public float Z;

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
                    case 2: return Z;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 c)
        {
            X = c.X;
            Y = c.Y;
            Z = c.Z;
        }

        #endregion

        #region Instance Methods

        #region Instance Methods - Setters

        public void Set(float value)
        {
            X = Y = Z = value;
        }

        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void SetLength(float length)
        {
            this = Normal().Multiply(length);
        }

        public Vector3 OfLength(float length)
        {
            return Normal().Multiply(length);
        }

        public Vector3 Copy(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            return this;
        }

        #endregion

        #region Instance Methods - Arithmatic

        public Vector3 Add(Vector3 v)
        {
            X += v.X;
            Y += v.Y;
            Z += v.Z;
            return this;
        }

        public Vector3 Add(float s)
        {
            X += s;
            Y += s;
            Z += s;
            return this;
        }

        //public Vector3 Subtract(Vector3 v1, Vector3 v2)
        //{
        //    X = v1.X - v2.X;
        //    Y = v1.Y - v2.Y;
        //    Z = v1.Z - v2.Z;
        //    return this;
        //}

        public Vector3 Subtract(Vector3 v)
        {
            X -= v.X;
            Y -= v.Y;
            Z -= v.Z;
            return this;
        }

        public Vector3 Subtract(float s)
        {
            X -= s;
            Y -= s;
            Z -= s;
            return this;
        }

        public Vector3 Multiply(Vector3 v)
        {
            X *= v.X;
            Y *= v.Y;
            Z *= v.Z;
            return this;
        }

        public Vector3 Multiply(float s)
        {
            X *= s;
            Y *= s;
            Z *= s;

            return this;
        }

        public Vector3 Divide(Vector3 v)
        {
            X /= v.X;
            Y /= v.Y;
            Z /= v.Z;
            return this;
        }

        public Vector3 Divide(float s)
        {
            X /= s;
            Y /= s;
            Z /= s;
            return this;
        }

        public Vector3 Negate()
        {
            return Multiply(-1);
        }

        #endregion

        #region Instance Methods - Mathematics
        /// <summary>
        /// Returns the dot product between this and the target vector.
        /// </summary>
        /// <param name="v">A vector to calculate the dot product with</param>
        public float Dot(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// Returns the length of the vector. For length comparisons, use
        /// LengthSquared for better performance.
        /// </summary>
        /// <see cref="Axiverse.Vector3.LengthSquared"/>
        public float Length()
        {
            return Functions.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Returns the square of the length of the vector. Runs faster than
        /// computing the length.
        /// </summary>
        /// <returns>The square.</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Returns the manhatten length of the vector.
        /// </summary>
        /// <returns>The length.</returns>
        public float ManhattanLength()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }

        /// <summary>
        /// Returns this vector after normalizing. The length of the vector
        /// will be 1.
        /// </summary>
        public void Normalize()
        {
            Divide(Length());
        }

        public Vector3 Normal()
        {
            return this / Length();
        }

        /// <summary>
        /// Returns this vector set to the cross product of this and the
        /// specified vector.
        /// </summary>
        /// <param name="v">V.</param>
        public Vector3 Cross(Vector3 v)
        {
            var x = X;
            var y = Y;
            var z = Z;

            X = y * v.Z - z * v.Y;
            Y = z * v.X - x * v.Z;
            Z = x * v.Y - y * v.X;

            return this;
        }

        /// <summary>
        /// Returns the distance between this and the specified vector.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="v">V.</param>
        public float DistanceTo(Vector3 v)
        {
            return Functions.Sqrt(DistanceToSquared(v));
        }

        /// <summary>
        /// Returns the square of the distance between this and the specified
        /// vector.
        /// </summary>
        /// <returns>The to squared.</returns>
        /// <param name="v">V.</param>
        public float DistanceToSquared(Vector3 v)
        {
            float dx = v.X - X;
            float dy = v.Y - Y;
            float dz = v.Z - Z;

            return dx * dx + dy * dy + dz * dz;
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
        public Vector3 Max(IEnumerable<Vector3> a)
        {
            X = float.NegativeInfinity;
            Y = float.NegativeInfinity;
            Z = float.NegativeInfinity;

            foreach (var v in a)
            {
                X = Math.Max(v.X, X);
                Y = Math.Max(v.Y, Y);
                Z = Math.Max(v.Z, Z);
            }

            return this;
        }

        /// <summary>
        /// Returns this vector with each component set to the minimum value of that
        /// component in the given list of vectors.
        /// </summary>
        /// <param name="a">The alpha component.</param>
        public Vector3 Min(IEnumerable<Vector3> a)
        {
            X = float.PositiveInfinity;
            Y = float.PositiveInfinity;
            Z = float.PositiveInfinity;

            foreach (var v in a)
            {
                X = Math.Min(v.X, X);
                Y = Math.Min(v.Y, Y);
                Z = Math.Min(v.Z, Z);
            }

            return this;
        }

        public Vector3 Midpoint(Vector3 l, Vector3 r)
        {
            X = (l.X + r.X) / 2;
            Y = (l.Y + r.Y) / 2;
            Z = (l.Z + r.Z) / 2;
            return this;
        }

        public Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }

        // Interpolation
        public Vector3 InterpolateLinear(Vector3 s, Vector3 o, float t)
        {
            float u = 1 - t;

            return new Vector3(
                s.X * u + o.X * t,
                s.Y * u + o.Y * t,
                s.Z * u + o.Z * t
            );
        }

        //public Vector3 InterpolateSpherical(Vector3 s, Vector3 o, float t)
        //{
        //    float dot = Range.Clamp(s.Dot(o), -1, 1);
        //    float theta = (float)Math.Acos(dot) * t;

        //    Vector3 delta = o - (s * dot);
        //    delta.Normalize();

        //    return (s * (float)Math.Cos(theta)) + (delta * (float)Math.Sin(theta));
        //}

        #endregion

        #region Instance Methods - Overrides

        public override bool Equals(object obj)
        {
            var v = obj as Vector3? ?? Vector3.NaN;

            return (v.X == X && v.Y == Y && v.Z == Z);
        }

        public IEnumerator<float> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override int GetHashCode()
        {
            return new { X, Y, Z }.GetHashCode();
        }

        public override string ToString()
        {
            return $"[ {X}, {Y}, {Z} ]";
        }

        public string ToString(int digits)
        {
            return $"[ {Math.Round(X, digits)}, {Math.Round(Y, digits)}, {Math.Round(Z, digits)} ]";
        }

        #endregion

        #endregion

        #region Static Methods

        #region Static Methods - Arithmatic

        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            Add(out var result, ref left, ref right);
            return result;
        }

        public static void Add(out Vector3 result, ref Vector3 left, ref Vector3 right)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
        }

        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            Subtract(out var result, ref left, ref right);
            return result;
        }

        public static void Subtract(out Vector3 result, ref Vector3 left, ref Vector3 right)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
        }

        public static Vector3 Multiply(Vector3 left, Vector3 right)
        {
            Multiply(out var result, ref left, ref right);
            return result;
        }

        public static void Multiply(out Vector3 result, ref Vector3 left, ref Vector3 right)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
        }

        public static Vector3 Multiply(Vector3 vector, float scalar)
        {
            Multiply(out var result, ref vector, ref scalar);
            return result;
        }

        public static Vector3 Multiply(float scalar, Vector3 vector)
        {
            Multiply(out var result, ref vector, ref scalar);
            return result;
        }

        public static void Multiply(out Vector3 result, ref Vector3 vector, ref float scalar)
        {
            Multiply(out result, ref scalar, ref vector);
        }

        public static void Multiply(out Vector3 result, ref float scalar, ref Vector3 vector)
        {
            result.X = vector.X * scalar;
            result.Y = vector.Y * scalar;
            result.Z = vector.Z * scalar;
        }

        public static Vector3 Divide(Vector3 left, Vector3 right)
        {
            Divide(out var result, ref left, ref right);
            return result;
        }

        public static void Divide(out Vector3 result, ref Vector3 left, ref Vector3 right)
        {
            result.X = left.X / right.X;
            result.Y = left.Y / right.Y;
            result.Z = left.Z / right.Z;
        }

        public static Vector3 Divide(Vector3 left, float right)
        {
            Divide(out var result, ref left, ref right);
            return result;
        }

        public static Vector3 Divide(float left, Vector3 right)
        {
            Divide(out var result, ref left, ref right);
            return result;
        }

        public static void Divide(out Vector3 result, ref Vector3 left, ref float right)
        {
            result.X = left.X / right;
            result.Y = left.Y / right;
            result.Z = left.Z / right;
        }

        public static void Divide(out Vector3 result, ref float left, ref Vector3 right)
        {
            result.X = left / right.X;
            result.Y = left / right.Y;
            result.Z = left / right.Z;
        }

        #endregion

        #region Static Methods - Mathematics

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Cross(out var result, ref left, ref right);
            return result;
        }

        public static void Cross(out Vector3 result, ref Vector3 left, ref Vector3 right)
        {
            var x = left.Y * right.Z - left.Z * right.Y;
            var y = left.Z * right.X - left.X * right.Z;
            var z = left.X * right.Y - left.Y * right.X;

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return Dot(ref left, ref right);
        }

        public static float Dot(ref Vector3 left, ref Vector3 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static float Distance(Vector3 left, Vector3 right)
        {
            return Distance(left, right);
        }

        public static float Distance(ref Vector3 left, ref Vector3 right)
        {
            return (float)Math.Sqrt(DistanceSquared(ref left, ref right));
        }

        public static float DistanceSquared(Vector3 left, Vector3 right)
        {
            return DistanceSquared(ref left, ref right);
        }

        public static float DistanceSquared(ref Vector3 left, ref Vector3 right)
        {
            float dx = right.X - left.X;
            float dy = right.Y - left.Y;
            float dz = right.Z - left.Z;

            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        public static Vector3 OrthroNormalize(Vector3 normal, Vector3 tangent)
        {
            OrthroNormalize(ref normal, ref tangent);
            return tangent;
        }

        public static void OrthroNormalize(Vector3 normal, ref Vector3 tangent)
        {
            OrthroNormalize(ref normal, ref tangent);
        }

        /// <summary>
        /// Gram-Schmidt orthonormalization
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        public static void OrthroNormalize(ref Vector3 normal, ref Vector3 tangent)
        {
            normal.Normalize();
            Vector3 projection = normal * Dot(tangent, normal);
            tangent = (tangent - projection).Normal();
        }

        #endregion

        #region Static Methods - Operators

        public static Vector3 operator +(Vector3 a)
        {
            return a;
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return Add(a, b);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return Subtract(a, b);
        }

        public static Vector3 operator *(float a, Vector3 b)
        {
            return Multiply(a, b) ;
        }

        public static Vector3 operator *(Vector3 b, float a)
        {
            return Multiply(a, b);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return Multiply(a, b);
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            return Divide(v, s);
        }

        public static Vector3 operator /(float s, Vector3 v)
        {
            return Divide(s, v);
        }

        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return Divide(left, right);
        }

        /// <summary>
        /// Cross product
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 operator %(Vector3 a, Vector3 b)
        {
            return Cross(a, b);
        }

        //public static Boolean3 operator >(Vector3 left, Vector3 right)
        //{
        //    return new Boolean3(left.X > right.X,
        //                        left.Y > right.Y,
        //                        left.Z > right.Z);
        //}

        //public static Boolean3 operator <(Vector3 left, Vector3 right)
        //{
        //    return new Boolean3(left.X < right.X,
        //                        left.Y < right.Y,
        //                        left.Z < right.Z);
        //}

        //public static Boolean3 operator >=(Vector3 left, Vector3 right)
        //{
        //    return new Boolean3(left.X >= right.X,
        //                        left.Y >= right.Y,
        //                        left.Z >= right.Z);
        //}

        //public static Boolean3 operator <=(Vector3 left, Vector3 right)
        //{
        //    return new Boolean3(left.X <= right.X,
        //                        left.Y <= right.Y,
        //                        left.Z <= right.Z);
        //}

        #endregion

        #region Static Methods - Conversion

        /// <summary>
        /// Parses a string representation of a 3-dimmensional vector.
        /// </summary>
        /// <param name="s">The string representation to parse.</param>
        public static Vector3 Parse(string s)
        {
            var pattern = @"^[\(\{\[\s]*([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\)\}\]\s]*$";
            var match = Regex.Match(s, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new ArgumentException($"Cannot parse {s} as 3-dimensional vector.");
            }

            var vector = new Vector3();
            vector.X = float.Parse(match.Groups[0].Value);
            vector.Y = float.Parse(match.Groups[1].Value);
            vector.Z = float.Parse(match.Groups[2].Value);

            return vector;
        }

        #endregion

        #endregion

        /// <summary>The zero vector.</summary>
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        
        /// <summary>The one vector.</summary>
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        /// <summary>The X unit vector.</summary>
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        
        /// <summary>The Y unit vector.</summary>
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        
        /// <summary>The Z unit vector.</summary>
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        /// <summary>The not-a-number vector.</summary>
        public static readonly Vector3 NaN = new Vector3(float.NaN, float.NaN, float.NaN);
        
        /// <summary>The negative infinity vector.</summary>
        public static readonly Vector3 NegativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
     
        /// <summary>The positive infinity vector.</summary>
        public static readonly Vector3 PositiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    }
}