using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    /// <summary>
    /// Provides data for the CollectionChanged event.
    /// </summary>
    public class CollectionChangedEventArgs : EventArgs
    {
        public Action Action { get; }

        public IList NewItems { get; }

        public int NewStartIndex { get; }

        public IList OldItems { get; }

        public int OldStartIndex { get; }

        public CollectionChangedEventArgs(Action action, IList newItems, int newStartIndex, IList oldItems, int oldStartIndex)
        {
            Action = action;
            NewItems = newItems;
            NewStartIndex = newStartIndex;
            OldItems = oldItems;
            OldStartIndex = oldStartIndex;
        }

        public CollectionChangedEventArgs(Action action, object newItem, int newStartIndex, object oldItem, int oldStartIndex)
        {
            Action = action;
            NewItems = newItem == null ? (IList)EmptyList<object>.Empty : new ItemList<object>(newItem);
            NewStartIndex = newStartIndex;
            OldItems = oldItem == null ? (IList)EmptyList<object>.Empty : new ItemList<object>(oldItem);
            OldStartIndex = oldStartIndex;
        }
    }

    /// <summary>
    /// Provides data for the CollectionChanged event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionChangedEventArgs<T> : CollectionChangedEventArgs
    {
        public new IList<T> NewItems { get; }

        public new IList<T> OldItems { get; }

        public CollectionChangedEventArgs(Action action, IList<T> newItems, int newStartIndex, IList<T> oldItems, int oldStartIndex)
            : base(action, newItems, newStartIndex, oldItems, oldStartIndex)
        {
            NewItems = newItems;
            OldItems = oldItems;
        }

        public CollectionChangedEventArgs(Action action, T newItem, int newStartIndex, T oldItem, int oldStartIndex)
            : base(
                  action,
                  newItem == null ? (IList)EmptyList<T>.Empty : new ItemList<T>(newItem),
                  newStartIndex,
                  oldItem == null ? (IList)EmptyList<T>.Empty : new ItemList<T>(oldItem),
                  oldStartIndex)
        {
            NewItems = base.NewItems as IList<T>;
            OldItems = base.OldItems as IList<T>;
        }
    }
}
