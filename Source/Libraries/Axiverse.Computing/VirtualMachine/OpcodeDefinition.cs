using System;
using System.Collections.Generic;

namespace Axiverse.Computing.VirtualMachine
{
    public class OpcodeDefinition
    {
        public Opcode Opcode { get; }
        public int Take { get; }
        public int Give { get; }
        public Type[] Params { get; }

        private OpcodeDefinition(Opcode opcode, Type[] @params, int take = 0, int give = 0)
        {
            Opcode = opcode;
            Params = @params;
            Take = take;
            Give = give;
        }


        static OpcodeDefinition()
        {
            Register(Opcode.Nop);
            Register(Opcode.Halt);

            Register(Opcode.Call16, typeof(short));
            Register(Opcode.Call32, typeof(int));

            Register(Opcode.Return);
            Register(Opcode.Return32);
            Register(Opcode.Return64);

            Register(Opcode.AddI32, 2, 1);
            Register(Opcode.SubtractI32, 2, 1);
            Register(Opcode.MultiplyI32, 2, 1);
            Register(Opcode.DivideI32, 2, 1);

            Register(Opcode.Jump16, typeof(short));
            Register(Opcode.Jump16IfZeroI32, typeof(short));
            Register(Opcode.Jump16IfNotZeroI32, typeof(short));
            Register(Opcode.Jump16IfPositiveI32, typeof(short));
            Register(Opcode.Jump16IfNegativeI32, typeof(short));

            Register(Opcode.Jump16CompareEqualI32, typeof(short));
            Register(Opcode.Jump16CompareNotEqualI32, typeof(short));
            Register(Opcode.Jump16CompareGreaterI32, typeof(short));
            Register(Opcode.Jump16CompareLesserI32, typeof(short));
            Register(Opcode.Jump16CompareGreaterOrEqualI32, typeof(short));
            Register(Opcode.Jump16CompareLesserOrEqualI32, typeof(short));

            Register(Opcode.Local16Load32, typeof(short));
            Register(Opcode.Local16Load64, typeof(short));
            Register(Opcode.Local16Store32, typeof(short));
            Register(Opcode.Local16Store64, typeof(short));


            Register(Opcode.Const32, typeof(int));
            Register(Opcode.Const64, typeof(long));

            Register(Opcode.Print, 1, 0);
            Register(Opcode.Debug);
        }

        private static void Register(Opcode opcode, int take, int give, params Type[] @params)
        {
            definitions.Add(opcode, new OpcodeDefinition(opcode, @params));
        }

        private static void Register(Opcode opcode, params Type[] @params)
        {
            definitions.Add(opcode, new OpcodeDefinition(opcode, @params));
        }

        public static int Stride(Opcode opcode)
        {
            var stride = sizeof(Opcode);
            foreach (var t in For(opcode).Params)
            {
                if (t == typeof(byte))
                {
                    stride += sizeof(byte);
                }
                else if (t == typeof(short))
                {
                    stride += sizeof(short);
                }
                else if (t == typeof(int))
                {
                    stride += sizeof(int);
                }
                else if (t == typeof(long))
                {
                    stride += sizeof(long);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return stride;
        }

        public static OpcodeDefinition For(Opcode opcode)
        {
            return definitions[opcode];
        }

        private static readonly Dictionary<Opcode, OpcodeDefinition> definitions =
            new Dictionary<Opcode, OpcodeDefinition>();
    }
}
