using System;

namespace Axiverse.Injection
{
    /// <summary>
    /// Key representing an injecting binding by both type and attribute.
    /// </summary>
    public class AttributedKey : Key
    {
        /// <summary>
        /// Gets the attribute associated with the key.
        /// </summary>
        public Attribute Attribute { get; }

        internal AttributedKey(Type type, Attribute attribute) : base(type)
        {
            Attribute = attribute;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is AttributedKey key)
            {
                return Type.Equals(key.Type) && Attribute.Equals(key.Attribute);
            }
            return false;
        }

        /// <summary>
        /// Gets the hash code for the current object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Attribute.GetHashCode();
        }
    }
}
