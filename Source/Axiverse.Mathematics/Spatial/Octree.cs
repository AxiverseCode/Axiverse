using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Spatial
{
    public class Octree
    {

    
    }

    public class OctreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        public Bounds3 Bounds { get; private set; }

        OctreeNode[] children = new OctreeNode[8];
        OctreeNode[] neigbors = new OctreeNode[8];


        /// <summary>
        /// 
        /// </summary>
        public OctreeNode(OctreeNode parent, Bounds3 bounds)
        {
            Bounds = bounds;
        }

        private void CreateChild(Boolean3 octant)
        {
            OctreeNode node = new OctreeNode(this, GetChildBounds(Bounds, octant));
            
        }

        private void Detach()
        {
            for (int i = 0; i < 8; i++)
            {
                Boolean3 direction = (Boolean3)i;
                if (neigbors[i] != null)
                {
                    // remove this from the neighbor
                    neigbors[i].neigbors[(int)!direction] = null;
                    neigbors[i] = null;
                }
            }
        }

        /// <summary>
        /// Creates the bounding box for the specified octant.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="octant"></param>
        /// <returns></returns>
        private static Bounds3 GetChildBounds(Bounds3 bounds, Boolean3 octant)
        {
            Vector3 center = bounds.Center;
            Vector3 half = bounds.Size / 2;
            Vector3 corner = center + (half * octant.ToVector3());

            return Bounds3.FromVectors(center, corner);
        }
    }
}
