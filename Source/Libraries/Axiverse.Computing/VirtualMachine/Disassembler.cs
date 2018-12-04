using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing.VirtualMachine
{
    public class Disassembler
    {
        public static string Disassemble(byte[] program)
        {
            return Disassemble(program, program.Length);
        }

        public static string Disassemble(byte[] program, int length)
        {
            StringBuilder builder = new StringBuilder();

            var position = 0;
            while (position < length)
            {
                var opcode = (Opcode)program[position];

                if (opcode == Opcode.Metadata)
                {
                    var m1 = program[position + 1];
                    var m2 = program[position + 2];
                    var m3 = program[position + 3];

                    builder.AppendFormat("\nFunction\n{0:D3}:\tMethod [Params: {3}]", position, m1, m2, m3);
                    position += 4;
                }
                else
                {
                    builder.AppendFormat("{0:D3}:\t{1, -16}", position, opcode);

                    position++;

                    foreach (Type t in OpcodeDefinition.For(opcode).Params)
                    {
                        if (t == typeof(byte))
                        {
                            builder.AppendFormat("{0:X2} ({0})\t", program[position++]);
                        }
                        else if (t == typeof(short))
                        {
                            builder.AppendFormat("{0:X4} ({0})\t", BitConverter.ToInt16(program, position));
                            position += sizeof(short);
                        }
                        else if (t == typeof(int))
                        {
                            builder.AppendFormat("{0:X8} ({0})\t", BitConverter.ToInt32(program, position));
                            position += sizeof(int);
                        }
                        else if (t == typeof(long))
                        {
                            builder.AppendFormat("{0:X16} ({0})\t", BitConverter.ToInt64(program, position));
                            position += sizeof(int);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }

                }
                builder.AppendLine();
            }


            return builder.ToString();
        }
    }
}
