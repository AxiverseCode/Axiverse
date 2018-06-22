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
    public class Cache
    {
        /// <summary>
        /// Adds an resource into the cache with the given uri.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Cached<T> Add<T>(Uri uri, T value)
        {
            return GetTypeCache<T>().Add(uri, value);
        }

        /// <summary>
        /// Removes a resource into the cache with the given uri.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        public bool Remove<T>(Uri uri)
        {
            return GetTypeCache<T>().Remove(uri);
        }

        /// <summary>
        /// Requests for an object to be loaded by the registered loaders.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Throws if none of the processors can load the uri.</exception>
        public Cached<T> Load<T>(Uri uri)
        {
            return GetTypeCache<T>().Load(uri);
        }

        /// <summary>
        /// Releases a reference to a cached object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cached"></param>
        public void Unload<T>(Cached<T> cached)
        {
            GetTypeCache<T>().Unload(cached);
        }

        /// <summary>
        /// Registers a loader to be used when loading objects.
        /// </summary>
        /// <param name="loader"></param>
        public void Register<T>(ILoader<T> loader)
        {
            GetTypeCache<T>().Register(loader);
        }

        /// <summary>
        /// Gets the <see cref="TypeCache{T}"/> for the requested type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected TypeCache<T> GetTypeCache<T>()
        {
            if (m_typeCaches.TryGetValue(typeof(T), out var cache))
            {
                return cache as TypeCache<T>;
            }

            var typeCache = new TypeCache<T>();
            m_typeCaches.Add(typeof(T), typeCache);
            return typeCache;
        }

        private Dictionary<Type, object> m_typeCaches = new Dictionary<Type, object>();
    }
}
