using System.Collections.Concurrent;

namespace Axiverse
{
    /// <summary>
    /// An concurrent object pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ObjectPool<T>
        where T : class
    {
        /// <summary>
        /// Takes an object from the pool if there are any, otherwise returns a new object.
        /// </summary>
        /// <returns></returns>
        public T Take()
        {
            if (bag.TryTake(out var result))
            {
                return result;
            }

            return Create();
        }

        /// <summary>
        /// Adds an object to the pool to be reset and made available for others to use.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Reset(item);
            bag.Add(item);
        }

        /// <summary>
        /// Creates an new object.
        /// </summary>
        /// <returns></returns>
        protected abstract T Create();

        /// <summary>
        /// Resets an added item.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void Reset(T item) { }

        private readonly ConcurrentBag<T> bag = new ConcurrentBag<T>();
    }
}
