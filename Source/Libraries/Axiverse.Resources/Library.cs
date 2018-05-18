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

        public Library(string basePath)
        {
            BasePath = basePath;
        }
    }
}
