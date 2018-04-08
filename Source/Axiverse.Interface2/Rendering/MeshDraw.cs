using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Interface.Graphics;

namespace Axiverse.Interface.Rendering
{
    public class MeshDraw
    {
        public PrimitiveType PrimitiveType;

        public int Count;

        public int Offset;

        public VertexBufferBinding[] VertexBuffers;

        public IndexBufferBinding IndexBuffer;
    }
}
