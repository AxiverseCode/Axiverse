using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{
    /// <summary>
    /// Indicates that a module is an entry point.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MainModuleAttribute : Attribute
    {
    }
}
