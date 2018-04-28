using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// Represents a virtual file system.
    /// </summary>
    public class Store
    {
        private readonly Dictionary<string, Mount> mounts = new Dictionary<string, Resources.Mount>();

        private readonly List<Mount> mountList = new List<Mount>();

        /// <summary>
        /// Gets the root node in the store.
        /// </summary>
        public IStoreNode Root { get; set; }

        // GetFiles
        // GetDirectory
        // DoesFileExists
        // OpenStream

        // Archive
        // Node
        // Entry

        // GetNodes
        // GetEntries
        // CreateEntries

        /// <summary>
        /// Gets all the entries within a node.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFiles(string path, string blob)
        {
            List<String> files = new List<string>();
            foreach (var mount in mountList)
            {
                files.AddRange(mount.GetFiles(path, blob));
            }
            return files.ToArray();
        }

        /// <summary>
        /// Gets all the entries within a node.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetDirectories(string path)
        {
            List<String> files = new List<string>();
            foreach (var mount in mountList)
            {
                files.AddRange(mount.GetDirectories(path));
            }
            return files.ToArray();
        }

        /// <summary>
        /// Gets whether a path exists within archive.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            foreach (var mount in mountList)
            {
                if (mount.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Opens the specified entry.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Stream Open(string path, FileMode mode)
        {
            foreach(var mount in mountList)
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

        public byte[] ReadAllBytes(string path)
        {
            using (var stream = Open(path, FileMode.Open))
            {
                if (stream != null)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    return buffer;
                }
            }
            return null;
        }

        public void Mount(Mount mount)
        {
            mountList.Add(mount);
        }

        public bool Unmount(Mount mount)
        {
            return mountList.Remove(mount);
        }

        
        public static readonly Store Default = new Store();
    }
}
