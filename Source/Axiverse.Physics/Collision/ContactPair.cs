using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Represents a pair of <see cref="RigidBody"/>s which may be in contact.
    /// </summary>
    public class ContactPair
    {
        /// <summary>
        /// The former <see cref="RigidBody"/> which may contact.
        /// </summary>
        public RigidBody Former { get; set; }

        /// <summary>
        /// The latter <see cref="RigidBody"/> which may contact.
        /// </summary>
        public RigidBody Latter { get; set; }

        /// <summary>
        /// Constructs a potential contact pair.
        /// </summary>
        /// <param name="former">The former body.</param>
        /// <param name="latter">The latter body.</param>
        public ContactPair(RigidBody former, RigidBody latter)
        {
            Former = former;
            Latter = latter;
        }
    }
}