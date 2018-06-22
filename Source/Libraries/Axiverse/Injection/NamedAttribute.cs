using System;

namespace Axiverse.Injection
{
    /// <summary>
    /// Attribute defined by a name.
    /// </summary>
    public class NamedAttribute : Attribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructs a <see cref="NamedAttribute"/> with the specified name.
        /// </summary>
        /// <param name="Name"></param>
        public NamedAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
