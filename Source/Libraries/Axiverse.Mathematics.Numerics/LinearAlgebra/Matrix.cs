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

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The number of columns of <paramref name="left"/> must be the same as the number of rows
        /// of <paramref name="right"/>.
        /// </remarks>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// The resulting matrix with the same number of rows as <paramref name="left"/> and the same
        /// number of columns as <paramref name="right"/>.
        /// </returns>
        public Matrix<T> Multiply(Matrix<T> left, Matrix<T> right)
        {
            Matrix<T> C = null;

            for (int i = 0; i < left.Rows; i++)
            {
                for (int k = 0; k < right.Columns; k++)
                {
                    for (int j = 0; j < left.Rows; j++)
                    {
                        C[i, j] = Operators.Add(C[i, j], Operators.Multiply(left[i, k], right[k, j]));
                    }
                }
            }

            return C;
        }
    }
}
