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
    public class FunctionBuilder
    {

        private readonly BinaryWriter writer;
        private readonly MemoryStream buffer = new MemoryStream();

        /// <summary>
        /// Gets the stream where the function is being written.
        /// </summary>
        public MemoryStream Buffer
        {
            get
            {
                return buffer;
            }
        }

        /// <summary>
        /// Gets the memory addresses where calls are made.
        /// </summary>
        public Dictionary<int, string> Calls { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the labels and their memory addresses.
        /// </summary>
        public Dictionary<int, string> Labels { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the relative instructions.
        /// </summary>
        public Dictionary<int, string> Relative { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets the name of the function block.
        /// </summary>
        public string Name { get; }

        public int Address { get; set; }

        public int CallOffset { get; } = 4;

        public byte Parameters
        {
            set
            {
                Buffer.Position = 3;
                writer.Write(value);
                writer.Flush();
            }
        }

        /// <summary>
        /// Gets the byte length of the function block.
        /// </summary>
        public int Length { get; private set; }

        public FunctionBuilder(string name)
        {
            Name = name;
            writer = new BinaryWriter(Buffer);
            writer.Write((byte)Opcode.Metadata);

            // Metadata bytes.
            Length = CallOffset;
        }

        public void Emit(Opcode opcode)
        {
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Flush();
            Length = (int)Buffer.Position;
        }

        public void Emit(Opcode opcode, byte param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(byte) }));
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write(param1);
            writer.Flush();
            Length = (int)Buffer.Position;
        }

        public void Emit(Opcode opcode, short param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(short) }));
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write(param1);
            writer.Flush();
            Length = (int)Buffer.Position;
        }

        public void Emit(Opcode opcode, int param1)
        {
            if (OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(short) }))
            {
                Emit(opcode, (short)param1);
                return;
            }

            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(int) }));
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write(param1);
            writer.Flush();
            Length = (int)Buffer.Position;
        }

        public void Emit(Opcode opcode, long param1)
        {
            Requires.That(OpcodeDefinition.For(opcode).Params.SequenceEqual(new Type[] { typeof(int) }));
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write(param1);
            writer.Flush();
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
            Labels.Add(Length, label);
            Buffer.Position = Length;
            writer.Write((byte)opcode);
            writer.Write((short)0x0bad);
            writer.Flush();
            Length = (int)Buffer.Position;
        }

        public void EmitCall(string name)
        {
            Buffer.Position = Length;
            writer.Write((byte)Opcode.Call16);

            Calls.Add((int)Buffer.Position, name);
            writer.Write((short)0x0bad);

            writer.Flush();
            Length = (int)Buffer.Position;
        }

        /// <summary>
        /// Marks the current location to the label.
        /// </summary>
        /// <param name="label"></param>
        public void MarkLabel(string label)
        {
            foreach (var item in Labels)
            {
                if (item.Value == label)
                {
                    var basePosition = item.Key;
                    var position = item.Key + 1;
                    Buffer.Position = position;
                    writer.Write((short)(Length - basePosition));
                    writer.Flush();
                }
            }
        }

        public void Link(int offset, int address)
        {
            var baseAddress = Address + offset - 1;
            Buffer.Position = offset - 1;
            var relative = checked(address - baseAddress);

            var opcode = (Opcode)buffer.ReadByte();

            Buffer.Seek(offset, SeekOrigin.Begin);
            writer.Write((short)relative);
            writer.Flush();
        }
    }
}
