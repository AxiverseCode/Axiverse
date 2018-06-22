using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Determines whether the specified type can be assigned to the type specified by this
        /// key.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsAssignableFrom(Type type)
        {
            return Type.IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the type of the specified object can be assigned to the type
        /// specified by this key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsAssignableFrom(object value)
        {
            return Type.IsAssignableFrom(value.GetType());
        }

        /// <summary>
        /// Determines whether this is equal to the specified object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Key key)
            {
                return Type.Equals(key.Type);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Key"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="Key"/>.</returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Key"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="Key"/>.</returns>
        public override string ToString()
        {
            return $"Key({Type.Name})";
        }

        /// <summary>
        /// Gets a key from the specified binding type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Key From<T>()
        {
            return TypedKey<T>.Key;
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
        public static Key From(Type type, Attribute attribute)
        {
            if (attribute == null)
            {
                return From(type);
            }

            if (attribute is NamedAttribute named)
            {
                return new NamedKey(type, named.Name);
            }

            return new AttributedKey(type, attribute);
        }

        /// <summary>
        /// Gets a key from the specified binding type and attribute.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static Key From(Type type, IEnumerable<Attribute> attributes)
        {
            Key key = null;
            foreach (var attribute in attributes)
            {
                if (ignoredTypes.Contains(attribute.GetType()))
                {
                    continue;
                }

                if (key != null)
                {
                    throw new NotSupportedException();
                }
                key = From(type, attribute);
            }

            return key ?? From(type);
        }

        /// <summary>
        /// Gets a named key from the specified binding type and name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Key From(Type type, string name)
        {
            return new NamedKey(type, name);
        }

        /// <summary>
        /// Creates a unique key from the specified binding type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Key CreateSynthetic(Type type)
        {
            return new SyntheticKey(type);
        }

        /// <summary>
        /// Creates a unique key from the specified binding type with the given display name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Key CreateSynthetic(Type type, string name)
        {
            return new SyntheticKey(type, name);
        }

        /// <summary>
        /// Singleton creation of a typed key without any attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected static class TypedKey<T>
        {
            /// <summary>
            /// The key of this type.
            /// </summary>
            public static readonly Key Key = new Key(typeof(T));
        }

        /// <summary>
        /// Registers a type of an attribute to be ignored when determining the key from a
        /// collection of attributes. By default <see cref="BindAttribute"/> and
        /// <see cref="InjectAttribute"/> is ignored.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="InvalidCastException">
        /// Throws if the type is not of an<see cref="Attribute"/>.
        /// </exception>
        public static void IgnoreAttribute(Type type)
        {
            Requires.AssignableFrom<Attribute>(type);
            ignoredTypes.Add(type);
        }

        static Key()
        {
            ignoredTypes.Add(typeof(BindAttribute));
            ignoredTypes.Add(typeof(InjectAttribute));
        }

        private static List<Type> ignoredTypes = new List<Type>();
    }
}
