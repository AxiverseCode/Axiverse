using SharpDX.Direct3D12;
using System;
using NativeRootSignature = SharpDX.Direct3D12.RootSignature;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Root signature which defines the buffer and sampler bindings.
    /// </summary>
    public class RootSignature : GraphicsResource
    {
        internal NativeRootSignature NativeRootSignature;

        internal RootSignature(GraphicsDevice device, NativeRootSignature rootSignature) : base(device)
        {
            NativeRootSignature = rootSignature;
        }

        /// <summary>
        /// Disposes the root signature.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                NativeRootSignature.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a root signature.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
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
