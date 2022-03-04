using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.HeapSort
{
    /// <summary>
    /// Creates a Priority Queue (or Heap) over the source enumerable using the
    /// supplied comparer.  The PriorityQueue memory footprint is exactly one
    /// array of integers matching the size of the source enumerable (this is
    /// comparable to the footprint of the OrderedEnumerable used by OrderBy)
    /// </summary>
    public abstract class APriorityEnumerable<TSource>
    {
        /// <summary>The number of items sorted so far total sorted</summary>
        protected int TotalSorted { get; set; }

        /// <summary>Indicates whether this is a Min-Heap or a Max-Heap</summary>
        protected bool MinHeap { get; set; }

        /// <summary>Indicates whether the list is fully sorted or not</summary>
        protected bool IsSorted { get; set; }

        /// <summary>The source array to sort</summary>
        protected TSource[] Source { get; set; }

        /// <summary>The heap datastructure - separate from source to allow for comparers and key selectors</summary>
        protected int[] HeapMap { get; set; }

        /// <summary>Sorts this instance yielding one element a time.  The heap is
        /// built at a cost of O(n) and each item is then sorted at a cost of Log(n)
        /// per item with no additional overhead.
        /// </summary>
        /// <returns>Sorted list one element at a time</returns>
        public IEnumerable<TSource> Sort()
        {
            BuildHeap();
            while (!IsSorted)
            {
                yield return Pop();
            }
        }

        /// <summary>
        /// Builds the heap with complexity O(n) by building from the bottom up.  Top-down
        /// building of the heap by using the Heapify function is O(n*log(n)).
        /// </summary>
        private void BuildHeap()
        {
            // Loop through the heap array in reverse order
            for (var i = HeapMap.Length / 2 - 1; i >= 0; i--)
            {
                DownHeap(i, HeapMap.Length);
            }
        }

        /// <summary>Pops the top item off of the heap by swapping it to the end and re-heapifying</summary>
        /// <returns>Top item off of the heap - the largest item for a max heap, or the smallest for a min heap.  Returns default(T) if no items remain on the heap.</returns>
        private TSource Pop()
        {
            var root = -1;
            var remaining = HeapMap.Length - TotalSorted;
            if (remaining > 10)
            {
                root = HeapMap[0];
                var lastIndex = remaining - 1;
                int temp = HeapMap[0];
                HeapMap[0] = HeapMap[lastIndex];
                HeapMap[lastIndex] = temp;

                TotalSorted++;
                remaining = HeapMap.Length - TotalSorted;
                if (remaining > 10)
                {
                    DownHeap(0, remaining);
                }
            }
            else if (remaining == 10)
            {
                InsertionSort(remaining);
                root = HeapMap[remaining - 1];
                TotalSorted++;
            }
            else if (remaining > 0)
            {
                root = HeapMap[remaining - 1];
                TotalSorted++;
                if (TotalSorted == HeapMap.Length)
                {
                    IsSorted = true;
                }
            }

            if (root == -1)
            {
                return default(TSource);
            }

            return Source[root];
        }

        /// <summary>Re-heapifies from the given index</summary>
        private void DownHeap(int index, int n)
        {
            var stack = new Stack<int>();
            stack.Push(index);

            while (stack.Count > 0)
            {
                index = stack.Pop();

                var leftIndex = 2 * index + 1;
                var rightIndex = 2 * index + 2;

                if (!MinHeap)
                {
                    var largest = index;
                    if (CompareKeys(HeapMap[leftIndex], HeapMap[largest]) > 0)
                    {
                        largest = leftIndex;
                    }

                    if (rightIndex < n && CompareKeys(HeapMap[rightIndex], HeapMap[largest]) > 0)
                    {
                        largest = rightIndex;
                    }

                    if (largest != index)
                    {
                        int temp = HeapMap[largest];
                        HeapMap[largest] = HeapMap[index];
                        HeapMap[index] = temp;

                        if (2 * largest + 1 < n)
                        {
                            stack.Push(largest);
                        }
                    }
                }
                else
                {
                    var smallest = index;
                    if (CompareKeys(HeapMap[leftIndex], HeapMap[smallest]) < 0)
                    {
                        smallest = leftIndex;
                    }

                    if (rightIndex < n && CompareKeys(HeapMap[rightIndex], HeapMap[smallest]) < 0)
                    {
                        smallest = rightIndex;
                    }

                    if (smallest != index)
                    {
                        int temp = HeapMap[smallest];
                        HeapMap[smallest] = HeapMap[index];
                        HeapMap[index] = temp;

                        if (2 * smallest + 1 < n)
                        {
                            stack.Push(smallest);
                        }
                    }
                }
            }
        }

        /// <summary>Performs an insertion sort on the remaining 'size' items.  Insertion sort is more performant on small lists</summary>
        /// <param name="size">The number of items to sort in the list</param>
        private void InsertionSort(int size)
        {
            if (!MinHeap)
            {
                var i = 1;
                while (i < size)
                {
                    var j = i;
                    while (j > 0 && CompareKeys(HeapMap[j - 1], HeapMap[j]) > 0)
                    {
                        int temp = HeapMap[j - 1];
                        HeapMap[j - 1] = HeapMap[j];
                        HeapMap[j] = temp;
                        j--;
                    }

                    i++;
                }
            }
            else
            {
                var i = 1;
                while (i < size)
                {
                    var j = i;
                    while (j > 0 && CompareKeys(HeapMap[j - 1], HeapMap[j]) < 0)
                    {
                        int temp = HeapMap[j - 1];
                        HeapMap[j - 1] = HeapMap[j];
                        HeapMap[j] = temp;
                        j--;
                    }

                    i++;
                }
            }
        }

        /// <summary>Compares the keys at indexes 'left' and 'right'</summary>
        /// <param name="left">The left item index</param>
        /// <param name="right">The right item index</param>
        /// <returns>-1 if left less than right; 0 if left equals right; 1 if left greater than right</returns>
        protected abstract int CompareKeys(int left, int right);
    }

    /// <summary>
    /// Creates a Priority Queue (or Heap) over the source enumerable using the
    /// supplied comparer.  The PriorityQueue memory footprint is exactly one
    /// array of integers matching the size of the source enumerable (this is
    /// comparable to the footprint of the OrderedEnumerable used by OrderBy)
    /// </summary>
    public class PriorityEnumerable<TSource> : APriorityEnumerable<TSource> where TSource : IComparable<TSource>
    {
        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource}"/> class.</summary>
        /// <param name="source">The source.</param>
        public PriorityEnumerable(IEnumerable<TSource> source) : this(source, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource}"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="minHeap">if set to <c>true</c> then creates a min-heap.</param>
        public PriorityEnumerable(IEnumerable<TSource> source, bool minHeap)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            MinHeap = minHeap;
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            var i = 0;
            foreach (var item in source)
            {
                Source[i] = item;
                HeapMap[i] = i;
                i++;
            }
            TotalSorted = 0;
        }

        /// <summary>Compares the keys at indexes 'left' and 'right'</summary>
        /// <param name="left">The left item index</param>
        /// <param name="right">The right item index</param>
        /// <returns>-1 if left less than right; 0 if left equals right; 1 if left greater than right</returns>
        protected override int CompareKeys(int left, int right)
        {
            return Source[left].CompareTo(Source[right]);
        }
    }

    /// <summary>
    /// Creates a Priority Queue (or Heap) over the source enumerable using the
    /// supplied comparer.  The PriorityQueue memory footprint is exactly one
    /// array of integers matching the size of the source enumerable (this is
    /// comparable to the footprint of the OrderedEnumerable used by OrderBy)
    /// </summary>

    public class PriorityEnumerable<TSource, TKey> : APriorityEnumerable<TSource> where TKey : IComparable<TKey>
    {
        /// <summary>Stores the key values per index for faster access</summary>
        private TKey[] _keyMap;

        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource, TKey}"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        public PriorityEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            : this(source, keySelector, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource, TKey}"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="minHeap">if set to <c>true</c> then creates a min-heap (ascending order).</param>
        public PriorityEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool minHeap)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            MinHeap = minHeap;
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            _keyMap = new TKey[Source.Length];
            var i = 0;
            foreach (var item in source)
            {
                Source[i] = item;
                _keyMap[i] = keySelector(item);
                HeapMap[i] = i;
                i++;
            }
            TotalSorted = 0;
        }

        /// <summary>Compares the keys at indexes 'left' and 'right'</summary>
        /// <param name="left">The left item index</param>
        /// <param name="right">The right item index</param>
        /// <returns>-1 if left less than right; 0 if left equals right; 1 if left greater than right</returns>
        protected override int CompareKeys(int left, int right)
        {
            return _keyMap[left].CompareTo(_keyMap[right]);
        }
    }

    /// <summary>
    /// Creates a Priority Queue (or Heap) over the source enumerable using the
    /// supplied comparer.  The PriorityQueue memory footprint is exactly one
    /// array of integers matching the size of the source enumerable (this is
    /// comparable to the footprint of the OrderedEnumerable used by OrderBy)
    /// </summary>
    public class PriorityEnumerableWithComparer<TSource, TKey> : APriorityEnumerable<TSource>
    {
        /// <summary>Stores the key values per index for faster access</summary>
        private TKey[] KeyMap;

        /// <summary>The comparer to use for comparing element keys</summary>
        private IComparer<TKey> Comparer;

        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource, TKey}"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        public PriorityEnumerableWithComparer(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
            : this(source, keySelector, comparer, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PriorityEnumerable{TSource, TKey}"/> class.</summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="minHeap">if set to <c>true</c> then creates a min-heap (ascending order).</param>
        public PriorityEnumerableWithComparer(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool minHeap)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            MinHeap = minHeap;
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            KeyMap = new TKey[Source.Length];
            Comparer = comparer;
            var i = 0;
            foreach (var item in source)
            {
                Source[i] = item;
                KeyMap[i] = keySelector(item);
                HeapMap[i] = i;
                i++;
            }
            TotalSorted = 0;
        }

        /// <summary>Compares the keys at indexes 'left' and 'right'</summary>
        /// <param name="left">The left item index</param>
        /// <param name="right">The right item index</param>
        /// <returns>-1 if left less than right; 0 if left equals right; 1 if left greater than right</returns>
        protected override int CompareKeys(int left, int right)
        {
            return Comparer.Compare(KeyMap[left], KeyMap[right]);
        }
    }
}
