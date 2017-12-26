using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public interface IGraphicsResource
    {
        void Prepare(GraphicsCommandList commandList);
        void Collect();
    }
}
