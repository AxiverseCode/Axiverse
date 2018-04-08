using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public enum PrimitiveType
    {
        Undefined = PrimitiveTopology.Undefined,
        PointList = PrimitiveTopology.PointList,

        LineList = PrimitiveTopology.LineList,
        LineStrip = PrimitiveTopology.LineStrip,

        TriangleList = PrimitiveTopology.TriangleList,
        TriangleStrip = PrimitiveTopology.TriangleStrip,

        Patch = PrimitiveTopology.PatchListWith10ControlPoints,
    }
}
