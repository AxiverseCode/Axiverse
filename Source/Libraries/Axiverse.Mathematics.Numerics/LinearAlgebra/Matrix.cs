using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.LinearAlgebra
{
    public class Matrix<T>
    {
        public int Width => 0;

        public int Height => 0;

        public int Rows => Height;
        public int Columns => Width;

        public T this[int i, int j]
        {
            get => default(T);
            set => value = default(T);
        }
    }
}
