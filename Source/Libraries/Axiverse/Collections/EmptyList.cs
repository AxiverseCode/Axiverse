using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public sealed class EmptyList<T> : IList<T>, IList
    {
        T IList<T>.this[int index]
        {
            get => throw new IndexOutOfRangeException();
            set => throw new IndexOutOfRangeException();
        }

        int ICollection<T>.Count => 0;

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => true;

        int ICollection.Count => 0;

        object ICollection.SyncRoot => Empty;

        bool ICollection.IsSynchronized => true;

        object IList.this[int index] { get => throw new IndexOutOfRangeException(); set => throw new IndexOutOfRangeException(); }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }

        int IList<T>.IndexOf(T item)
        {
            return -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            return false;
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return false;
        }

        void IList.Clear()
        {

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

        private EmptyList()
        {

        }

        public static readonly EmptyList<T> Empty = new EmptyList<T>();
    }
}
