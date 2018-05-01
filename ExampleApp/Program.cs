using Linq.HeapSort;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
@"This simple app will run sorting time comparisons between
the PriorityEnumerable and the default OrderBy randomly generated
integers ranging from 0-10,000.

The OrderBy implementation has to sort the entire list up front, so
it does not matter whether you take 5 or 500 elements in your list,
it will always take the same amount of time.");
            Console.WriteLine("Type 'Exit' at any time to terminate the program");
            
            var input = string.Empty;
            do
            {
                // Read in and parse the size of the list to text
                Console.WriteLine("Enter size of list to sort:");
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    break;
                }

                var size = int.Parse(input, System.Globalization.NumberStyles.AllowThousands);

                // Read in and parse the number of items to take
                Console.WriteLine("Enter number of items to take (from 1 to {0}", size);
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    break;
                }

                var take = int.Parse(input, System.Globalization.NumberStyles.AllowThousands);

                // Generate two identical lists of random numbers
                var testListOne = new List<int>();
                var testListTwo = new List<int>();
                var rand = new Random(DateTime.Now.Millisecond);
                for (var i = 0; i < size; i++)
                {
                    var val = rand.Next(10000);
                    testListOne.Add(val);
                    testListTwo.Add(val);
                }

                // Test OrderBy Ascending
                Console.WriteLine("Testing OrderBy...");
                var quickStopWatch = new Stopwatch();
                quickStopWatch.Start();
                var quickResult = testListOne.OrderByDescending(k => k).Take(take).ToList();
                quickStopWatch.Stop();

                Console.WriteLine("Testing HeapSort...");
                var priorityEnumerable = new PriorityEnumerable<int, int>(testListTwo, t => t);
                var heapStopWatch = new Stopwatch();
                heapStopWatch.Start();
                var result = priorityEnumerable.Sort().Take(take).ToList();
                heapStopWatch.Stop();

                Console.WriteLine("QuickSort: {0}", quickStopWatch.ElapsedTicks);
                Console.WriteLine("HeapSort: {0}", heapStopWatch.ElapsedTicks);
                Console.WriteLine("HeapSort is {0:N2}% Faster",
                    ((double)quickStopWatch.ElapsedTicks - (double)heapStopWatch.ElapsedTicks) / (double)quickStopWatch.ElapsedTicks * 100);
                Console.WriteLine("Or, Quick Sort is {0:N2}% slower",
                    ((double)quickStopWatch.ElapsedTicks - (double)heapStopWatch.ElapsedTicks) / (double)heapStopWatch.ElapsedTicks * 100);

                // TODO: Take average ticks for 100 of 10K items to gauge performance improvements of Heap implementation
                // TODO: Heap Performance improvements 
                //        X Manual Stack instead of Recursion
                //        X Insertion sort when list <= 10
                //        - Make sure recursion does not occur on leaves (trick: throw exception if left-node index is >= n)
                // TODO: Implement Min-Heap
            } while (true);
        }
    }
}
