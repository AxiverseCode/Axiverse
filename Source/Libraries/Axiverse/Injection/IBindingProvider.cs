using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    public interface IBindingProvider
    {
        object this[Key key] { get; }
        bool ContainsKey(Key key);
        bool TryGetValue(Key key, out object value);
    }
}
