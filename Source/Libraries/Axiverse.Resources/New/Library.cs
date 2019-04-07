using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public class Library
    {
        public void Mount(string path, IMount mount)
        {
            root = new Directory();
        }

        public T Load<T>(string path, bool ignoreExtension = false)
        {
            return default(T);
        }

        public T LoadAs<T>(string path, string extension)
        {
            return default(T);
        }

        private Node root;
        private Dictionary<Type, ITypeDeserializer> deserializers = new Dictionary<Type, ITypeDeserializer>();
    }
}
