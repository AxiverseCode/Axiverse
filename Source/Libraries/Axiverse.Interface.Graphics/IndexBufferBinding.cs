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
        public IndexBufferType Type;
        //public int Offset;
        public int Count;
        public int Stride => StrideOf(Type);

        public int StrideOf(IndexBufferType type)
        {
            switch(type)
            {
                case IndexBufferType.Integer16: return 2;
                case IndexBufferType.Integer32: return 4;
                default: return 0;
            }
        }
    }

    public enum IndexBufferType
    {
        Integer16,
        Integer32,
    }
}
