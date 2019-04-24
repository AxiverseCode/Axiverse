using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public class CachedDictionary<TKey, TValue>
    {
        LinkedList<KeyValuePair<TKey, TValue>> cache;
        Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> items;

        public void Add(TKey key, TValue value)
        {
            var node = cache.AddFirst(new KeyValuePair<TKey, TValue>(key, value));
            items.Add(key, node);
        }

        public void Touch(TKey key)
        {
            if (items.TryGetValue(key, out var node))
            {
                cache.Remove(node);
                cache.AddFirst(node);
            }
        }
    }
}
