using Axiverse.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DependencyAttribute : Attribute
    {
        public Type ModuleType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleType"></param>
        public DependencyAttribute(Type moduleType)
        {
            Requires.AssignableFrom(Key.From<Module>(), moduleType);
        }
    }
}
