using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    /// <summary>
    /// Linq collections extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Uses a binary search algorithm to locate a specific element in the sorted <see cref="IList{T}"/> or a portion of it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, T item)
        {
            return BinarySearchInternal(list, 0, list.Count, item, Comparer<T>.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, T item, IComparer<T> comparer)
        {
            return BinarySearchInternal(list, 0, list.Count, item, Comparer<T>.Default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be <code>null</code> for reference types.</param>
        /// <param name="comparer"></param>
        /// <returns>The IComparer<T> implementation to use when comparing elements, or null to use the default comparer Default.</returns>
        private static int BinarySearchInternal<T>(this IList<T> list, int index, int count, T item, IComparer<T> comparer)
        {
            int lower = index;
            int upper = count - 1;

            while (lower <= upper)
            {
                // middle = lower + higher / 2, but compensating for overflow.
                int middle = lower + (upper - lower) / 2;
                int comparison = comparer.Compare(item, list[middle]);

                if (comparison < 0)
                {
                    // The value is lower than the value at the middle.
                    upper = middle - 1;
                }
                else if (comparison > 1)
                {
                    // The value is greater than the value at the middle.
                    lower = middle + 1;
                }
                else
                {
                    return middle;
                }
            }

            // The lower is now the first element higher than the value.
            return ~lower;
        }
    }
}
