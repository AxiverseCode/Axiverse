using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Axiverse
{
    /// <summary>
    /// Represents a 3-dimensional Cartesian vector
    /// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4
    {
        #region Members

        /// <summary>Gets or sets the X component.</summary>
        public float X;

        /// <summary>Gets or sets the Y component.</summary>
        public float Y;

        /// <summary>Gets or sets the Z component.</summary>
        public float Z;

        /// <summary>Gets or sets the W component.s</summary>
        public float W;

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
                    case 3: return W;
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
                    case 3: W = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a quaternion from the given values.
        /// </summary>
        /// <param name="x">The value to set the X component.</param>
        /// <param name="y">The value to set the Y component.</param>
        /// <param name="z">The value to set the Z component.</param>
        /// <param name="w">The value to set the W component.</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Constructs a quaternion from the given values and a default W value of 0.
        /// </summary>
        /// <param name="x">The value to set the X component.</param>
        /// <param name="y">The value to set the Y component.</param>
        /// <param name="z">The value to set the Z component.</param>
        public Vector4(float x, float y, float z) : this(x, y, z, 0)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector4(float x, float y) : this(x, y, 0, 0) { }

        public Vector4(Vector4 v) : this(v.X, v.Y, v.Z, v.W) { }

        public Vector4(Vector3 v) : this(v.X, v.Y, v.Z) { }

        public Vector4(Vector2 v) : this(v.X, v.Y){ }

        public Vector4(Vector3 v, float w) : this(v.X, v.Y, v.Z, w) { }

        public Vector4(Vector2 u, Vector2 v) : this(u.X, u.Y, v.X, v.Y) {}

        public Vector4(Vector2 u, float z, float w) : this(u.X, u.Y, z, w){}

        public Vector4(Quaternion q) : this(q.X, q.Y, q.Z, q.W) { }

        #endregion

        #region Instance Methods
        
        #region Instance Methods - Setters

        /// <summary>
        /// Sets the components of the vector to the given values.
        /// </summary>
        /// <param name="x">The value to set the X component.</param>
        /// <param name="y">The value to set the Y component.</param>
        /// <param name="z">The value to set the Z component.</param>
        /// <param name="w">The value to set the W component.</param>
        /// <returns></returns>
        public Vector4 Set(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            return this;
        }

        #endregion

        #region Instance Methods - Arithmatic

        public Vector4 Add(Vector4 v)
        {
            X += v.X;
            Y += v.Y;
            Z += v.Z;
            W += v.W;
            return this;
        }

        public Vector4 Add(float s)
        {
            X += s;
            Y += s;
            Z += s;
            W += s;
            return this;
        }

        public Vector4 Subtract(Vector4 v)
        {
            X -= v.X;
            Y -= v.Y;
            Z -= v.Z;
            W -= v.W;
            return this;
        }

        public Vector4 Subtract(float s)
        {
            X -= s;
            Y -= s;
            Z -= s;
            W -= s;
            return this;
        }

        public Vector4 Multiply(Vector4 v)
        {
            X *= v.X;
            Y *= v.Y;
            Z *= v.Z;
            W *= v.W;
            return this;
        }

        public Vector4 Multiply(float s)
        {
            X *= s;
            Y *= s;
            Z *= s;
            W *= s;
            return this;
        }

        public Vector4 Divide(Vector4 v)
        {
            X /= v.X;
            Y /= v.Y;
            Z /= v.Z;
            W /= v.W;
            return this;
        }

        public Vector4 Divide(float s)
        {
            X /= s;
            Y /= s;
            Z /= s;
            W /= s;
            return this;
        }

        public Vector4 Negate()
        {
            return Multiply(-1);
        }

        #endregion

        #region Instance Methods - Mathematics

        public float Dot(Vector4 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z + W * v.W;
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float ManhattanLength()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z) + Math.Abs(W);
        }

        public Vector4 Normalize()
        {
            return Divide(this.Length());
        }

        public Vector4 SetLength(float s)
        {
            return Multiply(s / Length());
        }

        public float DistanceTo(Vector4 v)
        {
            return (float)Math.Sqrt(DistanceToSquared(v));
        }

        public float DistanceToSquared(Vector4 v)
        {
            float dx = v.X - X;
            float dy = v.Y - Y;
            float dz = v.Z - Z;
            float dw = v.W - W;

            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        public bool IsZero()
        {
            return (LengthSquared() < 0.0001);
        }

        #endregion

        #region Instance Methods - Overrides
        
        public override bool Equals(object obj)
        {
            var v = obj as Vector4? ?? Vector4.NaN;

            return (v.X == X && v.Y == Y && v.Z == Z && v.W == W);
        }

        public override int GetHashCode()
        {
            return new { X, Y, Z, W }.GetHashCode();
        }

        public override string ToString()
        {
            return $"[ {X}, {Y}, {Z}, {W} ]";
        }

        #endregion

        #endregion

        #region Static Methods

        #region Static Methods - Arithmatic

        public static Vector4 Add(Vector4 left, Vector4 right)
        {
            Add(out var result, ref left, ref right);
            return result;
        }

        public static void Add(out Vector4 result, ref Vector4 left, ref Vector4 right)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        public static Vector4 Subtract(Vector4 left, Vector4 right)
        {
            Subtract(out var result, ref left, ref right);
            return result;
        }

        public static void Subtract(out Vector4 result, ref Vector4 left, ref Vector4 right)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        public static Vector4 Multiply(Vector4 left, Vector4 right)
        {
            Multiply(out var result, ref left, ref right);
            return result;
        }

        public static void Multiply(out Vector4 result, ref Vector4 left, ref Vector4 right)
        {
            result.X = left.X * right.X;
            result.Y = left.Y * right.Y;
            result.Z = left.Z * right.Z;
            result.W = left.W * right.W;
        }

        #endregion

        #region Static Methods - Operators

        public static Vector4 operator +(Vector4 a)
        {
            return a;
        }

        public static Vector4 operator -(Vector4 a)
        {
            return new Vector4(-a.X, -a.Y, -a.Z);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return Add(a, b);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return Subtract(a, b);
        }

        //public static Vector4 operator *(float a, Vector4 b)
        //{
        //    return Multiply(a, b);
        //}

        //public static Vector4 operator *(Vector4 b, float a)
        //{
        //    return Multiply(a, b);
        //}

        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return Multiply(a, b);
        }

        //public static Vector4 operator /(Vector4 v, float s)
        //{
        //    return Divide(v, s);
        //}

        //public static Vector4 operator /(float s, Vector4 v)
        //{
        //    return Divide(s, v);
        //}

        //public static Vector4 operator /(Vector4 left, Vector4 right)
        //{
        //    return Divide(left, right);
        //}

        #endregion

        #region Static Methods - Conversion

        /// <summary>
        /// Parses a string representation of a 4 dimensional vector.
        /// </summary>
        /// <param name="s">The string representation to parse.</param>
        public static Vector4 Parse(string s)
        {
            var pattern = @"^[\(\{\[\s]*([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\s,]+([0-9\.Ee+-]+)[\)\}\]\s]*$";
            var match = Regex.Match(s, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new ArgumentException($"Cannot parse {s} as 4-dimensional vector.");
            }
            var value = new Vector4();
            value.X = float.Parse(match.Groups[0].Value);
            value.Y = float.Parse(match.Groups[1].Value);
            value.Z = float.Parse(match.Groups[2].Value);
            value.W = float.Parse(match.Groups[3].Value);

            return value;
        }

        #endregion

        #endregion

        /// <summary>The zero vector.</summary>
        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
      
        /// <summary>The one vector.</summary>
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

        /// <summary>The X unit vector.</summary>
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
       
        /// <summary>The Y unit vector.</summary>
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
      
        /// <summary>The Z unit vector.</summary>
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
       
        /// <summary>The W unit vector.</summary>
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

        /// <summary>The not-a-number vector.</summary>
        public static readonly Vector4 NaN = new Vector4(float.NaN, float.NaN, float.NaN, float.NaN);
      
        /// <summary>The negative infinity vector.</summary>
        public static readonly Vector4 NegativeInfinity = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
      
        /// <summary>The positive infinity vector.</summary>
        public static readonly Vector4 PositiveInfinity = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    }
}