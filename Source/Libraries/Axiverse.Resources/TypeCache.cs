using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Axiverse.Injection;

namespace Axiverse.Resources
{
    /// <summary>
    /// An object cache backed for loaded objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeCache<T>
    {
        /// <summary>
        /// Adds an object to the cache with the given uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Cached<T> Add(Uri uri, T value)
        {
            var cached = new Cached<T>(uri, value);
            m_objects.Add(uri, cached);
            return cached;
        }

        /// <summary>
        /// Removes an object from the cache with the given uri.
        /// </summary>
        /// <param name="uri"></param>
        public bool Remove(Uri uri)
        {
            return  m_objects.Remove(uri);
        }

        /// <summary>
        /// Requests for an object to be loaded by the registered loaders.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Throws if none of the processors can load the uri.</exception>
        public Cached<T> Load(Uri uri)
        {
            // Try to retrieve from cache.
            if (m_objects.TryGetValue(uri, out var cached))
            {
                cached.References += 1;
                return cached;
            }

            foreach (var loader in m_loaders)
            {
                if (loader.TryLoad(uri, out var value))
                {
                    cached = new Cached<T>(uri, value);
                    m_objects.Add(uri, cached);
                    return cached;
                }
            }
            
            throw new FileNotFoundException();
        }

        /// <summary>
        /// Releases a reference to a cached object.
        /// </summary>
        /// <param name="cached"></param>
        public void Unload(Cached<T> cached)
        {
            cached.References -= 1;
            if (cached.References == 0)
            {
                (cached.Value as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Registers a loader to be used when loading objects.
        /// </summary>
        /// <param name="loader"></param>
        public void Register(ILoader<T> loader)
        {
            m_loaders.Add(loader);
        }

        private readonly List<ILoader<T>> m_loaders = new List<ILoader<T>>();
        private readonly Dictionary<Uri, Cached<T>> m_objects = new Dictionary<Uri, Cached<T>>();
    }
}
