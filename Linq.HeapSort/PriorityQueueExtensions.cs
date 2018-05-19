using System;
using System.Collections.Generic;

namespace Linq.HeapSort
{
    public static class PriorityQueueExtensions
    {
        public static IEnumerable<TSource> PriorityOrderBy<TSource>(this IEnumerable<TSource> source)
            where TSource : IComparable<TSource>
        {
            var priorityQueue = new PriorityEnumerable<TSource>(source);
            return priorityQueue.Sort();
        }

        public static IEnumerable<TSource> PriorityOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            var priorityQueue = new PriorityEnumerable<TSource, TKey>(source, keySelector);
            return priorityQueue.Sort();
        }

        public static IEnumerable<TSource> PriorityOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var priorityQueue = new PriorityEnumerableWithComparer<TSource, TKey>(source, keySelector, comparer);
            return priorityQueue.Sort();
        }

        public static IEnumerable<TSource> PriorityOrderByDescending<TSource>(this IEnumerable<TSource> source)
            where TSource : IComparable<TSource>
        {
            var priorityQueue = new PriorityEnumerable<TSource>(source, false);
            return priorityQueue.Sort();
        }

        public static IEnumerable<TSource> PriorityOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            var priorityQueue = new PriorityEnumerable<TSource, TKey>(source, keySelector, false);
            return priorityQueue.Sort();
        }

        public static IEnumerable<TSource> PriorityOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var priorityQueue = new PriorityEnumerableWithComparer<TSource, TKey>(source, keySelector, comparer, false);
            return priorityQueue.Sort();
        }
    }
}
