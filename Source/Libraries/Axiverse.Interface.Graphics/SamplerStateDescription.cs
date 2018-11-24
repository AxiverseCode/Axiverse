using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Description of <see cref="SamplerState"/>.
    /// </summary>
    public class SamplerStateDescription
    {
        /// <summary>
        /// Gets or sets the address u.
        /// </summary>
        public TextureAddressMode AddressU;

        /// <summary>
        /// Gets or sets the address v.
        /// </summary>
        public TextureAddressMode AddressV;

        /// <summary>
        /// Gets or sets the address w.
        /// </summary>
        public TextureAddressMode AddressW;

        /// <summary>
        /// Gets or sets the filter type.
        /// </summary>
        public FilterType Filter;
    }
}
