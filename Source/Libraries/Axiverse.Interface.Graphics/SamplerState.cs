using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

using NativeSamplerStateDescription = SharpDX.Direct3D12.SamplerStateDescription;

namespace Axiverse.Interface.Graphics
{
    public class SamplerState : GraphicsResource
    {
        internal CpuDescriptorHandle NativeSampler;

        private SamplerState(GraphicsDevice device) : base(device)
        {

        }

        private void Initialize(SamplerStateDescription description)
        {
            var nativeDescription = new NativeSamplerStateDescription
            {
                Filter = Filter.ComparisonAnisotropic,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MinimumLod = float.MinValue,
                MaximumLod = float.MaxValue,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Never
            };

            NativeSampler = Device.SamplerAllocator.Allocate(1);
            Device.NativeDevice.CreateSampler(ref nativeDescription, NativeSampler);
        }

        public static SamplerState Create(GraphicsDevice device, SamplerStateDescription description)
        {
            var result = new SamplerState(device);
            result.Initialize(description);
            return result;
        }

    }
}
