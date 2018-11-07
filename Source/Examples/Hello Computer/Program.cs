using Axiverse.Computing.VirtualMachine;
using System;
using System.IO;

namespace Hello_Computer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var machine = new Machine();

            var stream = new MemoryStream(machine.Memory);
            var writer = new BinaryWriter(stream);

            writer.Write(Opcode.Const);
            writer.Write(1);
            writer.Write(Opcode.Const);
            writer.Write(10);
            writer.Write(Opcode.Add);
            writer.Write(Opcode.Print);
            writer.Write(Opcode.Halt);

            machine.Go();

            Console.ReadKey();
        }

        static void Write(this BinaryWriter writer, Opcode opcode)
        {
            writer.Write((byte)opcode);
        }
    }
}
