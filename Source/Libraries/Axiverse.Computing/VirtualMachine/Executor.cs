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
    public class Executor
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

        /// <summary>
        /// Constructs an executor.
        /// </summary>
        /// <param name="program"></param>
        public Executor(byte[] program) : this(program, program.Length)
        {

        }

        /// <summary>
        /// Constructs an executor.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="count"></param>
        public Executor(byte[] program, int count)
        {
            Requires.That(count > program.Length);
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

                    // Unary operations (a) -> v
                    case Opcode.Complement32: OperationI32(ref stackPointer, a => ~a); break;
                    case Opcode.Not32: OperationI32(ref stackPointer, a => a == 0 ? 1 : 0); break;
                    case Opcode.NegateI32: OperationI32(ref stackPointer, a => -a); break;
                    case Opcode.NegateF32: OperationF32(ref stackPointer, a => -a); break;

                    // Binary operations (a, b) -> v
                    case Opcode.And32: OperationI32(ref stackPointer, (a, b) => a & b); break;
                    case Opcode.Or32: OperationI32(ref stackPointer, (a, b) => a | b); break;
                    case Opcode.Xor32: OperationI32(ref stackPointer, (a, b) => a ^ b); break;
                    case Opcode.LeftShiftI32: OperationI32(ref stackPointer, (a, b) => a << b); break;
                    case Opcode.LeftShiftU32: OperationI32(ref stackPointer, (a, b) => (int)((uint)a << b)); break;
                    case Opcode.RightShiftI32: OperationI32(ref stackPointer, (a, b) => a >> b); break;
                    case Opcode.RightShiftU32: OperationI32(ref stackPointer, (a, b) => (int)((uint)a >> b)); break;

                    // Int operations (a, b) -> v
                    case Opcode.AddI32: OperationI32(ref stackPointer, (a, b) => a + b); break;
                    case Opcode.SubtractI32: OperationI32(ref stackPointer, (a, b) => a - b); break;
                    case Opcode.MultiplyI32: OperationI32(ref stackPointer, (a, b) => a * b); break;
                    case Opcode.DivideI32: OperationI32(ref stackPointer, (a, b) => a / b); break;

                    // Float operations (a, b) -> v
                    case Opcode.AddF32: OperationF32(ref stackPointer, (a, b) => a + b); break;
                    case Opcode.SubtractF32: OperationF32(ref stackPointer, (a, b) => a - b); break;
                    case Opcode.MultiplyF32: OperationF32(ref stackPointer, (a, b) => a * b); break;
                    case Opcode.DivideF32: OperationF32(ref stackPointer, (a, b) => a / b); break;

                    // Type cast (a) -> v
                    case Opcode.CastI32ToF32: Push((float)ReadInt32(ref stackPointer), ref stackPointer); break;
                    case Opcode.CastF32ToI32: Push((int)ReadSingle(ref stackPointer), ref stackPointer); break;


                    // Jump (relative) -> 
                    case Opcode.Jump16:
                        {
                            var address = ReadInt16(ref instructionPointer);
                            instructionPointer += address;
                            break;
                        }
                    // Jump if (value, relative) ->
                    case Opcode.Jump16IfZeroI32: Jump16IfI32(a => a == 0); break;
                    case Opcode.Jump16IfPositiveI32: Jump16IfI32(a => a > 0); break;
                    case Opcode.Jump16IfNegativeI32: Jump16IfI32(a => a < 0); break;

                    // Jump compare (a, b, relative) ->
                    case Opcode.Jump16CompareEqualI32: Jump16CompareI32((a, b) => a == b); break;
                    case Opcode.Jump16CompareNotEqualI32: Jump16CompareI32((a, b) => a != b); break;
                    case Opcode.Jump16CompareGreaterI32: Jump16CompareI32((a, b) => a > b); break;
                    case Opcode.Jump16CompareLesserI32: Jump16CompareI32((a, b) => a < b); break;
                    case Opcode.Jump16CompareGreaterOrEqualI32: Jump16CompareI32((a, b) => a >= b); break;
                    case Opcode.Jump16CompareLesserOrEqualI32: Jump16CompareI32((a, b) => a <= b); break;

                    // Local [relative] (value) -> .
                    case Opcode.Local16Load32:
                        {
                            var address = stackPointer + ReadInt32(ref instructionPointer);
                            Push(ReadInt32(ref address), ref stackPointer);
                            break;
                        }
                    case Opcode.Local16Load64:
                        {
                            var address = stackPointer + ReadInt32(ref instructionPointer);
                            Push(ReadInt64(ref address), ref stackPointer);
                            break;
                        }
                    case Opcode.Local16Store32:
                        {
                            var address = stackPointer + ReadInt32(ref instructionPointer);
                            Set(ReadInt64(ref stackPointer), address);
                            break;
                        }

                    // Global (value, address) -> .
                    case Opcode.GlobalLoad32:
                        {
                            var address = ReadInt32(ref stackPointer);
                            Push(ReadInt32(ref address), ref stackPointer);
                            break;
                        }
                    case Opcode.GlobalStore32:
                        {
                            var address = ReadInt32(ref stackPointer);
                            Set(ReadInt64(ref stackPointer), address);
                            break;
                        }

                    // Const [a] -> v
                    case Opcode.Const32:
                        {
                            Push(ReadInt32(ref instructionPointer), ref stackPointer);
                            break;
                        }
                    case Opcode.Const64:
                        {
                            Push(ReadInt64(ref instructionPointer), ref stackPointer);
                            break;
                        }

                    // Print (a) -> .
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

        private void Jump16IfI32(Predicate<int> predicate)
        {
            var address = ReadInt16(ref instructionPointer);
            var value = ReadInt32(ref stackPointer);

            if (predicate(value))
            {
                instructionPointer += address;
            }
        }

        private void Jump16CompareI32(Func<int, int, bool> predicate)
        {
            var address = ReadInt16(ref instructionPointer);
            var latter = ReadInt32(ref stackPointer);
            var former = ReadInt32(ref stackPointer);

            if (predicate(former, latter))
            {
                instructionPointer += address;
            }
        }


        private void OperationI32(ref int pointer, Func<int, int> operation)
        {
            Push(operation(ReadInt32(ref stackPointer)), ref stackPointer);
        }

        private void OperationI32(ref int pointer, Func<int, int, int> operation)
        {
            var latter = ReadInt32(ref stackPointer);
            var former = ReadInt32(ref stackPointer);
            Push(operation(former, latter), ref stackPointer);
        }

        private void OperationU32(ref int pointer, Func<uint, uint, uint> operation)
        {
            var latter = ReadUInt32(ref stackPointer);
            var former = ReadUInt32(ref stackPointer);
            Push(operation(former, latter), ref stackPointer);
        }

        private void OperationF32(ref int pointer, Func<float, float> operation)
        {
            Push(operation(ReadSingle(ref stackPointer)), ref stackPointer);
        }

        private void OperationF32(ref int pointer, Func<float, float, float> operation)
        {
            var latter = ReadSingle(ref stackPointer);
            var former = ReadSingle(ref stackPointer);
            Push(operation(former, latter), ref stackPointer);
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

        private uint ReadUInt32(ref int pointer)
        {
            pointer += sizeof(int);
            return ToUInt32(Memory, pointer - sizeof(int));
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

        private void Push(uint value, ref int pointer)
        {
            pointer -= sizeof(uint);
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(uint));
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





        private void Set(int value, int pointer)
        {
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(int));
        }

        private void Set(long value, int pointer)
        {
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(long));
        }

        private void Set(uint value, int pointer)
        {
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(uint));
        }

        private void Set(float value, int pointer)
        {
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(float));
        }

        private void Set(double value, int pointer)
        {
            Buffer.BlockCopy(GetBytes(value), 0, Memory, pointer, sizeof(double));
        }
    }
}