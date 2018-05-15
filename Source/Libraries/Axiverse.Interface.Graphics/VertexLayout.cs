using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Represents the layout of a vertex.
    /// </summary>
    public class VertexLayout
    {
        /// <summary>
        /// Gets the list of elements defined by this layout.
        /// </summary>
        public List<VertexElement> Elements { get; }

        /// <summary>
        /// Gets the stride of vertices defined by the layout.
        /// </summary>
        public int Stride { get; private set; }

        public VertexLayout()
        {
            Elements = new List<VertexElement>();
        }

        public VertexLayout(params VertexElement[] elements)
        {
            Elements = new List<VertexElement>(elements);

            Stride = elements.Max(e => e.Offset + GetStride(e.Format));
        }

        public void Add(string name, VertexFormat format)
        {
            Elements.Add(new VertexElement()
            {
                Format = format,
                Name = name,
                Offset = Stride,
            });
            Stride += GetStride(format);
        }

        public static Format GetFormat(VertexFormat vertexFormat)
        {
            switch (vertexFormat)
            {
                case VertexFormat.Vector2: return Format.R32G32_Float;
                case VertexFormat.Vector3: return Format.R32G32B32_Float;
                case VertexFormat.Vector4: return Format.R32G32B32A32_Float;
                default: return Format.Unknown;
            }
        }

        public static int GetStride(VertexFormat vertexFormat)
        {
            switch (vertexFormat)
            {
                case VertexFormat.Vector2: return 8;
                case VertexFormat.Vector3: return 12;
                case VertexFormat.Vector4: return 16;
                default: return 0;
            }
        }
    }
}
