using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class IndexBufferBinding
    {
        public GraphicsBuffer Buffer;
        public IndexBufferType Stride;
        public int Offset;
        public int Count;
    }

    public enum IndexBufferType
    {
        Integer16,
        Integer32,
    }
}
