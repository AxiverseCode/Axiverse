using System;
using System.Collections.Generic;

namespace Axiverse.Computing.VirtualMachine
{
    public class OpcodeDefinition
    {
        public Opcode Opcode { get; }
        public Type[] Params { get; }

        private OpcodeDefinition(Opcode opcode, Type[] @params)
        {
            Opcode = opcode;
            Params = @params;
        }


        static OpcodeDefinition()
        {
            Register(Opcode.AddI32);
            Register(Opcode.Print);
            Register(Opcode.Halt);
            Register(Opcode.Const32, typeof(int));
            Register(Opcode.Call16, typeof(short));
            Register(Opcode.Call32, typeof(int));
            Register(Opcode.Jump, typeof(short));
        }

        private static void Register(Opcode opcode, params Type[] @params)
        {
            definitions.Add(opcode, new OpcodeDefinition(opcode, @params));
        }

        public static OpcodeDefinition For(Opcode opcode)
        {
            return definitions[opcode];
        }

        private static readonly Dictionary<Opcode, OpcodeDefinition> definitions =
            new Dictionary<Opcode, OpcodeDefinition>();
    }
}
