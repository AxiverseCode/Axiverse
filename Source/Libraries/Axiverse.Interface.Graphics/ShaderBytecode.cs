using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class ShaderBytecode
    {
        public byte[] Data { get; set; }

        public ShaderBytecode(byte[] data)
        {
            Data = data;
        }

        public static implicit operator byte[] (ShaderBytecode shaderBytecode)
        {
            return shaderBytecode.Data;
        }

        public static ShaderBytecode CompileFromFile(string path, string profile, string entryPoint)
        {
            return new ShaderBytecode(SharpDX.D3DCompiler.ShaderBytecode.CompileFromFile(path, profile, entryPoint));
        }
    }
}
