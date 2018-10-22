using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class SystemStore
    {
        public string Schema => "file";

        private string systemRoot;
        private Uri mountRoot;

        /// <summary>
        /// Constructs a <see cref="SystemStore"/> for the given system directory at the specified
        /// mount root.
        /// </summary>
        /// <param name="systemRoot"></param>
        /// <param name="mountRoot"></param>
        public SystemStore(string systemRoot, string mountRoot)
        {
            Requires.That<DirectoryNotFoundException>(Directory.Exists(systemRoot));
            this.systemRoot = Path.GetFullPath(systemRoot);

            this.mountRoot = new Uri($"{Schema}:{mountRoot}");
        }

        /// <summary>
        /// Resolves a virtual path to a system path and ensures it is a child of the root
        /// directory.
        /// </summary>
        /// <param name="mountPath"></param>
        /// <returns></returns>
        public string Resolve(string mountPath)
        {
            Requires.That(mountPath.StartsWith(mountRoot.ToString()));
            var relativePath = mountPath.Substring(mountRoot.ToString().Length);
            var systemPath = Path.GetFullPath(Path.Combine(systemRoot, relativePath));

            Requires.That(systemPath.StartsWith(systemRoot + Path.PathSeparator));
            return systemPath;
        }
    }
}
