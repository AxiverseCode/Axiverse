using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    public class PyramidFrustum3
    {
        public Plane3 Left;
        public Plane3 Right;
        public Plane3 Bottom;
        public Plane3 Top;
        public Plane3 Near;
        public Plane3 Far;

        public Plane3 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Left;
                    case 1: return Right;
                    case 2: return Bottom;
                    case 3: return Top;
                    case 4: return Near;
                    case 5: return Far;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Left = value; break;
                    case 1: Right = value; break;
                    case 2: Bottom = value; break;
                    case 3: Top = value; break;
                    case 4: Near = value; break;
                    case 5: Far = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static PyramidFrustum3 FromProjection(Matrix4 projection)
        {
            PyramidFrustum3 result = new PyramidFrustum3();
            for (int i = 0; i < 4; i++)
            {
                result.Left[i] = projection[i, 3] + projection[i, 0];
                result.Right[i] = projection[i, 3] - projection[i, 0];
                result.Bottom[i] = projection[i, 3] + projection[i, 1];
                result.Top[i] = projection[i, 3] - projection[i, 1];
                result.Near[i] = projection[i, 3] + projection[i, 2];
                result.Far[i] = projection[i, 3] - projection[i, 2];
            }
            return result;
        }
    }
}
