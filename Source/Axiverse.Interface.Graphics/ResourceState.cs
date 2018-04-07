﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    [Flags]
    public enum ResourceState
    {
        Common = 0,
        Present = 0,
        VertexAndConstantBuffer = 1,
        IndexBuffer = 2,
        RenderTarget = 4,
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
