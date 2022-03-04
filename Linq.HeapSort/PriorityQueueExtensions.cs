using System;
using System.Collections.Generic;

namespace Linq.HeapSort
{
    /// <summary>Class for the PriorityQueue extension methods</summary>
    public static class PriorityQueueExtensions
    {
        /// <summary>
        /// Orders the collection with a Priority Queue using the default comparer.  The cost will be
        /// O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderBy<TSource>(this IEnumerable<TSource> source)
            where TSource : IComparable<TSource>
        {
            var priorityQueue = new PriorityEnumerable<TSource>(source);
            return priorityQueue.Sort();
        }

        /// <summary>
        /// Orders the collection with a Priority Queue using the default comparer.  The cost will be
        /// O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            var priorityQueue = new PriorityEnumerable<TSource, TKey>(source, keySelector);
            return priorityQueue.Sort();
        }

        /// <summary>
        /// Orders the collection with a Priority Queue using the default comparer.  The cost will be
        /// O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The compare to use on the keys.</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var priorityQueue = new PriorityEnumerableWithComparer<TSource, TKey>(source, keySelector, comparer);
            return priorityQueue.Sort();
        }

        /// <summary>
        /// Orders the collection in descending order with a Priority Queue using the default comparer.  The cost will be
        /// O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderByDescending<TSource>(this IEnumerable<TSource> source)
            where TSource : IComparable<TSource>
        {
            var priorityQueue = new PriorityEnumerable<TSource>(source, false);
            return priorityQueue.Sort();
        }

        /// <summary>
        /// Orders the collection in descending order with a Priority Queue using the default comparer.
        /// The cost will be O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            var priorityQueue = new PriorityEnumerable<TSource, TKey>(source, keySelector, false);
            return priorityQueue.Sort();
        }

        /// <summary>
        /// Orders the collection in descending order with a Priority Queue using the default comparer.
        /// The cost will be O(k*log(n)) where k is the number of items enumerated over.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source enumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The compare to use on the keys.</param>
        /// <returns>Ordered enumerable of TSource items</returns>
        /// <remarks>
        /// Time complexitiy is significantly improved over default OrderBy implementation for large
        /// lists with small values of k.  Performance improvement is observed up to a value of k that
        /// is roughly 40% the size of n.  For larger values of k, you are better off using the default OrderBy
        /// implementation as the QuickSort has a much faster average sort time for full lists.
        /// </remarks>
        public static IEnumerable<TSource> PriorityOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var priorityQueue = new PriorityEnumerableWithComparer<TSource, TKey>(source, keySelector, comparer, false);
            return priorityQueue.Sort();
        }
    }
}
