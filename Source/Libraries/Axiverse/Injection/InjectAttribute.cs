using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Indicates that a constructor should be used for activating the class by injection. Activate
    /// will throw an <see cref="AmbiguousMatchException"/> if multiple constructors are annotated
    /// with <see cref="InjectAttribute"/>. If <see cref="InjectAttribute"/> isn't specified on any
    /// constructor, the default public constructor will be used or the only public constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectAttribute : Attribute {}
}
