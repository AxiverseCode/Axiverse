using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// Emitter represents a single function.
    /// </summary>
    /// <remarks>
    /// The first 4 bytes of a function block includes metadata.
    /// 1: Argument count
    /// 2: Local count
    /// 3: Stack usage
    /// </remarks>
    public class FunctionBlock
    {

        private readonly BinaryWriter writer;

        /// <summary>
        /// Gets the stream where the function is being written.
        /// </summary>
        public MemoryStream Buffer { get; } = new MemoryStream();

        /// <summary>
        /// Gets the memory addresses where calls are made.
        /// </summary>
        public Dictionary<int, string> Calls { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the labels and their memory addresses.
        /// </summary>
        public Dictionary<string, int> Labels { get; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets the relative instructions.
        /// </summary>
        public Dictionary<int, string> Relative { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the name of the function block.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the byte length of the function block.
        /// </summary>
        public int Length { get; private set; }

        public FunctionBlock(string name)
        {
            Name = name;
            writer = new BinaryWriter(Buffer);

            // Metadata bytes.
            Length = 4;
        }

        public void Emit(Opcode opcode)
        {
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            Length = (int)Buffer.Position;
        }

        public void Emit(Opcode opcode, byte param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(byte) }));

        }

        public void Emit(Opcode opcode, short param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(short) }));
        }

        public void Emit(Opcode opcode, int param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(int) }));
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write(param1);
            Length = (int)Buffer.Position;
        }

        /// <summary>
        /// Write a offset with the offset marked as a label. Use set label later to change that
        /// label into an absolute offset.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="label"></param>
        public void EmitLabel(Opcode opcode, string label)
        {

        }

        public void EmitCall(string name)
        {
            Buffer.Position = Length;
            writer.Write((byte)Opcode.Call16);

            Calls.Add((int)Buffer.Position, name);
            writer.Write((short)0x0bad);

            Length = (int)Buffer.Position;
        }

        /// <summary>
        /// Marks the current location to the label.
        /// </summary>
        /// <param name="label"></param>
        public void MarkLabel(string label)
        {

        }

        public void Link(int offset, int address)
        {
            Buffer.Seek(offset, SeekOrigin.Begin);
            writer.Write(address);
        }

    }
}
