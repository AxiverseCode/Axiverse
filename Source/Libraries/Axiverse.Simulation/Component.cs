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
    public abstract class Component : IComponent
    {
        /// <summary>
        /// Gets the entity this component is bound to.
        /// </summary>
        public Entity Entity { get; internal set; }

        /// <summary>
        /// Clones the component.
        /// </summary>
        /// <returns></returns>
        public virtual Component Clone()
        {
            throw new NotImplementedException();
        }
    }
}
