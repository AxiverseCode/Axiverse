using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Represents a pair of <see cref="Body"/>s which may be in contact.
    /// </summary>
    public class ContactPair
    {
        /// <summary>
        /// The former <see cref="Body"/> which may contact.
        /// </summary>
        public Body Former { get; set; }

        /// <summary>
        /// The latter <see cref="Body"/> which may contact.
        /// </summary>
        public Body Latter { get; set; }

        /// <summary>
        /// Constructs a potential contact pair.
        /// </summary>
        /// <param name="former">The former body.</param>
        /// <param name="latter">The latter body.</param>
        public ContactPair(Body former, Body latter)
        {
            Former = former;
            Latter = latter;
        }
    }
}