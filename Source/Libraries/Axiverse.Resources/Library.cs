using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// Virtual file system for resources.
    /// </summary>
    /// <remarks>
    /// For every path, we use the store with the most specific matching path with the same schema.
    /// This means that some stores can shadow others.
    /// </remarks>
    public class Library
    {
        /// <summary>
        /// Gets the stores backing this library.
        /// </summary>
        public StoreCollection Stores { get; }

        /// <summary>
        /// Gets or sets the default scheme.
        /// </summary>
        public string DefaultScheme { get; set; } = "file";

        /// <summary>
        /// Constructs a library.
        /// </summary>
        public Library()
        {
            Stores = new StoreCollection(this);
        }
        
        /// <summary>
        /// Determines whether a directory exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool DirectoryExists(Uri path)
        {
            return false;
        }

        /// <summary>
        /// Determines whether a file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileExists(Uri path)
        {
            return false;
        }

        public Stream OpenRead(Uri uri)
        {
            return null;
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(GetPath(path));

        }
        
        protected string GetPath(string path)
        {
            return Path.Combine(BasePath, path);
        }


        // Path
        // - Get Files
        // - Get Directories
        // File
        // - Open
        // - Create
        // - Save
        // Cache
        // - GetOrLoad
        // - GetOnly
        // - Add
        // - Remove

        // How do we ensure unique mount paths? (most specific first?) - among the same scheme

        public string BasePath { get; set; }

        public Library(string basePath)
        {
            BasePath = basePath;
        }
    }
}
