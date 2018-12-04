using Axiverse.Computing.VirtualMachine;
using System;
using System.IO;

namespace Hello_Computer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var main = new FunctionBuilder("main");
            main.Emit(Opcode.Const32, 1);
            main.Emit(Opcode.Const32, 10);
            main.Emit(Opcode.AddI32);
            main.Emit(Opcode.Print);
            main.Emit(Opcode.Const32, 10);
            main.EmitCall("factorial");
            main.Emit(Opcode.Print);
            main.Emit(Opcode.Return);

            // factorial(i)
            var factorial = new FunctionBuilder("factorial");
            factorial.Parameters = 4;
            // if (i == 1) return 1;
            factorial.Emit(Opcode.Local16Load32, 12); // i
            factorial.Emit(Opcode.Const32, 1);
            factorial.EmitLabel(Opcode.Jump16CompareNotEqualI32, "recurse");
            factorial.Emit(Opcode.Const32, 1);
            factorial.Emit(Opcode.Return32);

            // call factorial(i - 1)
            factorial.MarkLabel("recurse");
            factorial.Emit(Opcode.Local16Load32, 12); // i
            factorial.Emit(Opcode.Const32, 1);
            factorial.Emit(Opcode.SubtractI32);
            factorial.EmitCall("factorial");
            // return i * result;
            factorial.Emit(Opcode.Local16Load32, 12); // i
            factorial.Emit(Opcode.MultiplyI32);
            factorial.Emit(Opcode.Return32);


            var linker = new Linker();
            var bytecode = linker.Link(main, factorial);

            Console.WriteLine(Disassembler.Disassemble(bytecode));

            var machine = new Executor(bytecode);
            machine.Go();

            Console.ReadKey();
        }

        static void Write(this BinaryWriter writer, Opcode opcode)
        {
            writer.Write((byte)opcode);
        }
    }
}
