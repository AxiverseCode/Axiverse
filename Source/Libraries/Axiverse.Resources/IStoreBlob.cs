using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// A virtual file or blob in the virtual file system
    /// </summary>
    public interface IStoreBlob
    {
        /// <summary>
        /// Gets the owning store for this blob.
        /// </summary>
        Store Store { get; }

        /// <summary>
        /// Gets the parent node containing this blobl.
        /// </summary>
        IStoreNode Node { get; }

        /// <summary>
        /// Gets the full path of this blob.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Gets the name of this blob with the extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the extension of this blob.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets whether this blob exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets whether this blob can be read.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Gets whether this blob can be written.
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// Gets the time when this blob was created.
        /// </summary>
        DateTime CreatedTime { get; }

        /// <summary>
        /// Gets the time when this blob was last update.
        /// </summary>
        DateTime UpdatedTime { get; }

        /// <summary>
        /// Gets the time when this blob was last update.
        /// </summary>
        DateTime AccessedTime { get; }
    }
}
