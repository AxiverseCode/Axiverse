using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;
using NativeRootSignature = SharpDX.Direct3D12.RootSignature;

namespace Axiverse.Interface.Graphics
{
    public class RootSignature : GraphicsResource
    {
        internal NativeRootSignature NativeRootSignature;

        internal RootSignature(GraphicsDevice device, NativeRootSignature rootSignature) : base(device)
        {
            NativeRootSignature = rootSignature;
        }

        protected override void Dispose(bool disposing)
        {
            NativeRootSignature.Dispose();
            base.Dispose(disposing);
        }

        public static RootSignature Create(GraphicsDevice device)
        {
            // NOTE: I think we could work with prebaked root signatures (we can define it
            // as an HLSL shader and then use it for all of our PSOs. 
            // Root signature
            var description = new RootSignatureDescription(RootSignatureFlags.AllowInputAssemblerInputLayout);
            var rootSignature = device.NativeDevice.CreateRootSignature(description.Serialize());

            return new RootSignature(device, rootSignature);
        }

        [Obsolete]
        internal static RootSignature Create(GraphicsDevice device, RootSignatureDescription description)
        {
            var rootSignature = device.NativeDevice.CreateRootSignature(description.Serialize());
            return new RootSignature(device, rootSignature);
        }
    }
}
