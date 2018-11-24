namespace Axiverse.Mathematics.LinearAlgebra
{
    /// <summary>
    /// Represents a matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Matrix<T>
    {
        /// <summary>
        /// Gets the width or number of columns int the matrix.
        /// </summary>
        public int Width => 0;

        /// <summary>
        /// Gets the height or number of rows in the matrix.
        /// </summary>
        public int Height => 0;

        /// <summary>
        /// Gets the height or number of rows in the matrix.
        /// </summary>
        public int Rows => Height;

        /// <summary>
        /// Gets the width or number of columns of the matrix.
        /// </summary>
        public int Columns => Width;

        /// <summary>
        /// Gets or sets an element
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public T this[int i, int j]
        {
            get => default(T);
            set => value = default(T);
        }

        public Matrix<T> Multiply(Matrix<T> A, Matrix<T> B)
        {
            Matrix<T> C = null;

            for (int i = 0; i < A.Rows; i++)
            {
                for (int k = 0; k < B.Columns; k++)
                {
                    for (int j = 0; j < A.Rows; j++)
                    {
                        C[i, j] = Operators.Add(C[i, j], Operators.Multiply(A[i, k], B[k, j]));
                    }
                }
            }

            return C;
        }
    }
}
