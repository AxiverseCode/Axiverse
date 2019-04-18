using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Collections
{
    /// <summary>
    /// Collection algorithms.
    /// </summary>
    public static class Algorithms
    {
        /// <summary>
        /// Bubble sort for a list with the given comparer.
        /// </summary>
        /// <remarks>
        /// This is appropriate for sorting list
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparer"></param>
        public static void BubbleSort<T>(IList<T> list, IComparer<T> comparer)
        {
            bool sorted = true;
            do
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (comparer.Compare(list[i], list[i + 1]) == -1)
                    {
                        T value = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = value;
                        sorted = false;
                    }
                }
            } while (!sorted);
        }
    }
}
