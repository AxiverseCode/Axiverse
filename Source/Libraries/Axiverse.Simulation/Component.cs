using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Component which represents a set of related attributes in an entity.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Clones the component.
        /// </summary>
        /// <returns></returns>
        public abstract Component Clone();
    }
}
