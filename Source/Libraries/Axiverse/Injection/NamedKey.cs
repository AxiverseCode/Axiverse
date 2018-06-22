using System;

namespace Axiverse.Injection
{
    /// <summary>
    /// Gets a key defined by a name.
    /// TODO: Replace with named attribute when the attributed key allows for multiple attributes.
    /// </summary>
    public class NamedKey : Key
    {
        /// <summary>
        /// Gets the name of the key. For a <see cref="NamedKey"/>, this is used for equality.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructs a <see cref="SyntheticKey"/> with the given display name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        protected internal NamedKey(Type type, String name) : base(type)
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
            if (obj is NamedKey key)
            {
                return key.Name == Name && key.Type == Type;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="SyntheticKey"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="SyntheticKey"/>.</returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Name.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="SyntheticKey"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="SyntheticKey"/>.</returns>
        public override string ToString()
        {
            return $"NamedKey({Name}, {Type.Name})";
        }
    }
}
