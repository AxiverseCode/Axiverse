using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics.Shapes;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Collider to create a <see cref="Manifold"/> from the potential collision of two shapes.
    /// </summary>
    public interface ICollider
    {
        /// <summary>
        /// Detects if the two shapes collide and if they do create a manifold.
        /// </summary>
        /// <param name="former">The former shape to collide.</param>
        /// <param name="latter">The latter shape to collide.</param>
        /// <returns>The manifold of the collision.</returns>
        Manifold Collide(Shape former, Shape latter);
    }
}
