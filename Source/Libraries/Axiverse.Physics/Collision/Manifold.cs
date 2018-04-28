using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// A collision manifold which describes the contact between two bodies.
    /// </summary>
    public class Manifold
    {
        /// <summary>
        /// Gets or sets the former <see cref="Body"/> in contact.
        /// </summary>
        public Body Former { get; set; }

        /// <summary>
        /// Gets or sets latter <see cref="Body"/> in contact.
        /// </summary>
        public Body Latter { get; set; }

        /// <summary>
        /// Gets the list of contacts in the manifold.
        /// </summary>
        public List<ManifoldContact> Contacts { get; } = new List<ManifoldContact>();
    }

}
