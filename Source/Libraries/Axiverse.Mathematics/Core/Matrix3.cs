using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Represents a 3 by 3 matrix.
    /// </summary>
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3
    {
        #region Members

        /// <summary>Gets or sets the component in the 1st row and 1st column.</summary>
        public float M11;

        /// <summary>Gets or sets the component in the 1st row and 2nd column.</summary>
        public float M12;

        /// <summary>Gets or sets the component in the 1st row and 3rd column.</summary>
        public float M13;

        /// <summary>Gets or sets the component in the 2nd row and 1st column.</summary>
        public float M21;

        /// <summary>Gets or sets the component in the 2nd row and 2nd column.</summary>
        public float M22;

        /// <summary>Gets or sets the component in the 2nd row and 3rd column.</summary>
        public float M23;

        /// <summary>Gets or sets the component in the 3rd row and 1st column.</summary>
        public float M31;

        /// <summary>Gets or sets the component in the 3rd row and 2nd column.</summary>
        public float M32;

        /// <summary>Gets or sets the component in the 3rd row and 3rd column.</summary>
        public float M33;

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
        /// Constructs a 3 by 3 matrix with specified value assigned to all components.
        /// </summary>
        /// <param name="value"></param>
        public Matrix3(float value)
        {
            M11 = M12 = M13 = value;
            M21 = M22 = M23 = value;
            M31 = M32 = M33 = value;
        }

        /// <summary>
        /// Constructs a 3 by 3 matrix with the specified diagonals.
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m22"></param>
        /// <param name="m33"></param>
        public Matrix3(float m11, float m22, float m33)
        {
            M11 = m11; M12 = 0; M13 = 0;
            M21 = 0; M22 = m22; M23 = 0;
            M31 = 0; M32 = 0; M33 = m33;
        }

        /// <summary>
        /// Constructs a 3 by 3 matrix with the specified values.
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        public Matrix3(
            float m11 = 0f, float m12 = 0f, float m13 = 0f,
            float m21 = 0f, float m22 = 0f, float m23 = 0f,
            float m31 = 0f, float m32 = 0f, float m33 = 0f)
        {
            M11 = m11; M12 = m12; M13 = m13;
            M21 = m21; M22 = m22; M23 = m23;
            M31 = m31; M32 = m32; M33 = m33;
        }

        #endregion

        #region Instance Methods

        #region Instance Methods - Setters & Getters

        /// <summary>
        /// Gets the vector representation of the specified row.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vector3 Row(int i)
        {
            switch (i)
            {
                case 0: return new Vector3(M11, M12, M13);
                case 1: return new Vector3(M21, M22, M23);
                case 2: return new Vector3(M31, M32, M33);
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Gets the vector representation of the specified column.
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public Vector3 Column(int j)
        {
            switch (j)
            {
                case 0: return new Vector3(M11, M21, M31);
                case 1: return new Vector3(M12, M22, M32);
                case 2: return new Vector3(M13, M23, M33);
            }
            throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region Instance Methods - Arithmatic
        
        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Add(Matrix3 matrix)
        {
            Add(out this, ref this, ref matrix);
        }

        /// <summary>
        /// Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Subtract(Matrix3 matrix)
        {
            Subtract(out this, ref this, ref matrix);
        }

        /// <summary>
        /// Multiplies this matrix with another matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public void Multiply(Matrix3 matrix)
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
        public Matrix3 Inverse()
        {
            return Inverse(this);
        }

        /// <summary>
        /// Computes a transposition of this matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix3 Transposition()
        {
            return Transpose(this);
        }

        /// <summary>
        /// Computes the determinant of the matrix. The determinant is the factor at which the matrix expands or contracts space.
        /// </summary>
        /// <returns>The determinant of the matrix.</returns>
        public float Determinant()
        {
            return M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 -
                   M31 * M22 * M13 - M32 * M23 * M11 - M33 * M21 * M12;
        }

        #endregion

        #region Instance Methods - Overrides

        /// <summary>
        /// Gets a string representation of the matrix.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[ {M11} {M12}, {M13} | {M21}, {M22}, {M23} | {M31}, {M32}, {M33} ]";
        }

        #endregion

        #endregion

        #region Static Methods

        #region Static Methods - Arithmatics

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix3 Add(Matrix3 left, Matrix3 right)
        {
            Add(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Add(out Matrix3 result, ref Matrix3 left, ref Matrix3 right)
        {
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix3 Subtract(Matrix3 left, Matrix3 right)
        {
            Subtract(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Substracts two matrices.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Subtract(out Matrix3 result, ref Matrix3 left, ref Matrix3 right)
        {
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix3 Multiply(Matrix3 left, Matrix3 right)
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
        public static void Multiply(out Matrix3 result, ref Matrix3 left, ref Matrix3 right)
        {
            float m11 = ((left.M11 * right.M11) + (left.M12 * right.M21)) + (left.M13 * right.M31);
            float m12 = ((left.M11 * right.M12) + (left.M12 * right.M22)) + (left.M13 * right.M32);
            float m13 = ((left.M11 * right.M13) + (left.M12 * right.M23)) + (left.M13 * right.M33);
            float m21 = ((left.M21 * right.M11) + (left.M22 * right.M21)) + (left.M23 * right.M31);
            float m22 = ((left.M21 * right.M12) + (left.M22 * right.M22)) + (left.M23 * right.M32);
            float m23 = ((left.M21 * right.M13) + (left.M22 * right.M23)) + (left.M23 * right.M33);
            float m31 = ((left.M31 * right.M11) + (left.M32 * right.M21)) + (left.M33 * right.M31);
            float m32 = ((left.M31 * right.M12) + (left.M32 * right.M22)) + (left.M33 * right.M32);
            float m33 = ((left.M31 * right.M13) + (left.M32 * right.M23)) + (left.M33 * right.M33);

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        #endregion

        #region Static Methods - Mathematics

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix3 Transpose(Matrix3 matrix)
        {
            Transpose(out var result, ref matrix);
            return result;
        }

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="matrix"></param>
        public static void Transpose(out Matrix3 result, ref Matrix3 matrix)
        {
            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
        }

        /// <summary>
        /// Computes the inverse of a matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix3 Inverse(Matrix3 value)
        {
            Inverse(out var result, ref value);
            return result;
        }

        /// <summary>
        /// Computes the inverse of a matrix.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="matrix"></param>
        public static void Inverse(out Matrix3 result, ref Matrix3 matrix)
        {
            float det = matrix.M11 * matrix.M22 * matrix.M33 -
                matrix.M11 * matrix.M23 * matrix.M32 -
                matrix.M12 * matrix.M21 * matrix.M33 +
                matrix.M12 * matrix.M23 * matrix.M31 +
                matrix.M13 * matrix.M21 * matrix.M32 -
                matrix.M13 * matrix.M22 * matrix.M31;

            float num11 = matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32;
            float num12 = matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33;
            float num13 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;

            float num21 = matrix.M23 * matrix.M31 - matrix.M33 * matrix.M21;
            float num22 = matrix.M11 * matrix.M33 - matrix.M31 * matrix.M13;
            float num23 = matrix.M13 * matrix.M21 - matrix.M23 * matrix.M11;

            float num31 = matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22;
            float num32 = matrix.M12 * matrix.M31 - matrix.M32 * matrix.M11;
            float num33 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;

            result.M11 = num11 / det;
            result.M12 = num12 / det;
            result.M13 = num13 / det;
            result.M21 = num21 / det;
            result.M22 = num22 / det;
            result.M23 = num23 / det;
            result.M31 = num31 / det;
            result.M32 = num32 / det;
            result.M33 = num33 / det;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector3 Transform(Vector3 vector, Matrix3 matrix)
        {
            Transform(out var result, ref vector, ref matrix);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        public static void Transform(out Vector3 result, ref Vector3 vector, ref Matrix3 matrix)
        {
            float x = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31);
            float y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32);
            float z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33);

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 Transform(Matrix3 matrix, Vector3 vector)
        {
            TransposedTransform(out var result, ref vector, ref matrix);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the transpose of a matrix.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        public static void TransposedTransform(out Vector3 result, ref Vector3 vector, ref Matrix3 matrix)
        {
            float x = (vector.X * matrix.M11) + (vector.Y * matrix.M12) + (vector.Z * matrix.M13);
            float y = (vector.X * matrix.M21) + (vector.Y * matrix.M22) + (vector.Z * matrix.M23);
            float z = (vector.X * matrix.M31) + (vector.Y * matrix.M32) + (vector.Z * matrix.M33);

            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Creates a three dimensional rotation matrix from a quaternion.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Matrix3 FromQuaternion(Quaternion quaternion)
        {
            Matrix3 result;
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
            result.M21 = 2f * (xy - zw);
            result.M22 = 1f - (2f * (zz + xx));
            result.M23 = 2f * (yz + xw);
            result.M31 = 2f * (zx + yw);
            result.M32 = 2f * (yz - xw);
            result.M33 = 1f - (2f * (yy + xx));
            return result;
        }

        #endregion

        #region Static Methods - Operators

        /// <summary>
        /// Transforms a vector by a matrix.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 vector, Matrix3 matrix)
        {
            Transform(out var result, ref vector, ref matrix);
            return result;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            Multiply(out var result, ref left, ref right);
            return result;
        }

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix3 operator ~(Matrix3 value)
        {
            return value.Transposition();
        }

        /// <summary>
        /// Computes the inverse of a matrix.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Matrix3 operator !(Matrix3 value)
        {
            return value.Inverse();
        }

        #endregion

        #endregion

        /// <summary>The zero matrix.</summary>
        public static readonly Matrix3 Zero = new Matrix3();
      
        /// <summary>The identity matrix.</summary>
        public static readonly Matrix3 Identity = new Matrix3(1, 1, 1);

        /// <summary>The not-a-number matrix.</summary>
        public static readonly Matrix3 NaN = new Matrix3(float.NaN);
        
        /// <summary>The negative infinity matrix.</summary>
        public static readonly Matrix3 NegativeInfinity = new Matrix3(float.NegativeInfinity);
      
        /// <summary>The positive infinity matrix.</summary>
        public static readonly Matrix3 PositiveInfinity = new Matrix3(float.PositiveInfinity);
    }
}