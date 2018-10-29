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
    /// A broad phase brute force filter detecting if axis-aligned bounding-boxes overlap.
    /// </summary>
    public class BruteForceFilter : IFilter
    {
        public World World { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BruteForceFilter(World world)
        {
            World = world;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ContactPair> Detect()
        {
            var pairs = new List<ContactPair>();

            for (int i = 0; i < World.Bodies.Count; i++)
            {
                var former = World.Bodies[i];

                for (int j = i + 1; j < World.Bodies.Count; j++)
                {
                    var latter = World.Bodies[j];

                    if (Detect(former, latter))
                    {
                        // generate collision pair for narrow-phase
                        pairs.Add(new ContactPair(former, latter));
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
        public bool Detect(Body left, Body right)
        {
            return left.CalculateBounds().Intersects(right.CalculateBounds());
        }

    }
}
