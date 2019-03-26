using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Axiverse.Injection
{
    /// <summary>
    /// Key representing a specific binding of a type. This key must be manually bound to injectors
    /// by reference. Only reference represents the binding, the <see cref="Name"/> property is
    /// only for convenience, not binding.
    /// </summary>
    public class SyntheticKey : Key
    {
        /// <summary>
        /// Gets or sets an optional display name. This field is not used for binding equality. The
        /// specific instance of the key must be used for binding.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the identifier for this <see cref="SyntheticKey"/> which is unique in the
        /// <see cref="AppContext"/>.
        /// </summary>
        public int Identifier { get; }

        /// <summary>
        /// Constructs a <see cref="SyntheticKey"/>.
        /// </summary>
        /// <param name="type"></param>
        protected internal SyntheticKey(Type type) : base(type)
        {
            Identifier = Interlocked.Increment(ref identifier);
        }

        /// <summary>
        /// Constructs a <see cref="SyntheticKey"/> with the given display name. Different unique
        /// <see cref="SyntheticKey"/>s can have the same name. It has no effect on injection and
        /// is used purely for developmental purposes.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        protected internal SyntheticKey(Type type, string name) : this(type)
        {
            Name = name;
        }

        /// <summary>
        /// Specified if this <see cref="SyntheticKey"/> is equal to the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="SyntheticKey"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="SyntheticKey"/>.</returns>
        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        /// <summary>
        /// Converts this <see cref="SyntheticKey"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="SyntheticKey"/>.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return $"SyntheticKey({Identifier})";
            }
            return $"SyntheticKey({Identifier}, {Name})";
        }

        /// <summary>
        /// The current last used identifier. Identifiers are assigned post increment, so the first
        /// identifier will be 1. 0 is unused.
        /// </summary>
        private static int identifier;
    }
}
