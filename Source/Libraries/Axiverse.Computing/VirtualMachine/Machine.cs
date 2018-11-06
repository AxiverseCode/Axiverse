using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.BitConverter;
namespace Axiverse.Computing.VirtualMachine
{
    public class Machine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Moves forward
        /// </remarks>
        public int InstructionPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Moves backward.
        /// </remarks>
        public int StackPointer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public int FramePointer { get; set; }

        public byte[] Memory { get; set; } = new byte[1024];

        // 32 bit?
        public void Go()
        {
            // http://web.cse.ohio-state.edu/~reeves.92/CSE2421au12/SlidesDay43.pdf
            Opcode opcode;
            while ((opcode = (Opcode)Memory[InstructionPointer++]) != Opcode.Halt)
            {
                switch (opcode)
                {
                    case Opcode.Add:
                        {
                            // Pop 2 from the stack
                            var a = ToInt32(Memory, StackPointer);
                            StackPointer += sizeof(Int32);
                            var b = ToInt32(Memory, StackPointer);
                            StackPointer += sizeof(Int32);

                            var c = a + b;
                            Buffer.BlockCopy(GetBytes(c), 0, Memory, StackPointer, sizeof(Int32));
                            StackPointer -= sizeof(Int32);

                            break;
                        }
                    case Opcode.Call:
                        {
                            // Allocate return space
                            // Push parameters
                            // 'Call'
                            // - Allocates locals
                            // - Writes return address (IP)
                            // - Sets frame pointer
                            // - Write previous frame pointer
                            break;
                        }
                    case Opcode.Return:
                        {
                            // 
                            break;
                        }
                    case Opcode.Const:
                        {
                            // read 32 bit.
                            var v = ToInt32(Memory, InstructionPointer);
                            InstructionPointer += sizeof(Int32);

                            // push onto stack.
                            Buffer.BlockCopy(GetBytes(v), 0, Memory, StackPointer, sizeof(Int32));
                            StackPointer -= sizeof(Int32);

                            break;
                        }
                }
            }


        }
    }
}