using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class GraphicsBufferAllocation
    {
        public GraphicsBuffer Buffer;
        public IntPtr Data;
        public int Size;
        public int Offset; //Needed?
    }
}
