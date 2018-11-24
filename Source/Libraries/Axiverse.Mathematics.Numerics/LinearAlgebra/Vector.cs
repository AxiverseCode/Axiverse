using System;

namespace Axiverse.Mathematics.LinearAlgebra
{
    /// <summary>
    /// Represents a vector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
