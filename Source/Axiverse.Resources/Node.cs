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
    public class Node
    {
        /// <summary>
        /// Gets the owning store for this blob.
        /// </summary>
        public Store Store { get; private set; }

        /// <summary>
        /// Gets the parent node containing this blobl.
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// Gets the full path of this blob.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets whether this node exists.
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Gets whether this node can be read.
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets whether this node can be written.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Gets the time when this node was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets the time when this node was last update.
        /// </summary>
        public DateTime AccessedTime { get; set; }
    }
}
