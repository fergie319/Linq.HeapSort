using System;
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
    public class PriorityEnumerable<TSource, TKey> where TKey: IComparable<TKey>
    {
        // TODO: Make three Priority enumerables with different type constraints?
        //       Yes.  And use an abstract CompareKeys method
        public int TotalSorted { get; set; }

        public bool MinHeap { get; private set; }

        public bool IsSorted { get; private set; }

        public TSource[] Source { get; private set; }

        // Use this for the implementation where no TKey is needed
        // public Tuple<TSource, TKey>[] Heap { get; set; }

        // TODO: Try out using two arrays, and use the HeapMap as an index array
        //       If performance is still 2X worse than QuickSort, then try a third
        //       array of TSource to see if the ElementAt(k) method is the source
        //       of the problem.
        public int[] HeapMap { get; set; }

        // Use this for the implementation (x2) for keySelector -
        // use type constraint for IComparer when no comparer is provided
        public TKey[] KeyMap { get; set; }

        public PriorityEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            Source = new TSource[source.Count()];
            HeapMap = new int[Source.Length];
            KeyMap = new TKey[Source.Length];
            var i = 0;
            foreach (var item in source)
            {
                HeapMap[i] = i;
                Source[i] = item;
                KeyMap[i] = keySelector(item);
                i++;
            }
            TotalSorted = 0;
        }

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
                Swap(0, HeapMap.Length - TotalSorted - 1);
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
            var leftIndex = 2*index + 1;
            var rightIndex = 2*index + 2;
            
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
                    Swap(index, largest);

                    if (largest < n)
                    {
                        DownHeap(largest, n);
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
                    Swap(index, smallest);

                    if (smallest < n)
                    {
                        DownHeap(smallest, n);
                    }
                }
            }
        }

        private void Swap(int indexA, int indexB)
        {
            int temp = HeapMap[indexA];
            HeapMap[indexA] = HeapMap[indexB];
            HeapMap[indexB] = temp;
        }

        protected virtual int CompareKeys(int left, int right)
        {
            return KeyMap[left].CompareTo(KeyMap[right]);
        }
    }
}
