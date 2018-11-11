using System;
using static System.BitConverter;

namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// An virtual machine runner.
    /// </summary>
    /// <remarks>
    /// http://coding-geek.com/jvm-memory-model/
    /// </remarks>
    public class ExecutionEngine
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

        public ExecutionEngine(byte[] program)
        {
            Memory = new byte[1024];

            Buffer.BlockCopy(program, 0, Memory, 0, program.Length);

            StackPointer = Memory.Length;

            // Skip entry point metadata.
            InstructionPointer = 4;
        }

        // 32 bit?
        // All stack entries are expanded to 32 or 64 bit registers
        public void Go()
        {
            // http://web.cse.ohio-state.edu/~reeves.92/CSE2421au12/SlidesDay43.pdf
            Opcode opcode;
            int baseAddress;
            while ((opcode = (Opcode)Memory[baseAddress = InstructionPointer++]) != Opcode.Halt)
            {
                Console.WriteLine(opcode.ToString());

                switch (opcode)
                {
                    case Opcode.Call16:
                        {
                            // Read function address and argument count.
                            var displacement = ReadInt16(ref instructionPointer);
                            var returnAddress = instructionPointer;
                            instructionPointer = baseAddress + displacement;
                            var metadata = ReadInt32(ref instructionPointer);

                            // Save state.
                            Push(metadata, ref stackPointer);
                            Push(FramePointer, ref stackPointer);
                            Push(returnAddress, ref stackPointer);

                            // Call function.
                            FramePointer = stackPointer;
                            break;
                        }
                    case Opcode.Return:
                        {
                            // Reset instruction pointer and frame pointer.
                            stackPointer = FramePointer;
                            instructionPointer = ReadInt32(ref stackPointer);
                            FramePointer = ReadInt32(ref stackPointer);
                            var metadata = ReadInt32(ref stackPointer);

                            // Discard arguments.
                            var argCount = metadata & 0xff;
                            stackPointer -= argCount * sizeof(int);

                            break;
                        }
                    case Opcode.AddI32:
                        {
                            Push(
                                ReadInt32(ref stackPointer) + ReadInt32(ref stackPointer),
                                ref stackPointer);
                            break;
                        }
                    case Opcode.SubtractI32:
                        {
                            Push(
                                ReadInt32(ref stackPointer) - ReadInt32(ref stackPointer),
                                ref stackPointer);
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
                                instructionPointer += address;
                            }

                            break;
                        }
                    case Opcode.JumpGreaterThanZero:
                        {
                            var address = ReadInt32(ref instructionPointer);
                            var value = ReadInt32(ref stackPointer);

                            if (value > 0)
                            {
                                instructionPointer += address;
                            }

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

        private short ReadInt16(ref int pointer)
        {
            pointer += sizeof(short);
            return ToInt16(Memory, pointer - sizeof(short));
        }

        private int ReadInt32(ref int pointer)
        {
            pointer += sizeof(int);
            return ToInt32(Memory, pointer - sizeof(int));
        }

        private long ReadInt64(ref int pointer)
        {
            pointer += sizeof(long);
            return ToInt64(Memory, pointer - sizeof(long));
        }

        private float ReadSingle(ref int pointer)
        {
            pointer += sizeof(float);
            return ToSingle(Memory, pointer - sizeof(float));
        }

        private double ReadDouble(ref int pointer)
        {
            pointer += sizeof(double);
            return ToDouble(Memory, pointer - sizeof(double));
        }

        private void Push(int value, ref int pointer)
        {
            pointer -= sizeof(int);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(int));
        }

        private void Push(long value, ref int pointer)
        {
            pointer -= sizeof(long);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(long));
        }

        private void Push(float value, ref int pointer)
        {
            pointer -= sizeof(float);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(float));
        }

        private void Push(double value, ref int pointer)
        {
            pointer -= sizeof(double);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(double));
        }
    }
}