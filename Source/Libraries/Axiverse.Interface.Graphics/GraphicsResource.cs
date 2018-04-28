using System;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// A resource bound to a <see cref="GraphicsDevice"/>.
    /// </summary>
    public class GraphicsResource : IDisposable
    {
        /// <summary>
        /// Gets the device the resource is bound to.
        /// </summary>
        public GraphicsDevice Device { get; }

        /// <summary>
        /// Gets whether the resource has already been disposed.
        /// </summary>
        public bool IsDisposed { get; protected set; }
        
        /// <summary>
        /// Constructs a resoruce bound to the specified device.
        /// </summary>
        /// <param name="device"></param>
        protected GraphicsResource(GraphicsDevice device)
        {
            Device = device;

            if (device != null)
            {
                device.Resources.Add(this);
            }
        }

        /// <summary>
        /// Disposes the resource.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resource.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            Device.Resources.Remove(this);
            IsDisposed = true;
        }
    }
}
