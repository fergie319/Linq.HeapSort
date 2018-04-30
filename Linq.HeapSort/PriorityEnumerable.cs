﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int TotalSorted { get; set; }

        public bool MinHeap { get; protected set; }

        public bool IsSorted { get; protected set; }

        public TSource[] Source { get; protected set; }

        public int[] HeapMap { get; set; }

        public IEnumerable<TSource> Sort()
        {
            BuildHeap();
            while (!IsSorted)
            {
                yield return Pop();
            }
        }

        /// <summary>
        /// Builds a max heap with complexity O(n) by building from the bottom up.  Top-down
        /// building of the heap by using the Heapify function is O(n*log(n)).
        /// </summary>
        public void BuildHeap()
        {
            BuildHeap(false);
        }

        /// <summary>
        /// Builds the heap with complexity O(n) by building from the bottom up.  Top-down
        /// building of the heap by using the Heapify function is O(n*log(n)).
        /// </summary>
        /// <param name="minHeap">if set to <c>true</c> then the heap is built with smallest items on top; otherwise, largest items on top.</param>
        public void BuildHeap(bool minHeap)
        {
            // Loop through the heap array in reverse order
            for (var i = HeapMap.Length / 2 - 1; i >= 0; i--)
            {
                DownHeap(i, HeapMap.Length);
            }
        }

        /// <summary>Pops the top item off of the heap by swapping it to the end and re-heapifying</summary>
        /// <returns>Top item off of the heap - the largest item for a max heap, or the smallest for a min heap.  Returns default(T) if no items remain on the heap.</returns>
        public TSource Pop()
        {
            var root = -1;
            if (HeapMap.Length - TotalSorted != 0)
            {
                root = HeapMap[0];
                var lastIndex = HeapMap.Length - TotalSorted - 1;
                int temp = HeapMap[0];
                HeapMap[0] = HeapMap[lastIndex];
                HeapMap[lastIndex] = temp;

                TotalSorted++;
                var remaining = HeapMap.Length - TotalSorted;
                if (remaining > 1)
                {
                    DownHeap(0, remaining);
                }
                else if (remaining == 0)
                {
                    IsSorted = true;
                }
            }

            if (root == -1)
            {
                return default(TSource);
            }
            else
            {
                return Source[root];
            }
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
                    if (leftIndex < n && CompareKeys(HeapMap[leftIndex], HeapMap[largest]) > 0)
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

                        if (2*largest + 1 < n)
                        {
                            stack.Push(largest);
                        }
                    }
                }
                else
                {
                    var smallest = index;
                    if (leftIndex < n && CompareKeys(HeapMap[leftIndex], HeapMap[smallest]) < 0)
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

                        if (2*smallest + 1 < n)
                        {
                            stack.Push(smallest);
                        }
                    }
                }
            }
        }

        protected abstract int CompareKeys(int left, int right);
    }

    public class PriorityEnumerable<TSource> : APriorityEnumerable<TSource> where TSource: IComparable<TSource>
    {
        public PriorityEnumerable(IEnumerable<TSource> source)
        {
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            for (var i = 0; i < Source.Length; i++)
            {
                HeapMap[i] = i;
                Source[i] = source.ElementAt(i);
                i++;
            }
            TotalSorted = 0;
        }

        protected override int CompareKeys(int left, int right)
        {
            return Source[left].CompareTo(Source[right]);
        }
    }

    public class PriorityEnumerable<TSource, TKey> : APriorityEnumerable<TSource> where TKey: IComparable<TKey>
    {
        // Use this for the implementation (x2) for keySelector -
        // use type constraint for IComparer when no comparer is provided
        public TKey[] KeyMap { get; set; }

        public PriorityEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            KeyMap = new TKey[Source.Length];
            for (var i = 0; i < Source.Length; i++)
            {
                HeapMap[i] = i;
                Source[i] = source.ElementAt(i);
                KeyMap[i] = keySelector(Source[i]);
            }
            TotalSorted = 0;
        }

        protected override int CompareKeys(int left, int right)
        {
            return KeyMap[left].CompareTo(KeyMap[right]);
        }
    }

    public class PriorityEnumerableWithComparer<TSource, TKey> : APriorityEnumerable<TSource>
    {
        // Use this for the implementation (x2) for keySelector -
        // use type constraint for IComparer when no comparer is provided
        public TKey[] KeyMap { get; set; }

        public IComparer<TKey> Comparer { get; set; }

        public PriorityEnumerableWithComparer(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            KeyMap = new TKey[Source.Length];
            Comparer = comparer;
            for (var i = 0; i < Source.Length; i++)
            {
                HeapMap[i] = i;
                Source[i] = source.ElementAt(i);
                KeyMap[i] = keySelector(Source[i]);
            }
            TotalSorted = 0;
        }

        protected override int CompareKeys(int left, int right)
        {
            return Comparer.Compare(KeyMap[left], KeyMap[right]);
        }
    }
}
