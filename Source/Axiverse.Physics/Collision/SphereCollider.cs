using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Physics.Shapes;

namespace Axiverse.Physics.Collision
{
    /// <summary>
    /// Collider to create a <see cref="Manifold"/> from the potential collision of two spheres.
    /// </summary>
    public class SphereCollider : Collider<Sphere, Sphere>
    {
        /// <summary>
        /// Detects if the two spheres collide and if they do create a manifold.
        /// </summary>
        /// <param name="former">The former sphere to collide.</param>
        /// <param name="latter">The latter sphere to collide.</param>
        /// <returns>The manifold of the collision.</returns>
        public override Manifold Collide(Sphere former, Sphere latter)
        {
            Vector3 difference = former.Position - latter.Position;
            float length = difference.Length();
            float formerRadius = former.Radius;
            float latterRadius = latter.Radius;

            if (length > (formerRadius + latterRadius))
            {
                return null;
            }

            float distance = length - (formerRadius + latterRadius);

            Vector3 normal = Vector3.UnitX;
            if (length > float.Epsilon)
            {
                normal = difference / length;
            }

            Vector3 position = former.Position + formerRadius * normal;


            // add contact point!
            throw new NotImplementedException();

        }
    }
}
