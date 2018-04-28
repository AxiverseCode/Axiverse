using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    [Flags]
    public enum ResourceState
    {
        Common = ResourceStates.Common,
        Present = ResourceStates.Present,
        VertexAndConstantBuffer = 1,
        IndexBuffer = 2,
        RenderTarget = ResourceStates.RenderTarget,
        UnorderedAccess = 8,
        DepthWrite = 16,
        DepthRead = 32,
        NonPixelShaderResource = 64,
        PixelShaderResource = 128,
        StreamOut = 256,
        IndirectArgument = 512,
        Predication = 512,
        CopyDestination = 1024,
        CopySource = 2048,
        GenericRead = 2755,
        ResolveDestination = 4096,
        ResolveSource = 8192,
    }
}
