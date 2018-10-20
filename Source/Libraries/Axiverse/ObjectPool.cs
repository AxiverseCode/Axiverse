using System.Collections.Concurrent;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Axiverse
{
    /// <summary>
    /// An concurrent object pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T>
        where T : class
    {
        /// <summary>
        /// Gets the activator.
        /// </summary>
        public Func<T> Activator { get; }

        /// <summary>
        /// Gets the number of elements in the <see cref="ObjectPool{T}"/>.
        /// </summary>
        public int Count => bag.Count;

        /// <summary>
        /// Takes an object from the pool if there are any, otherwise returns a new object.
        /// </summary>
        /// <returns></returns>
        public virtual T Take()
        {
            if (bag.TryTake(out var result))
            {
                return result;
            }

            return Create();
        }

        public ObjectPool()
        {

        }

        public ObjectPool(Func<T> activator)
        {
            Activator = activator;
        }

        /// <summary>
        /// Adds an object to the pool to be reset and made available for others to use.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Reset(item);
            bag.Add(item);
            // Debug.WriteLine("returned " + typeof(T).Name);
        }

        /// <summary>
        /// Adds objects to the pool to be reset and made available for others to use.
        /// </summary>
        /// <param name="items"></param>
        public void AddAll(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Creates an new object.
        /// </summary>
        /// <returns></returns>
        protected virtual T Create()
        {
            return Activator();
        }

        /// <summary>
        /// Resets an added item.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void Reset(T item) { }

        private readonly ConcurrentBag<T> bag = new ConcurrentBag<T>();
    }
}
