using Axiverse.Interface.Graphics;

namespace Axiverse.Interface.Rendering
{
    /// <summary>
    /// Mesh geometry draw information.
    /// </summary>
    public class MeshDraw
    {
        /// <summary>
        /// The type of primitive described.
        /// </summary>
        public PrimitiveType PrimitiveType;

        /// <summary>
        /// Gets or sets the number of primitives.
        /// </summary>
        public int Count;

        /// <summary>
        /// Gets or sets the offset of the buffers.
        /// </summary>
        public int Offset;

        /// <summary>
        /// Gets or sets the vertex buffers.
        /// </summary>
        public VertexBufferBinding[] VertexBuffers;

        /// <summary>
        /// Gets or sets the index buffer.
        /// </summary>
        public IndexBufferBinding IndexBuffer;
    }
}
