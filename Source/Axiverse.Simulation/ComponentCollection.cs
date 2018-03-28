using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// Represents a collection of components bound to an entity.
    /// </summary>
    public class ComponentCollection
    {
        public Component this[Type type]
        {
            get
            {
                return components[type];
            }
            set
            {
                Contract.Requires<InvalidCastException>(type.IsAssignableFrom(value.GetType()));
                components[type] = value;
            }
        }

        /// <summary>
        /// Gets the number of components 
        /// </summary>
        public int Count => components.Count;

        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();
    }
}
