using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics.Shapes;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Computes the collision between two objects
    /// </summary>
    public class CollisionDispatcher
    {

        /// <summary>
        /// Gets a list of all collision manifolds from the given pairs of potential contacts.
        /// </summary>
        /// <param name="pairs">The pairs of potential contacts.</param>
        /// <returns></returns>
        public List<Manifold> Collide(List<ContactPair> pairs)
        {
            var manifolds = new List<Manifold>();

            foreach (var pair in pairs)
            {
                if (m_colliders.TryGetValue(
                    new Tuple<Type, Type>(
                        pair.Former.CollisionShape.GetType(),
                        pair.Latter.CollisionShape.GetType()),
                    out var collider))
                {

                }
            }

            return manifolds;
        }

        /// <summary>
        /// Adds a collider to the dispatcher. Ordering of the types do not matter.
        /// </summary>
        /// <param name="former">The former type of the collider.</param>
        /// <param name="latter">The latter type of the collider.</param>
        /// <param name="collider">The collider to register.</param>
        public void Add(Type former, Type latter, ICollider collider)
        {
            m_colliders.Add(new Tuple<Type, Type>(former, latter), collider);

            if (former != latter)
            {
                m_colliders.Add(new Tuple<Type, Type>(latter, former), collider);
            }
        }

        /// <summary>
        /// Adds a collider to the dispatcher. Ordering of the types do not matter.
        /// </summary>
        /// <typeparam name="T">The former type of the collider.</typeparam>
        /// <typeparam name="U">The latter type of the collider.</typeparam>
        /// <param name="collider">The collider to register.</param>
        public void Add<T, U>(Collider<T, U> collider)
            where T:Shape
            where U:Shape
        {
            Add(typeof(T), typeof(U), collider);
        }

        Dictionary<Tuple<Type, Type>, ICollider> m_colliders = new Dictionary<Tuple<Type, Type>, ICollider>();

        static CollisionDispatcher()
        {
            Default.Add(typeof(Sphere), typeof(Sphere), new SphereCollider());
        }

        /// <summary>
        /// Gets the default dispatcher with common shapes registered.
        /// </summary>
        public static readonly CollisionDispatcher Default = new CollisionDispatcher();
    }
}
