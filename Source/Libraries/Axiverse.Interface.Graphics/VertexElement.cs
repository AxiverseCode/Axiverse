using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public struct VertexElement
    {
        public string Name;
        public int Offset;
        public VertexFormat Format;

        public VertexElement(string name, int offset, VertexFormat format)
        {
            Name = name;
            Offset = offset;
            Format = format;
        }
    }
}
