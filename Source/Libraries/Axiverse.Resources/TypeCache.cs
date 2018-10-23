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
    /// An typed object cache backed for loaded objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeCache<T>
    {
        /// <summary>
        /// Gets the <see cref="Library"/> which backs the cache and loaders.
        /// </summary>
        public Library Library { get; }

        /// <summary>
        /// Constructs a typed cache given the specified <see cref="Library"/>.
        /// </summary>
        public TypeCache(Library library)
        {
            Library = library;
        }

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

            return Create(uri);
        }

        /// <summary>
        /// Creates the file drawing 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected Cached<T> Create(Uri uri)
        {
            // Get the stream.
            if (Library.FileExists(uri))
            {
                using(var stream = Library.OpenRead(uri))
                {
                    foreach (var loader in m_loaders)
                    {
                        if (loader.TryLoad(stream, out var value))
                        {
                            var cached = new Cached<T>(uri, value);
                            m_objects.Add(uri, cached);
                            return cached;
                        }
                    }
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
        public void Register(IResourceLoader<T> loader)
        {
            m_loaders.Add(loader);
        }

        private readonly List<IResourceLoader<T>> m_loaders = new List<IResourceLoader<T>>();
        private readonly Dictionary<Uri, Cached<T>> m_objects = new Dictionary<Uri, Cached<T>>();
    }
}
