using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// An concurrent object pool for disposable objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DisposableObjectPool<T> : ObjectPool<T>, ITrackedDisposable
        where T : class, IDisposable
    {
        /// <summary>
        /// Takes an object from the pool if there are any, otherwise returns a new object.
        /// </summary>
        /// <returns></returns>
        public override T Take()
        {
            Requires.IsNotDisposed(this);
            return base.Take();
        }

        /// <summary>
        /// Gets whether this <see cref="DisposableObjectPool{T}"/> has already been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes all objects in the object pool.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    while (Count > 0)
                    {
                        Take().Dispose();
                    }
                }
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Disposes all objects in the object pool.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
