using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class RootSignature : GraphicsResource
    {
        public SharpDX.Direct3D12.RootSignature NativeRootSignature;

        public RootSignature(GraphicsDevice device, SharpDX.Direct3D12.RootSignature rootSignature) : base(device)
        {
            NativeRootSignature = rootSignature;
        }

        public static RootSignature Create(GraphicsDevice device)
        {
            // NOTE: I think we could work with prebaked root signatures (we can define it
            // as an HLSL shader and then use it for all of our PSOs. 
            // Root signature
            var rootSignatureDesc = new SharpDX.Direct3D12.RootSignatureDescription(SharpDX.Direct3D12.RootSignatureFlags.AllowInputAssemblerInputLayout);
            var rootSignature = device.NativeDevice.CreateRootSignature(rootSignatureDesc.Serialize());

            return new RootSignature(device, rootSignature);
        }
    }
}
