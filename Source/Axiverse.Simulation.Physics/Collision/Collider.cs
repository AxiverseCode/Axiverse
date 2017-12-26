using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Physics.Shapes;

namespace Axiverse.Simulation.Physics.Collision
{
    public abstract class Collider<T, U> : ICollider
        where T : Shape
        where U : Shape
    {
        public abstract Manifold Collide(T former, U latter);

        public Manifold Collide(Shape former, Shape latter)
        {
            return Collide(former as T, latter as U);
        }

        public float ImpactTime()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICollider
    {
        Manifold Collide(Shape former, Shape latter);
        float ImpactTime();
    }
}
