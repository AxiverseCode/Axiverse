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
    public class Blob
    {
        /// <summary>
        /// Gets the owning store for this blob.
        /// </summary>
        public Store Store { get; private set; }

        /// <summary>
        /// Gets the parent node containing this blobl.
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Gets the full path of this blob.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets the name of this blob with the extension.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the extension of this blob.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets whether this blob exists.
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Gets whether this blob can be read.
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets whether this blob can be written.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Gets the time when this blob was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets the time when this blob was last update.
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets the time when this blob was last update.
        /// </summary>
        public DateTime AccessedTime { get; set; }
    }
}
