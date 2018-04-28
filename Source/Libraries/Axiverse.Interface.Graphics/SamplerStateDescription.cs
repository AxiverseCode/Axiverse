using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class SamplerStateDescription
    {
        public TextureAddressMode AddressU;
        public TextureAddressMode AddressV;
        public TextureAddressMode AddressW;
        public FilterType Filter;
    }
}
