using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Physics;
using SharpDX;

namespace Axiverse.Interface2
{
    using Vector3 = SharpDX.Vector3;

    public class Entity
    {
        public Body Body { get; set; } = new Body();
        public Mesh Mesh { get; set; }
        public Matrix Transform { get; set; } = Matrix.Identity;

        public virtual void Update(float delta)
        {
            Vector3 position = new Vector3(Body.LinearPosition.X, Body.LinearPosition.Y, Body.LinearPosition.Z);
            Transform = Matrix.Translation(position);

            Body.AccumulateLocalCentralForce(new Axiverse.Vector3(0, 1, 0));
            Body.Integrate(delta);
            Body.ClearForces();
        }
    }
}
