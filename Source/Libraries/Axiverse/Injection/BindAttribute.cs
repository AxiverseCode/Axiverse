using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Indicates that a field or property should be bound by the binder.
    /// </summary>
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class BindAttribute : Attribute {}
}
