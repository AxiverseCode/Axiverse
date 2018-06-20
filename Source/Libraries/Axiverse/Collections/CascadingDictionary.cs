using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public class CascadingDictionary<TKey, TValue>
    {
        public IDictionary<TKey, TValue> Base { get; set; }
    }
}
