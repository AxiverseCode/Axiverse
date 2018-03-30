using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Key representing an injection binding by type.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// Gets the type of values bound to the key.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Constructs a <see cref="Key"/>.
        /// </summary>
        /// <param name="type">The type of values bound by the <see cref="Key"/>.</param>
        protected internal Key(Type type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is Key key)
            {
                return Type.Equals(key.Type);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        /// <summary>
        /// Gets a key from the specified binding type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Key From(Type type)
        {
            return new Key(type);
        }

        /// <summary>
        /// Gets a key from the specified binding type and attribute.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static Key From(Type type, Type attribute)
        {
            return new AttributedKey(type, attribute);
        }

        /// <summary>
        /// Creates a unique key from the specified binding type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Key Create(Type type)
        {
            return new SyntheticKey(type);
        }

        /// <summary>
        /// Creates a unique key from the specified binding type with the given display name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Key Create(Type type, string name)
        {
            return new SyntheticKey(type, name);
        }
    }
}
