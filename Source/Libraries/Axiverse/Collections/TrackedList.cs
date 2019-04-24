using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists. Notes when items are added or removed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public abstract class TrackedList<T> : IList<T>, IList
    {
        private readonly List<T> list;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the number of elements contained in the <see cref="TrackedList{T}"/>.
        /// </summary>
        public int Count => list.Count;

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without
        /// resizing.
        /// </summary>
        public int Capacity => list.Capacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedList{T}"/> class that is empty and
        /// has the default initial capacity.
        /// </summary>
        public TrackedList()
        {
            list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedList{T}"/> class that contains
        /// elements copied from the specified collection and has sufficient capacity to
        /// accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public TrackedList(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                OnItemAdding(item);
            }
            list = new List<T>(collection);
            foreach (var item in collection)
            {
                OnItemAdded(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedList{T}"/> class that is empty and
        /// has the specified initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public TrackedList(int capacity)
        {
            list = new List<T>(capacity);
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="TrackedList{T}"/>
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            OnItemAdding(item);
            list.Add(item);
            OnItemAdded(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the
        /// <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                OnItemAdding(item);
            }
            list.AddRange(collection);
            foreach (var item in collection)
            {
                OnItemAdded(item);
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="TrackedList{T}"/>.
        /// </summary>
        public void Clear()
        {
            foreach (var item in list)
            {
                OnItemRemoving(item);
            }
            var items = list.ToArray();
            list.Clear();
            foreach (var item in items)
            {
                OnItemRemoved(item);
            }
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="TrackedList{T}"/> to a compatible one-dimensional array,
        /// starting at the beginning of the target array.
        /// </summary>
        /// <param name="array"></param>
        public void CopyTo(T[] array)
        {
            list.CopyTo(array);
        }

        /// <summary>
        /// Copies the entire <see cref="TrackedList{T}"/> to a compatible one-dimensional array,
        /// starting at thespecified index of the target array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the entire <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Inserts an element into the <see cref="TrackedList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            OnItemAdded(item);
            list.Insert(index, item);
            OnItemAdded(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            OnItemRemoving(item);
            var removed = Remove(item);
            if (removed)
            {
                OnItemRemoved(item);
            }
            return removed;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="TrackedList{T}"/>.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            var item = this[index];
            OnItemRemoving(item);
            RemoveAt(index);
            OnItemRemoved(item);
        }

        /// <summary>
        /// Raises the ItemAdded event.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemAdding(T item) { }

        /// <summary>
        /// Raises the ItemAdded event.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemAdded(T item) { }

        /// <summary>
        /// Raises the ItemAdded event.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemRemoving(T item) { }

        /// <summary>
        /// Raises the ItemRemoved event.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemRemoved(T item) { }

        #region IList

        object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }

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
            var item = (T)value;
            var index = ((IList)list).Add(item);
            OnItemAdded(item);
            return index;
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
