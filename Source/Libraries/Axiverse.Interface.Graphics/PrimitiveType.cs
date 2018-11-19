using SharpDX.Direct3D;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Primitive types for drawing.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = PrimitiveTopology.Undefined,

        /// <summary>
        /// Point list.
        /// </summary>
        PointList = PrimitiveTopology.PointList,

        /// <summary>
        /// Line list.
        /// </summary>
        LineList = PrimitiveTopology.LineList,
        /// <summary>
        /// Line strip.
        /// </summary>
        LineStrip = PrimitiveTopology.LineStrip,

        /// <summary>
        /// Triangle list.
        /// </summary>
        TriangleList = PrimitiveTopology.TriangleList,
        /// <summary>
        /// Triangle strip.
        /// </summary>
        TriangleStrip = PrimitiveTopology.TriangleStrip,

        /// <summary>
        /// Patch.
        /// </summary>
        Patch = PrimitiveTopology.PatchListWith10ControlPoints,
    }
}
