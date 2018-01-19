using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// Gets the mount.
        /// </summary>
        public FileSystemMount Mount { get; }

        /// <summary>
        /// Gets the relative path to the mount root.
        /// </summary>
        public string MountPath { get; }

        /// <summary>
        /// Gets the local file system path of this blob.
        /// </summary>
        public string SystemPath => Mount.GetSystemPath(MountPath);

        /// <summary>
        /// Gets the owning store for this blob.
        /// </summary>
        public Store Store => Mount.MountRoot.Store;

        /// <summary>
        /// Gets the parent node containing this blob.
        /// </summary>
        public IStoreNode Parent => throw new NotImplementedException();

        /// <summary>
        /// Gets the collection of child nodes under this node.
        /// </summary>
        public ICollection<IStoreNode> Children => Mount.GetDirectories(MountPath);

        /// <summary>
        /// Gets the collection of blobs under this node.
        /// </summary>
        public ICollection<IStoreBlob> Blobs => Mount.GetFiles(MountPath);

        /// <summary>
        /// Gets the full path of this blob.
        /// </summary>
        public string FullPath => Mount.GetFullPath(MountPath);

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        public string Name => Path.GetFileName(SystemPath);

        /// <summary>
        /// Gets whether this node exists.
        /// </summary>
        public bool Exists => File.Exists(SystemPath);

        /// <summary>
        /// Gets whether this node can be read.
        /// </summary>
        public bool CanRead => true;

        /// <summary>
        /// Gets whether this node can be written.
        /// </summary>
        public bool CanWrite => true;

        /// <summary>
        /// Gets the time when this node was created.
        /// </summary>
        public DateTime CreatedTime => File.GetCreationTime(SystemPath);

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        public DateTime UpdatedTime => File.GetLastWriteTime(SystemPath);

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        public DateTime AccessedTime => File.GetLastAccessTime(SystemPath);

        /// <summary>
        /// Creates a node based on a local file system file.
        /// </summary>
        /// <param name="mount">The mount.</param>
        /// <param name="mountPath">Relative path to the mount root.</param>
        public FileSystemNode(FileSystemMount mount, string mountPath)
        {
            Mount = mount;
            MountPath = mountPath;
        }
    }
}
