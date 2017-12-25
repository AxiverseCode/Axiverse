using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class FileSystem
    {
        private readonly List<Mount> mounts = new List<Mount>();

        public bool Exists(string path)
        {
            foreach (var mount in mounts)
            {
                if (mount.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        public Stream Open(string path, FileMode mode)
        {
            foreach(var mount in mounts)
            {
                if (mount.Exists(path))
                {
                    return mount.Open(path, mode);
                }
            }
            return null;
        }

        public String ReadAll(string path)
        {
            using(var stream = Open(path, FileMode.Open))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return null;
        }

        public void Mount(Mount mount)
        {
            mounts.Add(mount);
        }

        public bool Unmount(Mount mount)
        {
            return mounts.Remove(mount);
        }

        
        public static readonly FileSystem Default = new FileSystem();
    }
}
