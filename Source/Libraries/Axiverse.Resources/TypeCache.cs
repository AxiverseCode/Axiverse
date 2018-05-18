using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Axiverse.Injection;

namespace Axiverse.Resources
{
    public class TypeCache<T>
    {
        public Library Library { get; }

        public Cached<T> Load(string path)
        {
            // Try to retrieve from cache.
            if (m_objects.TryGetValue(path, out var cached))
            {
                cached.References += 1;
                return cached;
            }

            var extension = Path.GetExtension(path).ToLower();
            if (m_extensions.TryGetValue(extension, out var loader))
            {
                cached = new Cached<T>(path, loader.Load(Library.OpenRead(path), new LoadContext(Library, path)));
                m_objects.Add(path, cached);
                return cached;
            }
            return null;
        }

        public void Unload(Cached<T> cached)
        {
            cached.References -= 1;
            if (cached.References == 0)
            {
                // dispose?
            }
        }

        public void Register(ILoader<T> loader)
        {
            foreach (var extension in loader.Extensions)
            {
                m_extensions.Add(extension, loader);
            }
        }

        public TypeCache()
        {
            Library = Injector.Global.Resolve<Library>();
        }

        public TypeCache(Library library)
        {
            Library = library;
        }

        Dictionary<string, ILoader<T>> m_extensions = new Dictionary<string, ILoader<T>>();
        Dictionary<string, Cached<T>> m_objects = new Dictionary<string, Cached<T>>();
    }
}
