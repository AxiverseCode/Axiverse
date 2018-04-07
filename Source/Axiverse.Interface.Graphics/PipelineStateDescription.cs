using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class PipelineStateDescription
    {
        public InputLayoutDescription InputLayout { get; set; }
        public RootSignature RootSignature { get; set; }
        public ShaderBytecode VertexShader { get; set; }
        public ShaderBytecode PixelShader { get; set; }
    }
}
