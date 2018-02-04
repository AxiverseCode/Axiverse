using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Engine.Rendering
{
    /// <summary>
    /// Is responsible for creating all other objects (textures, buffers, shaders, pipeline states, etc.)
    /// </summary>
    public class RenderDevice
    {
        /// <summary>
        /// Gets the native d3d device
        /// </summary>
        public Device NativeDevice { get; private set; }

        /// <summary>
        /// Initializes the GPU device
        /// </summary>
        public void Init()
        {
#if DEBUG
            DebugInterface.Get().EnableDebugLayer();
#endif
            NativeDevice = new Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);
        }

    }
}
