using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
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

        public bool IsAssignableFrom(Type type)
        {
            return Type.IsAssignableFrom(type);
        }

        public bool IsAssignableFrom(object value)
        {
            return Type.IsAssignableFrom(value.GetType());
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

        public static Key From(Type type, string name)
        {
            return new NamedKey(type, name);
        }

        public static Key From(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(false);
            var type = fieldInfo.FieldType;

            if (attributes.Length > 1)
            {
                // TODO: allow multiple attributes
                throw new AmbiguousMatchException();
            }

            return From(type, attributes.Length > 0 ? attributes[0] as Attribute : null);
        }

        public static Key From(ParameterInfo parameterInfo)
        {
            var attributes = parameterInfo.GetCustomAttributes(false);
            var type = parameterInfo.ParameterType;

            if (attributes.Length > 1)
            {
                // TODO: allow multiple attributes
                throw new AmbiguousMatchException();
            }

            return From(type, attributes.Length > 0 ? attributes[0] as Attribute : null);
        }

        public static Key From(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(false);
            var type = propertyInfo.PropertyType;

            if (attributes.Length > 1)
            {
                // TODO: allow multiple attributes
                throw new AmbiguousMatchException();
            }

            return From(type, attributes.Length > 0 ? attributes[0] as Attribute : null);
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

        protected static class TypedKey<T>
        {
            public static readonly Key Key = new Key(typeof(T));
        }
    }
}
