using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    public interface ITrackedDisposable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
