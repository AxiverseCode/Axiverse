using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public class TrackedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> BaseDictionary { get; }

        public TValue this[TKey key]
        {
            get => BaseDictionary[key];
            set
            {
                if (BaseDictionary.TryGetValue(key, out var item))
                {
                    OnItemRemoving(key, item);
                }

                OnItemAdding(key, value);
                BaseDictionary[key] = value;
                OnItemRemoved(key, item);
                OnItemAdded(key, value);
            }
        }

        public int Count => BaseDictionary.Count;

        public ICollection<TKey> Keys => BaseDictionary.Keys;

        public ICollection<TValue> Values => BaseDictionary.Values;

        public TrackedDictionary()
        {
            BaseDictionary = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            OnItemAdding(key, value);
            BaseDictionary.Add(key, value);
            OnItemAdded(key, value);
        }

        public bool Remove(TKey key)
        {
            if (!BaseDictionary.TryGetValue(key, out var item))
            {
                return false;
            }

            OnItemRemoving(key, item);
            BaseDictionary.Remove(key);
            OnItemRemoved(key, item);
            return true;
        }

        public void Clear()
        {
            var items = BaseDictionary.ToArray();
            foreach (var item in items)
            {
                OnItemRemoving(item.Key, item.Value);
            }
            BaseDictionary.Clear();
            foreach (var item in items)
            {
                OnItemRemoved(item.Key, item.Value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value) => BaseDictionary.TryGetValue(key, out value);

        public bool ContainsKey(TKey key) => BaseDictionary.ContainsKey(key);

        public bool ContainsValue(TValue value) => BaseDictionary.ContainsValue(value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => BaseDictionary.GetEnumerator();

        protected virtual void OnItemAdding(TKey key, TValue value) { }

        protected virtual void OnItemAdded(TKey key, TValue value) { }

        protected virtual void OnItemRemoving(TKey key, TValue value) { }

        protected virtual void OnItemRemoved(TKey key, TValue value) { }





        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)BaseDictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)BaseDictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
