using Axiverse.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// The type of module
        /// </summary>
        public Type ModuleType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleType"></param>
        public DependencyAttribute(Type moduleType)
        {
            ModuleType = Requires.AssignableFrom<Module>(moduleType);
        }
    }
}
