using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class MemoryStore
    {
        /// <summary>
        /// Gets the Uri schema for the store.
        /// </summary>
        public string Schema { get => "memory"; }

        /// <summary>
        /// Gets the root path for the store.
        /// </summary>
        public Uri Root => new Uri("memory:");
        
        readonly Dictionary<Uri, Stream> stores = new Dictionary<Uri, Stream>();

        class MemoryDirectory
        {
            public readonly Dictionary<string, MemoryDirectory> directories = new Dictionary<string, MemoryDirectory>();
            public readonly Dictionary<string, Stream> streams = new Dictionary<string, Stream>();
        }
    }
}
