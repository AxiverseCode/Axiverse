using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public class TrackedList<T> : IList<T>, IList
    {
        private readonly List<T> list = new List<T>();

        public T this[int index]
        {
            get => list[index];
            set
            {
                list[index] = value;
            }
        }

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => list.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>)list).IsReadOnly;

        bool IList.IsReadOnly => ((IList)list).IsReadOnly;

        bool IList.IsFixedSize => ((IList)list).IsFixedSize;

        object ICollection.SyncRoot => ((IList)list).SyncRoot;

        bool ICollection.IsSynchronized => ((IList)list).IsSynchronized;

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return Remove(item);
        }

        public void RemoveAt(int index)
        {
            RemoveAt(index);
        }

        int IList.Add(object value)
        {
            return ((IList)list).Add(value);
        }

        bool IList.Contains(object value)
        {
            return ((IList)list).Contains(value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)list).CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.IndexOf(object value)
        {
            return ((IList)list).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)list).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            ((IList)list).Remove(value);
        }
    }
}
