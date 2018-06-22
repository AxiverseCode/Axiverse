using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    public abstract class DisposableObjectPool<T> : ObjectPool<T>, ITrackedDisposable
        where T : class, IDisposable
    {
        public override T Take()
        {
            Requires.IsNotDisposed(this);
            return base.Take();
        }

        public bool IsDisposed { get; private set; }

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
        
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
