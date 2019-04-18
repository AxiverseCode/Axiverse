using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    public class Voxmap
    {
        // rgba
        private readonly uint[,,] voxels;

        /// <summary>
        /// Gets or sets the color voxel;
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public uint this[int x, int y, int z]
        {
            get
            {
                return voxels[x, y, z];
            }
        }

        public Voxmap(int width, int height, int depth)
        {
            voxels = new uint[width, height, depth];
        }
    }
}
