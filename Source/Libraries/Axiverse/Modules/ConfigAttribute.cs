using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{
    /// <summary>
    /// Defines an config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the config.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the config.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Constructs a <see cref="ConfigAttribute"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public ConfigAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
