using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Simulation.Physics.Shapes;

namespace Axiverse.Physics.Collision
{
    public abstract class SeparatingAxisCollider<T, U> : Collider<T, U>
        where T : Shape
        where U : Shape
    {
        public override Manifold Collide(T former, U latter)
        {
            // https://gamedev.stackexchange.com/questions/26888/finding-the-contact-point-with-sat

            // for each normal
            {
                // project all vertices onto  that normal

                // if they don't overlap, there is no collision
            }

            // if there is a collision, generate a contact point?
            return null;
        }

        public List<Vector3> ExtractVertex(Shape s)
        {
            return null;
        }

        public List<Vector3> ExtractNormals(Shape s)
        {
            return null;
        }
    }
}
