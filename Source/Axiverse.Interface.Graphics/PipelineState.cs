using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    class PipelineState
    {
        internal SharpDX.Direct3D12.PipelineState CompiledState;
        internal SharpDX.Direct3D12.RootSignature RootSignature;
        internal PrimitiveTopology PrimitiveTopology;
    }
}
