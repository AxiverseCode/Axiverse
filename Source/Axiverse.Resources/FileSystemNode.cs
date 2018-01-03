using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// A store node backed by the local file system.
    /// </summary>
    public class FileSystemNode : IStoreNode
    {
        public Store Store => throw new NotImplementedException();

        public IStoreNode Parent => throw new NotImplementedException();

        public ICollection<IStoreNode> Children => throw new NotImplementedException();

        public ICollection<IStoreBlob> Blobs => throw new NotImplementedException();

        public string FullPath => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public bool CanRead => throw new NotImplementedException();

        public bool CanWrite => throw new NotImplementedException();

        public DateTime CreatedTime => throw new NotImplementedException();

        public DateTime UpdatedTime => throw new NotImplementedException();

        public DateTime AccessedTime => throw new NotImplementedException();
    }
}
