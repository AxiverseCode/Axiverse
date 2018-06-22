using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Gets a key defined by a name.
    /// TODO: Replace with named attribute when the attributed key allows for multiple attributes.
    /// </summary>
    public class NamedKey : Key
    {
        /// <summary>
        /// Gets or sets an optional display name. This field is not used for binding equality. The
        /// specific instance of the key must be used for binding.
        /// </summary>
        public string Name { get; set; }

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
