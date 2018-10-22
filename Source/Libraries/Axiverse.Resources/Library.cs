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
        public string BasePath { get; set; }
        
        public Stream OpenRead(string path)
        {
            return File.OpenRead(GetPath(path));
        }

        protected string GetPath(string path)
        {
            return Path.Combine(BasePath, path);
        }

        public Library()
        {

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

        public Library(string basePath)
        {
            BasePath = basePath;
        }
    }
}
