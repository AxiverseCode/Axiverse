using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public abstract class Loader<T> where T : class, IResource
    {
        public abstract bool Exists(string path);

        public abstract T Load(string path);

        public virtual bool TryLoad(string path, out T resource)
        {
            if (Exists(path))
            {
                resource = Load(path);
                return true;
            }

            resource = null;
            return false;
        }
    }
}
