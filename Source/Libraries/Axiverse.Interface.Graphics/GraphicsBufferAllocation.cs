using System;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Allocation of space within a buffer for parameter binding in constant buffers.
    /// </summary>
    public class GraphicsBufferAllocation
    {
        public GraphicsBuffer Buffer;
        public IntPtr Data;
        public int Size;
        public int Offset; //Needed?
    }
}
