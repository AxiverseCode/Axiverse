using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Collision
{
    public class Manifold
    {
        public RigidBody Former { get; set; }
        public RigidBody Latter { get; set; }

        public List<ManifoldContact> Contacts { get; set; }

        public Manifold()
        {
            Contacts = new List<ManifoldContact>();
        }
    }

    public class ManifoldContact
    {
        public Vector3 FormerLocalPoint;
        public Vector3 LatterLocalPoint;

        public Vector3 Position;
        public Vector3 Normal;

        public float Distance;
        public float CombinedFriction;
        public float CombinedAngularFriction;
        public float CombinedSpinningFriction;
        public float CombinedRestitution;
    }

}
