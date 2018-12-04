using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Axiverse.Computing.VirtualMachine
{
    /// <summary>
    /// Links functions together.
    /// </summary>
    public class Linker
    {
        public byte[] Link(params FunctionBuilder[] emitters)
        {
            return Link((IEnumerable<FunctionBuilder>)emitters);
        }

        public byte[] Link(IEnumerable<FunctionBuilder> emitters)
        {
            // goes through all the emitters, lays them out in memory and then links them to their
            // actual memory addresses.

            // first emitter is interpreted as main.

            var totalLength = emitters.Sum(e => e.Length);
            var memoryStream = new MemoryStream(totalLength);
            var addresses = new Dictionary<string, int>();

            // layout all emitters
            var offset = OpcodeDefinition.Stride(Opcode.Jump16) + OpcodeDefinition.Stride(Opcode.Halt);
            var entry = emitters.First();

            foreach (var emitter in emitters)
            {
                var disassembly = Disassembler.Disassemble(emitter.Buffer.GetBuffer(), emitter.Length);
                //Console.WriteLine(disassembly);

                Console.WriteLine($"Linked {emitter.Name} @ {offset}");
                addresses.Add(emitter.Name, offset);
                emitter.Address = offset;
                offset += emitter.Length;
            }

            // set absolute addresses
            foreach (var emitter in emitters)
            {
                foreach (var call in emitter.Calls)
                {
                    emitter.Link(call.Key, addresses[call.Value]);
                }
            }

            // Write jump to entry function.
            memoryStream.WriteByte((byte)Opcode.Call16);
            memoryStream.Write(BitConverter.GetBytes((short)addresses[entry.Name]), 0, sizeof(short));
            memoryStream.WriteByte((byte)Opcode.Halt);

            // iterate through all emitters, and rewrite
            foreach (var emitter in emitters)
            {
                var disassembly = Disassembler.Disassemble(emitter.Buffer.GetBuffer(), emitter.Length);
                //Console.WriteLine(disassembly);
                Requires.That(addresses[emitter.Name] == memoryStream.Position);
                emitter.Buffer.Position = 0;
                emitter.Buffer.CopyTo(memoryStream, emitter.Length);
            }

            memoryStream.Position = 0;
            BinaryReader reader = new BinaryReader(memoryStream);
            return reader.ReadBytes((int)memoryStream.Length);
        }
    }
}
