using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Physics.Shapes;

namespace Axiverse.Simulation.Physics.Collision
{
    /// <summary>
    /// Computes the collision between two objects
    /// </summary>
    public class Dispatcher
    {
        public Dispatcher()
        {
            Add(typeof(Sphere), typeof(Sphere), new SphereCollider());
        }

        public void Add(Type left, Type right, ICollider collider)
        {
            m_colliders.Add(new Tuple<Type, Type>(left, right), collider);

            if (left != right)
            {
                m_colliders.Add(new Tuple<Type, Type>(right, left), collider);
            }
        }

        Dictionary<Tuple<Type, Type>, ICollider> m_colliders = new Dictionary<Tuple<Type, Type>, ICollider>();
    }
}
