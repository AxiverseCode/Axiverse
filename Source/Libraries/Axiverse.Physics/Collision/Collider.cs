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
    /// Ordering of the types don't matter, but by convention the types should be alphabetical.
    /// </summary>
    /// <typeparam name="T">First type to collide.</typeparam>
    /// <typeparam name="U">Second type to collide</typeparam>
    public abstract class Collider<T, U> : ICollider
        where T : Shape
        where U : Shape
    {
        /// <summary>
        /// Detects if the two shapes collide and if they do create a manifold.
        /// </summary>
        /// <param name="former">The former shape to collide.</param>
        /// <param name="latter">The latter shape to collide.</param>
        /// <returns>The manifold of the collision.</returns>
        public Manifold Collide(Shape former, Shape latter)
        {
            var left = former as T;
            var right = latter as U;

            if (left == null || right == null)
            {
                return null;
            }
            return Collide(left, right);
        }

        /// <summary>
        /// Detects if the two shapes collide and if they do create a manifold.
        /// </summary>
        /// <param name="former">The former shape to collide.</param>
        /// <param name="latter">The latter shape to collide.</param>
        /// <returns>The manifold of the collision.</returns>
        public abstract Manifold Collide(T former, U latter);
    }
}
