using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public enum PrimitiveType
    {
        Undefined,
        PointList,

        LineList,
        LineStrip,

        TriangleList,
        TriangleStrip,

        Patch,
    }
}
