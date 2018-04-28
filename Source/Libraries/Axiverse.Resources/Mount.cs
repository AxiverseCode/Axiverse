using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class Mount
    {
        public string BasePath { get; }

        public string[] GetFiles(string path, string blob)
        {
            return Directory.GetFiles(Path.Combine(BasePath, path), blob).Select(s => s.Replace(BasePath, "")).ToArray();
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(Path.Combine(BasePath, path)).Select(s => s.Replace(BasePath, "")).ToArray();
        }

        public bool Exists(string path)
        {
            return File.Exists(Path.Combine(BasePath, path));
        }

        public Stream Open(string path, FileMode mode)
        {
            return File.Open(Path.Combine(BasePath, path), mode);
        }

        public Mount(string basePath)
        {
            BasePath = Path.GetFullPath(basePath) + @"\";
        }
    }
}
