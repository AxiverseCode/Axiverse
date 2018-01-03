using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// A virtual directory or node in the virtual file system.
    /// </summary>
    public interface IStoreNode
    {
        /// <summary>
        /// Gets the owning store for this blob.
        /// </summary>
        Store Store { get; }

        /// <summary>
        /// Gets the parent node containing this blobl.
        /// </summary>
        IStoreNode Parent { get; }

        /// <summary>
        /// Gets the collection of child nodes under this node.
        /// </summary>
        ICollection<IStoreNode> Children { get; }

        /// <summary>
        /// Gets the collection of blobs under this node.
        /// </summary>
        ICollection<IStoreBlob> Blobs { get; }

        /// <summary>
        /// Gets the full path of this blob.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether this node exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets whether this node can be read.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Gets whether this node can be written.
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// Gets the time when this node was created.
        /// </summary>
        DateTime CreatedTime { get; }

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        DateTime UpdatedTime { get; }

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        DateTime AccessedTime { get; }
    }
}
