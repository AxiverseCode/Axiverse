using System;
using System.Runtime.InteropServices;

namespace Axiverse
{
    /// <summary>
    /// Represents a 4 by 4 matrix.
    /// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4
    {
        #region Members

        /// <summary>Gets or sets the component in the 1st row and 1st column.</summary>
        public float M11;

        /// <summary>Gets or sets the component in the 1st row and 2nd column.</summary>
        public float M12;

        /// <summary>Gets or sets the component in the 1st row and 3rd column.</summary>
        public float M13;

        /// <summary>Gets or sets the component in the 1st row and 4rd column.</summary>
        public float M14;

        /// <summary>Gets or sets the component in the 2nd row and 1st column.</summary>
        public float M21;

        /// <summary>Gets or sets the component in the 2nd row and 2nd column.</summary>
        public float M22;

        /// <summary>Gets or sets the component in the 2nd row and 3rd column.</summary>
        public float M23;

        /// <summary>Gets or sets the component in the 2nd row and 4rd column.</summary>
        public float M24;

        /// <summary>Gets or sets the component in the 3rd row and 1st column.</summary>
        public float M31;

        /// <summary>Gets or sets the component in the 3rd row and 2nd column.</summary>
        public float M32;

        /// <summary>Gets or sets the component in the 3rd row and 3rd column.</summary>
        public float M33;

        /// <summary>Gets or sets the component in the 3rd row and 4rd column.</summary>
        public float M34;

        /// <summary>Gets or sets the component in the 4th row and 1st column.</summary>
        public float M41;

        /// <summary>Gets or sets the component in the 4th row and 2nd column.</summary>
        public float M42;

        /// <summary>Gets or sets the component in the 4th row and 3rd column.</summary>
        public float M43;

        /// <summary>Gets or sets the component in the 4th row and 4rd column.</summary>
        public float M44;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the component at the given index.
        /// </summary>
        /// <param name="i">The i index of the component.</param>
        /// <param name="j">The j index of the component.</param>
        /// <returns></returns>
        public float this[int i, int j]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        switch (j)
                        {
                            case 0: return M11;
                            case 1: return M12;
                            case 2: return M13;
                            case 3: return M14;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 1:
                        switch (j)
                        {
                            case 0: return M21;
                            case 1: return M22;
                            case 2: return M23;
                            case 3: return M24;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 2:
                        switch (j)
                        {
                            case 0: return M31;
                            case 1: return M32;
                            case 2: return M33;
                            case 3: return M34;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 3:
                        switch (j)
                        {
                            case 0: return M41;
                            case 1: return M42;
                            case 2: return M43;
                            case 3: return M44;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        switch (j)
                        {
                            case 0: M11 = value; return;
                            case 1: M12 = value; return;
                            case 2: M13 = value; return;
                            case 3: M14 = value; return;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 1:
                        switch (j)
                        {
                            case 0: M21 = value; return;
                            case 1: M22 = value; return;
                            case 2: M23 = value; return;
                            case 3: M24 = value; return;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 2:
                        switch (j)
                        {
                            case 0: M31 = value; return;
                            case 1: M32 = value; return;
                            case 2: M33 = value; return;
                            case 3: M34 = value; return;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    case 3:
                        switch (j)
                        {
                            case 0: M41 = value; return;
                            case 1: M42 = value; return;
                            case 2: M43 = value; return;
                            case 3: M44 = value; return;
                            default: throw new ArgumentOutOfRangeException();
                        }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a 4 by 4 matrix with all members are populated with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public Matrix4(float value)
        {
            M11 = M12 = M13 = M14 = value;
            M21 = M22 = M23 = M24 = value;
            M31 = M32 = M33 = M34 = value;
            M41 = M42 = M43 = M44 = value;
        }

        /// <summary>
        /// Constructs a 4 by 4 matrix with the specified diagonals.
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m22"></param>
        /// <param name="m33"></param>
        /// <param name="m44"></param>
        public Matrix4(float m11, float m22, float m33, float m44)
        {
            M11 = m11; M12 = 0; M13 = 0; M14 = 0;
            M21 = 0; M22 = m22; M23 = 0; M24 = 0;
            M31 = 0; M32 = 0; M33 = m33; M34 = 0;
            M41 = 0; M42 = 0; M43 = 0; M44 = m44;
        }

        /// <summary>
        /// Constructs a 4 by 4 matrix with the specified values.
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m14"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m24"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        /// <param name="m34"></param>
        /// <param name="m41"></param>
        /// <param name="m42"></param>
        /// <param name="m43"></param>
        /// <param name="m44"></param>
        public Matrix4(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24, 
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        #endregion

        #region Instance Methods

        #region Instance Methods - Setters & Getters

        public Vector4 Row(int i)
        {
            switch (i)
            {
                case 0: return new Vector4(M11, M12, M13, M14);
                case 1: return new Vector4(M21, M22, M23, M24);
                case 2: return new Vector4(M31, M32, M33, M34);
                case 3: return new Vector4(M41, M42, M43, M44);
            }
            throw new ArgumentOutOfRangeException();
        }

        public Vector4 Column(int j)
        {
            switch (j)
            {
                case 0: return new Vector4(M11, M21, M31, M41);
                case 1: return new Vector4(M12, M22, M32, M42);
                case 2: return new Vector4(M13, M23, M33, M43);
                case 3: return new Vector4(M14, M24, M34, M44);
            }
            throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region Instance Methods - Arithmatic

        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Add(Matrix4 matrix)
        {
            Add(out this, ref this, ref matrix);
        }

        /// <summary>
        /// Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Subtract(Matrix4 matrix)
        {
            Subtract(out this, ref this, ref matrix);
        }

        /// <summary>
        /// Multiplies this matrix with another matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Multiply(Matrix4 matrix)
        {
            Multiply(out this, ref this, ref matrix);
        }

        #endregion

        #region Instance Methods - Mathematic

        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        public void Invert()
        {
            this = Inverse();
        }

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        public void Transpose()
        {
            this = Transposition();
        }

        /// <summary>
        /// Computes the inverse of this matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix4 Inverse()
        {
            return Inverse(this);
        }

        /// <summary>
        /// Computes a transposition of this matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix4 Transposition()
        {
            return Transpose(this);
        }

        /// <summary>
        /// Computes the determinant of the matrix. The determinant is the factor at which the matrix expands or contracts space.
        /// </summary>
        /// <returns>The determinant of the matrix.</returns>
        public float Determinant()
        {
            float a = (M33 * M44) - (M34 * M43);
            float b = (M32 * M44) - (M34 * M42);
            float c = (M32 * M43) - (M33 * M42);
            float d = (M31 * M44) - (M34 * M41);
            float e = (M31 * M43) - (M33 * M41);
            float f = (M31 * M42) - (M32 * M41);

            return ((((M11 * (((M22 * a) - (M23 * b)) + (M24 * c))) - (M12 * (((M21 * a) -
                (M23 * d)) + (M24 * e)))) + (M13 * (((M21 * b) - (M22 * d)) + (M24 * f)))) -
                (M14 * (((M21 * c) - (M22 * e)) + (M23 * f))));
        }

        #endregion

        #region Instance Methods - Overrides

        public override string ToString()
        {
            return $"[ {M11} {M12}, {M13}, {M14} | {M21}, {M22}, {M23}, {M24} | {M31}, {M32}, {M33}, {M34} | {M41}, {M42}, {M43}, {M44} ]";
        }

        #endregion

        #endregion

        #region Static Methods

        #region Static Methods - Arithmatics

        public static Matrix4 Add(Matrix4 left, Matrix4 right)
        {
            Add(out var result, ref left, ref right);
            return result;
        }

        public static void Add(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;
        }

        public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        {
            Subtract(out var result, ref left, ref right);
            return result;
        }

        public static void Subtract(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;
        }

        public static Matrix4 Multiply(Matrix4 left, Matrix4 right)
        {
            Multiply(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Multiply(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        {
            result = new Matrix4
            {
                M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41),
                M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42),
                M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43),
                M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44),
                M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41),
                M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42),
                M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43),
                M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44),
                M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41),
                M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42),
                M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43),
                M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44),
                M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41),
                M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42),
                M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43),
                M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44)
            };
        }

        #endregion

        #region Static Methods - Mathematics

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix4 Transpose(Matrix4 matrix)
        {
            Transpose(out var result, ref matrix);
            return result;
        }

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="value"></param>
        public static void Transpose(out Matrix4 result, ref Matrix4 value)
        {
            result = new Matrix4
            {
                M11 = value.M11,
                M12 = value.M21,
                M13 = value.M31,
                M14 = value.M41,
                M21 = value.M12,
                M22 = value.M22,
                M23 = value.M32,
                M24 = value.M42,
                M31 = value.M13,
                M32 = value.M23,
                M33 = value.M33,
                M34 = value.M43,
                M41 = value.M14,
                M42 = value.M24,
                M43 = value.M34,
                M44 = value.M44
            };
        }

        /// <summary>
        /// Computes the inverse of the matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix4 Inverse(Matrix4 value)
        {
            Inverse(out var result, ref value);
            return result;
        }

        /// <summary>
        /// Computes the inverse of the matrix.
        /// </summary>
        /// <param name="result">The inverse of the matrix or the zero matrix if there is no inverse.</param>
        /// <param name="value"></param>
        public static bool Inverse(out Matrix4 result, ref Matrix4 value)
        {
            float b0 = (value.M31 * value.M42) - (value.M32 * value.M41);
            float b1 = (value.M31 * value.M43) - (value.M33 * value.M41);
            float b2 = (value.M34 * value.M41) - (value.M31 * value.M44);
            float b3 = (value.M32 * value.M43) - (value.M33 * value.M42);
            float b4 = (value.M34 * value.M42) - (value.M32 * value.M44);
            float b5 = (value.M33 * value.M44) - (value.M34 * value.M43);

            float d11 = value.M22 * b5 + value.M23 * b4 + value.M24 * b3;
            float d12 = value.M21 * b5 + value.M23 * b2 + value.M24 * b1;
            float d13 = value.M21 * -b4 + value.M22 * b2 + value.M24 * b0;
            float d14 = value.M21 * b3 + value.M22 * -b1 + value.M23 * b0;

            float determinant = value.M11 * d11 - value.M12 * d12 + value.M13 * d13 - value.M14 * d14;
            if (Math.Abs(determinant) == 0.0f)
            {
                result = Zero;
                return false;
            }

            determinant = 1f / determinant;

            float a0 = (value.M11 * value.M22) - (value.M12 * value.M21);
            float a1 = (value.M11 * value.M23) - (value.M13 * value.M21);
            float a2 = (value.M14 * value.M21) - (value.M11 * value.M24);
            float a3 = (value.M12 * value.M23) - (value.M13 * value.M22);
            float a4 = (value.M14 * value.M22) - (value.M12 * value.M24);
            float a5 = (value.M13 * value.M24) - (value.M14 * value.M23);

            float d21 = value.M12 * b5 + value.M13 * b4 + value.M14 * b3;
            float d22 = value.M11 * b5 + value.M13 * b2 + value.M14 * b1;
            float d23 = value.M11 * -b4 + value.M12 * b2 + value.M14 * b0;
            float d24 = value.M11 * b3 + value.M12 * -b1 + value.M13 * b0;

            float d31 = value.M42 * a5 + value.M43 * a4 + value.M44 * a3;
            float d32 = value.M41 * a5 + value.M43 * a2 + value.M44 * a1;
            float d33 = value.M41 * -a4 + value.M42 * a2 + value.M44 * a0;
            float d34 = value.M41 * a3 + value.M42 * -a1 + value.M43 * a0;

            float d41 = value.M32 * a5 + value.M33 * a4 + value.M34 * a3;
            float d42 = value.M31 * a5 + value.M33 * a2 + value.M34 * a1;
            float d43 = value.M31 * -a4 + value.M32 * a2 + value.M34 * a0;
            float d44 = value.M31 * a3 + value.M32 * -a1 + value.M33 * a0;

            result.M11 = +d11 * determinant; result.M12 = -d21 * determinant; result.M13 = +d31 * determinant; result.M14 = -d41 * determinant;
            result.M21 = -d12 * determinant; result.M22 = +d22 * determinant; result.M23 = -d32 * determinant; result.M24 = +d42 * determinant;
            result.M31 = +d13 * determinant; result.M32 = -d23 * determinant; result.M33 = +d33 * determinant; result.M34 = -d43 * determinant;
            result.M41 = -d14 * determinant; result.M42 = +d24 * determinant; result.M43 = -d34 * determinant; result.M44 = +d44 * determinant;

            return true;
        }

        public static Vector4 Transform(Vector4 vector, Matrix4 matrix)
        {
            Transform(out var result, ref vector, ref matrix);
            return result;
        }

        public static void Transform(out Vector4 result, ref Vector4 vector, ref Matrix4 matrix)
        {
            result = new Vector4(
                (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31) + (vector.W * matrix.M41),
                (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32) + (vector.W * matrix.M42),
                (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33) + (vector.W * matrix.M43),
                (vector.X * matrix.M14) + (vector.Y * matrix.M24) + (vector.Z * matrix.M34) + (vector.W * matrix.M44));
        }

        public static Vector4 Transform(Matrix4 matrix, Vector4 vector)
        {
            TransposedTransform(out var result, ref vector, ref matrix);
            return result;
        }

        public static void TransposedTransform(out Vector4 result, ref Vector4 vector, ref Matrix4 matrix)
        {
            result = new Vector4(
                (vector.X * matrix.M11) + (vector.Y * matrix.M12) + (vector.Z * matrix.M13) + (vector.W * matrix.M14),
                (vector.X * matrix.M21) + (vector.Y * matrix.M22) + (vector.Z * matrix.M23) + (vector.W * matrix.M24),
                (vector.X * matrix.M31) + (vector.Y * matrix.M32) + (vector.Z * matrix.M33) + (vector.W * matrix.M34),
                (vector.X * matrix.M41) + (vector.Y * matrix.M42) + (vector.Z * matrix.M43) + (vector.W * matrix.M44));
        }

        public static Matrix4 FromQuaternion(Quaternion quaternion)
        {
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;
            Matrix4 result = new Matrix4
            {
                M11 = 1f - (2f * (yy + zz)),
                M12 = 2f * (xy + zw),
                M13 = 2f * (zx - yw),
                M14 = 0,

                M21 = 2f * (xy - zw),
                M22 = 1f - (2f * (zz + xx)),
                M23 = 2f * (yz + xw),
                M24 = 0,

                M31 = 2f * (zx + yw),
                M32 = 2f * (yz - xw),
                M33 = 1f - (2f * (yy + xx)),
                M34 = 0,

                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return result;
        }

        #endregion

        #region Static Methods - Operators

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            Add(out var result, ref left, ref right);
            return result;
        }
        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            Subtract(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector4 operator *(Vector4 vector, Matrix4 matrix)
        {
            Transform(out var result, ref vector, ref matrix);
            return result;
        }

        /// <summary>
        /// Multiplies two matrices together.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            Multiply(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix4 operator ~(Matrix4 value)
        {
            return value.Transposition();
        }

        /// <summary>
        /// Inverts the matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix4 operator !(Matrix4 value)
        {
            return value.Inverse();
        }

        #endregion

        /// <summary>
        /// Created a right handed projection matrix.
        /// </summary>
        /// <param name="fov">Field of view in the y direction, in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="znear">Minimum z-value of the viewing volume.</param>
        /// <param name="zfar">Maximum z-value of the viewing volume.</param>
        public static Matrix4 PerspectiveFovRH(float fov, float aspect, float znear, float zfar)
        {
            float yScale = (float)(1.0f / Math.Tan(fov * 0.5f));
            float q = zfar / (znear - zfar);

            var result = new Matrix4
            {
                M11 = yScale / aspect,
                M22 = yScale,
                M33 = q,
                M34 = -1.0f,
                M43 = q * znear
            };
            return result;
        }

        /// <summary>
        /// Creates a right-handed, look-at matrix.
        /// </summary>
        /// <param name="eye">The position of the viewer's eye.</param>
        /// <param name="target">The camera look-at target.</param>
        /// <param name="up">The camera's up vector.</param>
        /// <param name="result">When the method completes, contains the created look-at matrix.</param>
        public static Matrix4 LookAtRH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 xaxis, yaxis, zaxis;
            Vector3.Subtract(out zaxis, ref eye, ref target); zaxis.Normalize();
            Vector3.Cross(out xaxis, ref up, ref zaxis); xaxis.Normalize();
            Vector3.Cross(out yaxis, ref zaxis, ref xaxis);

            var result = Matrix4.Identity;
            result.M11 = xaxis.X; result.M21 = xaxis.Y; result.M31 = xaxis.Z;
            result.M12 = yaxis.X; result.M22 = yaxis.Y; result.M32 = yaxis.Z;
            result.M13 = zaxis.X; result.M23 = zaxis.Y; result.M33 = zaxis.Z;

            result.M41 = Vector3.Dot(ref xaxis, ref eye);
            result.M42 = Vector3.Dot(ref yaxis, ref eye);
            result.M43 = Vector3.Dot(ref zaxis, ref eye);

            result.M41 = -result.M41;
            result.M42 = -result.M42;
            result.M43 = -result.M43;

            return result;
        }

        public static Matrix4 Scale(float x, float y, float z)
        {
            return new Matrix4(x, y, z, 1);
        }

        public static Matrix4 Translate(float x, float y, float z)
        {
            var result = Identity;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            return result;
        }
        #endregion

        /// <summary>The zero matrix.</summary>
        public static readonly Matrix4 Zero = new Matrix4();

        /// <summary>The identity matrix.</summary>
        public static readonly Matrix4 Identity = new Matrix4(1, 1, 1, 1);

        /// <summary>The not-a-number matrix.</summary>
        public static readonly Matrix4 NaN = new Matrix4(float.NaN);

        /// <summary>The negative infinity matrix.</summary>
        public static readonly Matrix4 NegativeInfinity = new Matrix4(float.NegativeInfinity);

        /// <summary>The positive infinity matrix.</summary>
        public static readonly Matrix4 PositiveInfinity = new Matrix4(float.PositiveInfinity);
    }
}