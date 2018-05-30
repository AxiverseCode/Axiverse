using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
                        }
                        throw new ArgumentOutOfRangeException();
                    case 1:
                        switch (j)
                        {
                            case 0: return M21;
                            case 1: return M22;
                            case 2: return M23;
                        }
                        throw new ArgumentOutOfRangeException();
                    case 2:
                        switch (j)
                        {
                            case 0: return M31;
                            case 1: return M32;
                            case 2: return M33;
                        }
                        throw new ArgumentOutOfRangeException();
                }
                throw new ArgumentOutOfRangeException();
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
                        }
                        throw new ArgumentOutOfRangeException();
                    case 1:
                        switch (j)
                        {
                            case 0: M21 = value; return;
                            case 1: M22 = value; return;
                            case 2: M23 = value; return;
                        }
                        throw new ArgumentOutOfRangeException();
                    case 2:
                        switch (j)
                        {
                            case 0: M31 = value; return;
                            case 1: M32 = value; return;
                            case 2: M33 = value; return;
                        }
                        throw new ArgumentOutOfRangeException();
                }
                throw new ArgumentOutOfRangeException();
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

        //#region Instance Methods - Setters & Getters

        //public Vector3 Row(int i)
        //{
        //    switch (i)
        //    {
        //        case 0: return new Vector3(M11, M12, M13);
        //        case 1: return new Vector3(M21, M22, M23);
        //        case 2: return new Vector3(M31, M32, M33);
        //    }
        //    throw new ArgumentOutOfRangeException();
        //}

        //public Vector3 Column(int j)
        //{
        //    switch (j)
        //    {
        //        case 0: return new Vector3(M11, M21, M31);
        //        case 1: return new Vector3(M12, M22, M32);
        //        case 2: return new Vector3(M13, M23, M33);
        //    }
        //    throw new ArgumentOutOfRangeException();
        //}

        //#endregion

        //#region Instance Methods - Arithmatic

        ///// <summary>
        ///// Adds another matrix to this matrix.
        ///// </summary>
        ///// <param name="matrix"></param>
        //public void Add(Matrix4 matrix)
        //{
        //    Add(out this, ref this, ref matrix);
        //}

        ///// <summary>
        ///// Subtracts another matrix from this matrix.
        ///// </summary>
        ///// <param name="matrix"></param>
        //public void Subtract(Matrix4 matrix)
        //{
        //    Subtract(out this, ref this, ref matrix);
        //}

        ///// <summary>
        ///// Multiplies this matrix with another matrix.
        ///// </summary>
        ///// <param name="matrix"></param>
        //public void Multiply(Matrix4 matrix)
        //{
        //    Multiply(out this, ref this, ref matrix);
        //}

        //#endregion

        //#region Instance Methods - Mathematic

        ///// <summary>
        ///// Inverts the matrix.
        ///// </summary>
        //public void Invert()
        //{
        //    this = Inverse();
        //}

        ///// <summary>
        ///// Transposes the matrix.
        ///// </summary>
        //public void Transpose()
        //{
        //    this = Transposition();
        //}

        ///// <summary>
        ///// Computes the inverse of this matrix.
        ///// </summary>
        ///// <returns></returns>
        //public Matrix4 Inverse()
        //{
        //    return Inverse(this);
        //}

        ///// <summary>
        ///// Computes a transposition of this matrix.
        ///// </summary>
        ///// <returns></returns>
        //public Matrix4 Transposition()
        //{
        //    return Transpose(this);
        //}

        ///// <summary>
        ///// Computes the determinant of the matrix. The determinant is the factor at which the matrix expands or contracts space.
        ///// </summary>
        ///// <returns>The determinant of the matrix.</returns>
        //public float Determinant()
        //{
        //    return M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 -
        //           M31 * M22 * M13 - M32 * M23 * M11 - M33 * M21 * M12;
        //}

        //#endregion

        //#region Instance Methods - Overrides

        //public override string ToString()
        //{
        //    return $"[ {M11} {M12}, {M13} | {M21}, {M22}, {M23} | {M31}, {M32}, {M33} ]";
        //}

        //#endregion

        //#endregion

        //#region Static Methods

        //#region Static Methods - Arithmatics

        //public static Matrix4 Add(Matrix4 left, Matrix4 right)
        //{
        //    Add(out var result, ref left, ref right);
        //    return result;
        //}

        //public static void Add(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        //{
        //    result.M11 = left.M11 + right.M11;
        //    result.M12 = left.M12 + right.M12;
        //    result.M13 = left.M13 + right.M13;
        //    result.M21 = left.M21 + right.M21;
        //    result.M22 = left.M22 + right.M22;
        //    result.M23 = left.M23 + right.M23;
        //    result.M31 = left.M31 + right.M31;
        //    result.M32 = left.M32 + right.M32;
        //    result.M33 = left.M33 + right.M33;
        //}

        //public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        //{
        //    Subtract(out var result, ref left, ref right);
        //    return result;
        //}

        //public static void Subtract(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        //{
        //    result.M11 = left.M11 - right.M11;
        //    result.M12 = left.M12 - right.M12;
        //    result.M13 = left.M13 - right.M13;
        //    result.M21 = left.M21 - right.M21;
        //    result.M22 = left.M22 - right.M22;
        //    result.M23 = left.M23 - right.M23;
        //    result.M31 = left.M31 - right.M31;
        //    result.M32 = left.M32 - right.M32;
        //    result.M33 = left.M33 - right.M33;
        //}

        //public static Matrix4 Multiply(Matrix4 left, Matrix4 right)
        //{
        //    Multiply(out var result, ref left, ref right);
        //    return result;
        //}

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Multiply(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        {
            Matrix4 value = new Matrix4();
            value.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
            value.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
            value.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
            value.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
            value.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
            value.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
            value.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
            value.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
            value.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
            value.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
            value.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
            value.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
            value.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
            value.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
            value.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
            value.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
            result = value;
        }
        //public static void Multiply(out Matrix4 result, ref Matrix4 left, ref Matrix4 right)
        //{
        //    float m11 = ((left.M11 * right.M11) + (left.M12 * right.M21)) + (left.M13 * right.M31);
        //    float m12 = ((left.M11 * right.M12) + (left.M12 * right.M22)) + (left.M13 * right.M32);
        //    float m13 = ((left.M11 * right.M13) + (left.M12 * right.M23)) + (left.M13 * right.M33);
        //    float m21 = ((left.M21 * right.M11) + (left.M22 * right.M21)) + (left.M23 * right.M31);
        //    float m22 = ((left.M21 * right.M12) + (left.M22 * right.M22)) + (left.M23 * right.M32);
        //    float m23 = ((left.M21 * right.M13) + (left.M22 * right.M23)) + (left.M23 * right.M33);
        //    float m31 = ((left.M31 * right.M11) + (left.M32 * right.M21)) + (left.M33 * right.M31);
        //    float m32 = ((left.M31 * right.M12) + (left.M32 * right.M22)) + (left.M33 * right.M32);
        //    float m33 = ((left.M31 * right.M13) + (left.M32 * right.M23)) + (left.M33 * right.M33);

        //    result.M11 = m11;
        //    result.M12 = m12;
        //    result.M13 = m13;
        //    result.M21 = m21;
        //    result.M22 = m22;
        //    result.M23 = m23;
        //    result.M31 = m31;
        //    result.M32 = m32;
        //    result.M33 = m33;
        //}

        //#endregion

        //#region Static Methods - Mathematics

        //public static Matrix4 Transpose(Matrix4 matrix)
        //{
        //    Transpose(out var result, ref matrix);
        //    return result;
        //}

        //public static void Transpose(out Matrix4 result, ref Matrix4 matrix)
        //{
        //    result.M11 = matrix.M11;
        //    result.M12 = matrix.M21;
        //    result.M13 = matrix.M31;
        //    result.M21 = matrix.M12;
        //    result.M22 = matrix.M22;
        //    result.M23 = matrix.M32;
        //    result.M31 = matrix.M13;
        //    result.M32 = matrix.M23;
        //    result.M33 = matrix.M33;
        //}

        //public static Matrix4 Inverse(Matrix4 value)
        //{
        //    Inverse(out var result, ref value);
        //    return result;
        //}

        //public static void Inverse(out Matrix4 result, ref Matrix4 matrix)
        //{
        //    float det = matrix.M11 * matrix.M22 * matrix.M33 -
        //        matrix.M11 * matrix.M23 * matrix.M32 -
        //        matrix.M12 * matrix.M21 * matrix.M33 +
        //        matrix.M12 * matrix.M23 * matrix.M31 +
        //        matrix.M13 * matrix.M21 * matrix.M32 -
        //        matrix.M13 * matrix.M22 * matrix.M31;

        //    float num11 = matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32;
        //    float num12 = matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33;
        //    float num13 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;

        //    float num21 = matrix.M23 * matrix.M31 - matrix.M33 * matrix.M21;
        //    float num22 = matrix.M11 * matrix.M33 - matrix.M31 * matrix.M13;
        //    float num23 = matrix.M13 * matrix.M21 - matrix.M23 * matrix.M11;

        //    float num31 = matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22;
        //    float num32 = matrix.M12 * matrix.M31 - matrix.M32 * matrix.M11;
        //    float num33 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;

        //    result.M11 = num11 / det;
        //    result.M12 = num12 / det;
        //    result.M13 = num13 / det;
        //    result.M21 = num21 / det;
        //    result.M22 = num22 / det;
        //    result.M23 = num23 / det;
        //    result.M31 = num31 / det;
        //    result.M32 = num32 / det;
        //    result.M33 = num33 / det;
        //}

        //public static Vector3 Transform(Vector3 vector, Matrix4 matrix)
        //{
        //    Transform(out var result, ref vector, ref matrix);
        //    return result;
        //}

        //public static void Transform(out Vector3 result, ref Vector3 vector, ref Matrix4 matrix)
        //{
        //    float x = ((vector.X * matrix.M11) + (vector.Y * matrix.M21)) + (vector.Z * matrix.M31);
        //    float y = ((vector.X * matrix.M12) + (vector.Y * matrix.M22)) + (vector.Z * matrix.M32);
        //    float z = ((vector.X * matrix.M13) + (vector.Y * matrix.M23)) + (vector.Z * matrix.M33);

        //    result.X = x;
        //    result.Y = y;
        //    result.Z = z;
        //}

        //public static Vector3 Transform(Matrix4 matrix, Vector3 vector)
        //{
        //    TransposedTransform(out var result, ref vector, ref matrix);
        //    return result;
        //}

        //public static void TransposedTransform(out Vector3 result, ref Vector3 vector, ref Matrix4 matrix)
        //{
        //    float x = ((vector.X * matrix.M11) + (vector.Y * matrix.M12)) + (vector.Z * matrix.M13);
        //    float y = ((vector.X * matrix.M21) + (vector.Y * matrix.M22)) + (vector.Z * matrix.M23);
        //    float z = ((vector.X * matrix.M31) + (vector.Y * matrix.M32)) + (vector.Z * matrix.M33);

        //    result.X = x;
        //    result.Y = y;
        //    result.Z = z;
        //}

        public static Matrix4 FromQuaternion(Quaternion quaternion)
        {
            Matrix4 result;
            float xx = quaternion.X * quaternion.X;
            float yy = quaternion.Y * quaternion.Y;
            float zz = quaternion.Z * quaternion.Z;
            float xy = quaternion.X * quaternion.Y;
            float zw = quaternion.Z * quaternion.W;
            float zx = quaternion.Z * quaternion.X;
            float yw = quaternion.Y * quaternion.W;
            float yz = quaternion.Y * quaternion.Z;
            float xw = quaternion.X * quaternion.W;
            result.M11 = 1f - (2f * (yy + zz));
            result.M12 = 2f * (xy + zw);
            result.M13 = 2f * (zx - yw);
            result.M14 = 0;
            result.M21 = 2f * (xy - zw);
            result.M22 = 1f - (2f * (zz + xx));
            result.M23 = 2f * (yz + xw);
            result.M24 = 0;
            result.M31 = 2f * (zx + yw);
            result.M32 = 2f * (yz - xw);
            result.M33 = 1f - (2f * (yy + xx));
            result.M34 = 0;
            result.M41 = result.M42 = result.M43 = 0;
            result.M44 = 1;
            return result;
        }

        //#endregion

        //#region Static Methods - Operators

        //public static Vector3 operator *(Vector3 vector, Matrix4 matrix)
        //{
        //    Transform(out var result, ref vector, ref matrix);
        //    return result;
        //}

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

        //public static Matrix4 operator ~(Matrix4 value)
        //{
        //    return value.Transposition();
        //}

        //public static Matrix4 operator !(Matrix4 value)
        //{
        //    return value.Inverse();
        //}

        //#endregion

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

            var result = new Matrix4();
            result.M11 = yScale / aspect;
            result.M22 = yScale;
            result.M33 = q;
            result.M34 = -1.0f;
            result.M43 = q * znear;
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

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix4 Transpose(Matrix4 value)
        {
            Matrix4 result = new Matrix4();
            result.M11 = value.M11;
            result.M12 = value.M21;
            result.M13 = value.M31;
            result.M14 = value.M41;
            result.M21 = value.M12;
            result.M22 = value.M22;
            result.M23 = value.M32;
            result.M24 = value.M42;
            result.M31 = value.M13;
            result.M32 = value.M23;
            result.M33 = value.M33;
            result.M34 = value.M43;
            result.M41 = value.M14;
            result.M42 = value.M24;
            result.M43 = value.M34;
            result.M44 = value.M44;

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