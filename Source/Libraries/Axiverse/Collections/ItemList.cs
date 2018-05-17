using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Axiverse.Collections
{
    /// <summary>
    /// A read only collection with one item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemList<T> : IList<T>, IList
    { 
        T IList<T>.this[int index]
        {
            get => index == 0 ? Item : throw new IndexOutOfRangeException();
            set => value = index == 0 ? value : throw new IndexOutOfRangeException();
        }
        
        object IList.this[int index]
        {
            get => index == 0 ? Item : throw new IndexOutOfRangeException();
            set => value = index == 0 ? value : throw new IndexOutOfRangeException();
        }

        public T Item { get; set; }

        public int Count => 1;

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => true;

        object ICollection.SyncRoot => null;

        bool ICollection.IsSynchronized => false;

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return Item.Equals(item);
        }

        bool IList.Contains(object value)
        {
            return Item.Equals(value);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = Item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return Item;
        }

        public int IndexOf(T item)
        {
            return Item.Equals(item) ? 0 : -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        int IList.IndexOf(object value)
        {
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {

        }

        public ItemList()
        {

        }

        public ItemList(T item)
        {
            Item = item;
        }
    }
}
