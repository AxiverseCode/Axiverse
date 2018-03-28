using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Simulation.Physics.Shapes;

namespace Axiverse.Physics.Collision
{
    public class SphereCollider : Collider<Sphere, Sphere>
    {
        public float CalculateImpactTime()
        {
            throw new NotImplementedException();
        }

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
