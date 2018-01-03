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
        public string BasePath { get; set; }

        public string[] GetFiles(string path)
        {
            return Node.GetFiles(Path.Combine(BasePath, path));
        }

        public bool Exists(string path)
        {
            return File.Exists(Path.Combine(BasePath, path));
        }

        public Stream Open(string path, FileMode mode)
        {
            return File.Open(Path.Combine(BasePath, path), mode);
        }
    }
}
