using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Describes a single contact point in a manifold.
    /// </summary>
    public class ManifoldContact
    {
        /// <summary>
        /// Gets or sets the point of contact in the perspective of the local space of the former
        /// body.
        /// </summary>
        public Vector3 FormerLocalPoint;

        /// <summary>
        /// Gets or sets the point of contact in the perspective of the local space of the latter
        /// body.
        /// </summary>
        public Vector3 LatterLocalPoint;

        /// <summary>
        /// Gets or sets the point of contact in world space.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets the normal of the contact from the persepctive of the former body.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the distance or depth of penetration.
        /// </summary>
        public float Distance;

        /// <summary>
        /// Gets or sets the combined friction of the two bodies.
        /// </summary>
        public float CombinedFriction;

        /// <summary>
        /// Gets or sets the combined angular friction of the two bodies.
        /// </summary>
        public float CombinedAngularFriction;

        /// <summary>
        /// Gets or sets the combined spinning friction of the two bodies.
        /// </summary>
        public float CombinedSpinningFriction;

        /// <summary>
        /// Gets or sets the combined restitution of the two bodies.
        /// </summary>
        public float CombinedRestitution;
    }
}
