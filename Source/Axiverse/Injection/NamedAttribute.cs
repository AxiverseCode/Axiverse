using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    public class NamedAttribute : Attribute
    {
        public string Name { get; }

        public NamedAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
