using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.LinearAlgebra
{
    public class Vector<T>
        where T : IEquatable<T>, IFormattable
    {
        public int Length => 0;

        public T this[int i, int j]
        {
            get => default(T);
            set => value = default(T);
        }


    }
}
