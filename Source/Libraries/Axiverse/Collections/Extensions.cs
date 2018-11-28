using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    public static class Extensions
    {
        public static int BinarySearch<T>(this IList<T> list, T item)
        {
            return BinarySearchInternal(list, 0, list.Count, item, Comparer<T>.Default);
        }

        public static int BinarySearch<T>(this IList<T> list, T item, IComparer<T> comparer)
        {
            return BinarySearchInternal(list, 0, list.Count, item, Comparer<T>.Default);
        }

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
