using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// An object cache backed for loaded objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cache<T> where T : class, IResource
    {
        public T this[string path]
        {
            get
            {
                if (m_resources.TryGetValue(path, out var resource))
                {
                    return resource;
                }

                m_loaders.Find(loader => loader.TryLoad(path, out resource));
                return resource;
            }
            set
            {
                m_resources[path] = value;
            }
        }

        public int Count => m_resources.Count;

        public Cache()
        {

        }

        /// <summary>
        /// Checks if a path exists as a loaded resource or if it exists in any of the registered loaders.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            return m_resources.ContainsKey(path) || m_loaders.Exists(loader => loader.Exists(path));
        }

        /// <summary>
        /// Registers a resource loader.
        /// </summary>
        /// <param name="loader"></param>
        public void Register(Loader<T> loader)
        {
            m_loaders.Add(loader);
        }

        /// <summary>
        /// Unregisters a resource loader.
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        public bool Unregister(Loader<T> loader)
        {
            return m_loaders.Remove(loader);
        }

        private readonly List<Loader<T>> m_loaders = new List<Loader<T>>();
        private readonly Dictionary<String, T> m_resources = new Dictionary<string, T>();

        public static Cache<T> Default => new Cache<T>();
    }
}
