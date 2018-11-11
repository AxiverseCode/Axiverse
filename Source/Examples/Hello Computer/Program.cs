using Axiverse.Computing.VirtualMachine;
using System;
using System.IO;

namespace Hello_Computer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var main = new FunctionBlock("main");
            main.Emit(Opcode.Const, 1);
            main.Emit(Opcode.Const, 10);
            main.Emit(Opcode.AddI32);
            main.Emit(Opcode.Print)
                ;
            main.Emit(Opcode.Halt);

            // factorial(i)
            var factorial = new FunctionBlock("factorial");
            // if (i == 1) return 1;
            factorial.Emit(Opcode.Load32, -16); // i
            factorial.Emit(Opcode.Const, 1);
            factorial.EmitLabel(Opcode.JumpCompareNotEqual, "recurse");
            factorial.Emit(Opcode.Const, 1);
            factorial.Emit(Opcode.Return32);

            // call factorial(i - 1)
            factorial.MarkLabel("recurse");
            factorial.Emit(Opcode.Load32, -16); // i
            factorial.Emit(Opcode.Const, 1);
            factorial.Emit(Opcode.SubtractI32);
            factorial.EmitCall("factorial");
            // return i * result;
            factorial.Emit(Opcode.Load32, -16); // i
            factorial.Emit(Opcode.MultiplyI32);
            factorial.Emit(Opcode.Return32);


            var linker = new Linker();
            var bytecode = linker.Link(main);

            var machine = new ExecutionEngine(bytecode);
            machine.Go();

            Console.ReadKey();
        }

        static void Write(this BinaryWriter writer, Opcode opcode)
        {
            writer.Write((byte)opcode);
        }
    }
}
