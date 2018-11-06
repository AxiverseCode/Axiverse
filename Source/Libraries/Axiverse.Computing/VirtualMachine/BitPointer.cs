using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.VirtualMachine
{
    public class BitPointer
    {
        public byte[] Buffer { get; set; }

        public int Pointer { get; set; }

        public byte ReadUint8()
        {
            return Buffer[Pointer];
        }

        public sbyte ReadInt8()
        {
            return unchecked((sbyte)Buffer[Pointer]);
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(Buffer, Pointer);
        }
    }
}
