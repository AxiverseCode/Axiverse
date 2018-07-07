using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public abstract class TrackedList<T> : IList<T>, IList
    {
        private readonly List<T> list = new List<T>();

        public T this[int index]
        {
            get => list[index];
            set
            {
                var previous = list[index];
                list[index] = value;
                OnItemRemoved(previous);
                OnItemAdded(value);
            }
        }


        public void Add(T item)
        {
            list.Add(item);
            OnItemAdded(item);
        }

        public void Clear()
        {
            var items = list.ToArray();
            list.Clear();
            foreach (var item in items)
            {
                OnItemRemoved(item);
            }
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
            OnItemAdded(item);
        }

        public bool Remove(T item)
        {
            var removed = Remove(item);
            if (removed)
            {
                OnItemRemoved(item);
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            RemoveAt(index);
            OnItemRemoved(item);
        }
        
        public abstract void OnItemAdded(T item);

        public abstract void OnItemRemoved(T item);

        #region IList

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => list.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>)list).IsReadOnly;

        bool IList.IsReadOnly => ((IList)list).IsReadOnly;

        bool IList.IsFixedSize => ((IList)list).IsFixedSize;

        object ICollection.SyncRoot => ((IList)list).SyncRoot;

        bool ICollection.IsSynchronized => ((IList)list).IsSynchronized;

        int IList.IndexOf(object value)
        {
            return ((IList)list).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            Remove((T)value);
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
            // return ((IList)list).Add(value);
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

        #endregion
    }
}
