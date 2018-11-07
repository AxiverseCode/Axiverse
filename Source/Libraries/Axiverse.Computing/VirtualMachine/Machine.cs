using System;
using static System.BitConverter;

namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// An virtual machine runner.
    /// </summary>
    public class Machine
    {

        private int stackPointer;
        private int instructionPointer;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Moves forward
        /// </remarks>
        public int InstructionPointer
        {
            get => instructionPointer;
            set => instructionPointer = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Moves backward.
        /// </remarks>
        public int StackPointer
        {
            get => stackPointer;
            set => stackPointer = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public int FramePointer { get; set; }

        /// <summary>
        /// The memory allocated to the program.
        /// </summary>
        public byte[] Memory { get; set; }

        public Machine()
        {
            Memory = new byte[1024];

            StackPointer = Memory.Length;
        }

        // 32 bit?
        // All stack entries are expanded to 32 or 64 bit registers
        public void Go()
        {
            // http://web.cse.ohio-state.edu/~reeves.92/CSE2421au12/SlidesDay43.pdf
            Opcode opcode;
            while ((opcode = (Opcode)Memory[InstructionPointer++]) != Opcode.Halt)
            {
                Console.WriteLine(opcode.ToString());

                switch (opcode)
                {
                    case Opcode.Add:
                        {
                            // Pop 2 from the stack
                            var a = ReadInt32(ref stackPointer);
                            var b = ReadInt32(ref stackPointer);

                            Push(a + b, ref stackPointer);

                            break;
                        }
                    case Opcode.Subtract:
                        {
                            // Pop 2 from the stack
                            var a = ReadInt32(ref stackPointer);
                            var b = ReadInt32(ref stackPointer);

                            Push(a - b, ref stackPointer);

                            break;
                        }
                    case Opcode.Jump:
                        {
                            var address = ReadInt32(ref instructionPointer);
                            instructionPointer = address;

                            break;
                        }
                    case Opcode.JumpIfZero:
                        {
                            var address = ReadInt32(ref instructionPointer);
                            var value = ReadInt32(ref stackPointer);

                            if (value == 0)
                            {
                                instructionPointer = address;
                            }

                            break;
                        }
                    case Opcode.JumpGreaterThanZero:
                        {
                            var address = ReadInt32(ref instructionPointer);
                            var value = ReadInt32(ref stackPointer);

                            if (value > 0)
                            {
                                instructionPointer = address;
                            }

                            break;
                        }
                    case Opcode.Call:
                        {
                            // Read function address and argument count.
                            var address = ReadInt32(ref instructionPointer);
                            var argCount = ReadInt32(ref instructionPointer);

                            // Save state.
                            Push(argCount, ref stackPointer);
                            Push(FramePointer, ref stackPointer);
                            Push(instructionPointer, ref stackPointer);

                            // Call function.
                            FramePointer = stackPointer;
                            instructionPointer = address;
                            break;
                        }
                    case Opcode.Return:
                        {
                            // Save return value.
                            var returnValue = ReadInt32(ref stackPointer);

                            // Reset instruction pointer and frame pointer.
                            stackPointer = FramePointer;
                            instructionPointer = ReadInt32(ref stackPointer);
                            FramePointer = ReadInt32(ref stackPointer);

                            // Discard arguments.
                            var argCount = ReadInt32(ref stackPointer);
                            stackPointer -= argCount * sizeof(Int32);

                            // Push return value.
                            Push(returnValue, ref stackPointer);

                            break;
                        }
                    case Opcode.Const:
                        {
                            // read 32 bit.
                            var v = ToInt32(Memory, InstructionPointer);
                            InstructionPointer += sizeof(Int32);

                            // push onto stack.
                            Push(v, ref stackPointer);

                            break;
                        }
                    case Opcode.Print:
                        {
                            var a = ReadInt32(ref stackPointer);
                            Console.WriteLine(a.ToString("x8"));
                            break;
                        }
                    default:
                        break;
                }
            }

        }

        private Int32 ReadInt32(ref int pointer)
        {
            pointer += sizeof(Int32);
            return ToInt32(Memory, pointer - sizeof(Int32));
        }

        private void Push(Int32 value, ref int pointer)
        {
            pointer -= sizeof(Int32);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(Int32));
        }
    }
}