using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// A store mount backed by the local file system.
    /// </summary>
    public class FileSystemMount
    {
        /// <summary>
        /// The root local file system path for this mount.
        /// </summary>
        public string SystemPath { get; }

        /// <summary>
        /// Gets the root virtual node for this mount.
        /// </summary>
        public FileSystemNode MountRoot { get; }

        /// <summary>
        /// Constructs a mount.
        /// </summary>
        /// <param name="systemPath">The root local file system path for this mount.</param>
        public FileSystemMount(string systemPath)
        {
            Contract.Requires<IOException>(Directory.Exists(systemPath));

            SystemPath = systemPath;
            MountRoot = new FileSystemNode(this, "/");
        }

        /// <summary>
        /// Gets the local file system path for a relative mount path.
        /// </summary>
        /// <param name="mountPath">The relative path the the mount root.</param>
        /// <returns></returns>
        public string GetSystemPath(string mountPath)
        {
            return Path.Combine(SystemPath, mountPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mountPath"></param>
        /// <returns></returns>
        public string GetFullPath(string mountPath)
        {
            throw new NotImplementedException();
            return Path.Combine(mountPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemPath"></param>
        /// <returns></returns>
        public string GetMountPath(string systemPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of child nodes from a mount path.
        /// </summary>
        /// <param name="mountPath"></param>
        /// <returns></returns>
        public ICollection<IStoreNode> GetDirectories(string mountPath)
        {
            List<IStoreNode> directories = new List<IStoreNode>();
            foreach (string path in Directory.GetDirectories(GetSystemPath(mountPath)))
            {
                if (!nodes.TryGetValue(path, out var node))
                {
                    node = new FileSystemNode(this, GetMountPath(path));
                    nodes.Add(path, node);
                }
                directories.Add(node);
            }
            return directories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mountPath"></param>
        /// <returns></returns>
        public ICollection<IStoreBlob> GetFiles(string mountPath)
        {
            List<IStoreBlob> directories = new List<IStoreBlob>();
            foreach (string path in Directory.GetFiles(GetSystemPath(mountPath)))
            {
                if (!blobs.TryGetValue(path, out var blob))
                {
                    blob = new FileSystemBlob(this, GetMountPath(path));
                    blobs.Add(path, blob);
                }
                directories.Add(blob);
            }
            return directories;
        }

        public IStoreNode GetDirectory(string mountPath)
        {
            var path = GetSystemPath(mountPath);

            if (!nodes.TryGetValue(path, out var node))
            {
                node = new FileSystemNode(this, GetMountPath(path));
                nodes.Add(path, node);
            }

            return node;
        }

        public IStoreBlob GetFile(string mountPath)
        {
            var path = GetSystemPath(mountPath);

            if (!blobs.TryGetValue(path, out var blob))
            {
                blob = new FileSystemBlob(this, GetMountPath(path));
                blobs.Add(path, blob);
            }

            return blob;
        }

        readonly Dictionary<string, FileSystemNode> nodes = 
            new Dictionary<string, FileSystemNode>();

        readonly Dictionary<string, FileSystemBlob> blobs =
            new Dictionary<string, FileSystemBlob>();
    }
}
