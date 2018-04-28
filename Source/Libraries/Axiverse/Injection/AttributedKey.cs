using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override bool Equals(object obj)
        {
            if (obj is AttributedKey key)
            {
                return Type.Equals(key.Type) && Attribute.Equals(key.Attribute);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Attribute.GetHashCode();
        }
    }
}
