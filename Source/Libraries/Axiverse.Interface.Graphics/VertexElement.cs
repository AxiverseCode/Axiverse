namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// An element in a vertex layout.
    /// </summary>
    public struct VertexElement
    {
        /// <summary>
        /// Gets or sets the name of the vertex element.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets the offset of the vertex element.
        /// </summary>
        public int Offset;

        /// <summary>
        /// Gets or sets the format of the vertex element.
        /// </summary>
        public VertexFormat Format;

        /// <summary>
        /// Constructs a vertex element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="offset"></param>
        /// <param name="format"></param>
        public VertexElement(string name, int offset, VertexFormat format)
        {
            Name = name;
            Offset = offset;
            Format = format;
        }
    }
}
