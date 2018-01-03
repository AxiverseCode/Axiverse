using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// A store blob backed by the local file system.
    /// </summary>
    public class FileSystemBlob : IStoreBlob
    {
        public Store Store => throw new NotImplementedException();

        public IStoreNode Node => throw new NotImplementedException();

        public string FullPath => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public bool CanRead => throw new NotImplementedException();

        public bool CanWrite => throw new NotImplementedException();

        public DateTime CreatedTime => throw new NotImplementedException();

        public DateTime UpdatedTime => throw new NotImplementedException();

        public DateTime AccessedTime => throw new NotImplementedException();
    }
}
