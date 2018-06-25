using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Input
{
    /// <summary>
    /// A listener for input.
    /// </summary>
    public abstract class Listener
    {
        /// <summary>
        /// Gets or sets whether the listener is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the listener.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the listener.
        /// </summary>
        public string Description { get; set; }

    }
}
