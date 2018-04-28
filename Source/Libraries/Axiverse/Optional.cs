using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// An optional value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Optional<T> where T : class
    {
        /// <summary>
        /// Gets the optional value is present or null if not.
        /// </summary>
        public T NullableValue { get; }

        /// <summary>
        /// Gets the present value.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when the value is not defined.</exception>
        public T Value => NullableValue ?? throw new NullReferenceException();

        /// <summary>
        /// Gets whether a value is present.
        /// </summary>
        public bool IsPresent => NullableValue != null;

        /// <summary>
        /// Constructs an Optional with the defined value.
        /// </summary>
        /// <param name="value">The defined non-null value.</param>
        /// <exception cref="NullReferenceException">Thrown if the value is null.</exception>
        [Pure]
        public Optional(T value)
        {
            Contract.Requires<NullReferenceException>(value != null);
            NullableValue = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Optional<T> optional)
            {
                return optional.NullableValue == NullableValue;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return NullableValue.GetHashCode();
        }

        public override string ToString()
        {
            return NullableValue != null ? NullableValue.ToString() : "Absent";
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static implicit operator T(Optional<T> optional)
        {
            return optional.Value;
        }

        public static readonly Optional<T> Absent = new Optional<T>();
    }
}
