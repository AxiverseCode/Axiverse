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
    public class Cache
    {
        public Library Library { get; set; }

        public Cache()
        {
            Library = Injector.Global.Resolve<Library>();
        }

        public Cache(Library library)
        {
            Library = library;
        }

        public Cached<T> Load<T>(string path)
        {
            return GetTypeCache<T>().Load(path);
        }

        public void Unload<T>(Cached<T> cached)
        {
            GetTypeCache<T>().Unload(cached);
        }

        public void Register<T>(ILoader<T> loader)
        {
            GetTypeCache<T>().Register(loader);
        }

        protected TypeCache<T> GetTypeCache<T>()
        {
            if (m_typeCaches.TryGetValue(typeof(T), out var cache))
            {
                return cache as TypeCache<T>;
            }

            var typeCache = new TypeCache<T>(Library);
            m_typeCaches.Add(typeof(T), typeCache);
            return typeCache;
        }

        private Dictionary<Type, object> m_typeCaches = new Dictionary<Type, object>();
    }
}
