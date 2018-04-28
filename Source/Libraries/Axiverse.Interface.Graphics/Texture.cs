using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class Texture : GraphicsResource
    {
        internal CpuDescriptorHandle NativeRenderTargetView;
        internal CpuDescriptorHandle NativeDepthStencilView;

        /// <summary>
        /// Gets the dimensions of the texture.
        /// </summary>
        public int Dimensions { get; private set; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the texture. This will always be 1 for 1D textures.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the depth of the texture. This will always be 1 for 1D and 2D textures.
        /// </summary>
        public int Depth { get; private set; }

        public Texture(GraphicsDevice device) : base(device)
        {

        }
    }
}
