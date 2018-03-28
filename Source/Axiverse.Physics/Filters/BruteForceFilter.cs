using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics.Collision;
using Axiverse.Physics.Shapes;

namespace Axiverse.Physics.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class BruteForceFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public List<IBoundedBody> Bodies { get; } = new List<IBoundedBody>();
        // single phase brute force collider

        /// <summary>
        /// 
        /// </summary>
        public BruteForceFilter()
        {
            Bodies = new List<IBoundedBody>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                        pairs.Add(new ContactPair((Body)former, (Body)latter));
                    }
                }
            }

            return pairs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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
