using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Represents a binding hierarchy. Binding dictionaries will be queried in reverse order when
    /// looking for bindings.
    /// </summary>
    public class BindingHierarchy : List<BindingDictionary>, IBindingProvider
    {
        /// <summary>
        /// Gets the binding with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws if the key is not found.</exception>
        public object this[Key key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Sees if any provider contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Key key)
        {
            for (int i = Count - 1; i >= 0 ; i++)
            {
                if (this[i].ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to get a binding from the top dictionary and looks down.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(Key key, out object value)
        {
            for (int i = Count - 1; i >= 0; i++)
            {
                if (this[i].TryGetValue(key, out value))
                {
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}
