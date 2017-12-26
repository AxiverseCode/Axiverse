using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Physics.Collision
{
    public class BruteForceFilter
    {
        public List<IBoundedBody> Bodies { get; }
        // single phase brute force collider

        public BruteForceFilter()
        {
            Bodies = new List<IBoundedBody>();
        }

        public List<ContactPair> Detect()
        {
            var pairs = new List<ContactPair>();

            for (int i = 0; i < Bodies.Count; i++)
            {
                IBoundedBody former = Bodies[i];

                for (int j = i + 1; j < Bodies.Count; j++)
                {
                    IBoundedBody latter = Bodies[j];

                    if (Detect(former, latter))
                    {
                        // generate collision pair for narrow-phase
                        pairs.Add(new ContactPair { Former = (RigidBody)former, Latter = (RigidBody)latter });
                    }
                }
            }

            return pairs;
        }

        public bool Detect(IBoundedBody left, IBoundedBody right)
        {
            if (left.IsUnaffected && right.IsUnaffected)
            {
                return false;
            }

            return left.Bounds.Intersects(right.Bounds);
        }

    }
}
