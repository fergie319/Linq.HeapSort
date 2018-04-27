using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.HeapSort
{
    public static class PriorityQueueExtensions
    {
        public static IEnumerable<TSource> PriorityOrderBy<TSource>(this IEnumerable<TSource> source)
            where TSource : IComparable<TSource>
        {
            return null;
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return null;
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var key = keySelector(source.ElementAt(0));
            return null;
        }
    }
}
