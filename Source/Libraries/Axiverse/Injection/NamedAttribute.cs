using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Attribute defined by a name.
    /// </summary>
    public class NamedAttribute : Attribute
    {
        public string Name { get; }

        public NamedAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
