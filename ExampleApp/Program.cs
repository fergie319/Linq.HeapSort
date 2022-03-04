using Linq.HeapSort;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ExampleApp
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            Console.WriteLine(
@"This simple app will run sorting time comparisons between
the PriorityEnumerable and the default OrderBy with randomly generated
integers ranging from 0-10,000.

The OrderBy implementation has to sort the entire list up front, so
it does not matter whether you take 5 or 500 elements in your list,
it will always take the same amount of time.");
            Console.WriteLine("Type 'Exit' at any time to terminate the program");
            
            var input = string.Empty;
            do
            {
                // Read in and parse the size of the list to test
                Console.WriteLine("Enter size of list to sort:");
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    break;
                }

                var size = int.Parse(input, System.Globalization.NumberStyles.AllowThousands);

                // Read in and parse the number of samples to run per test
                Console.WriteLine("Enter number of samples per test run:");
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    break;
                }

                var samples = int.Parse(input, System.Globalization.NumberStyles.AllowThousands);

                // Read in and parse the value to increment the 'take' value by for each test
                Console.WriteLine("Enter the number to increment each test by (the first test use this value):");
                input = Console.ReadLine();
                if (input == "Exit")
                {
                    break;
                }

                var increment = int.Parse(input, System.Globalization.NumberStyles.AllowThousands);
                var take = increment;
                while (take <= size)
                {
                    Console.WriteLine("Testing Take = {0}", take);

                    var quickStopWatch = new Stopwatch();
                    var heapStopWatch = new Stopwatch();
                    for (var i = 0; i < samples; i++)
                    {
                        // Generate two identical lists of random numbers
                        var testListOne = new List<int>();
                        var testListTwo = new List<int>();
                        var rand = new Random(DateTime.Now.Millisecond);
                        for (var j = 0; j < size; j++)
                        {
                            var val = rand.Next(10000);
                            testListOne.Add(val);
                            testListTwo.Add(val);
                        }

                        // Test OrderBy Ascending
                        quickStopWatch.Start();
                        var quickResult = testListOne.OrderByDescending(k => k).Take(take).ToList();
                        quickStopWatch.Stop();

                        var priorityEnumerable = new PriorityEnumerable<int, int>(testListTwo, t => t);
                        heapStopWatch.Start();
                        var result = priorityEnumerable.Sort().Take(take).ToList();
                        heapStopWatch.Stop();
                    }

                    // Log the total ticks for the heap sort
                    var logData = new LoggingEventData();
                    logData.Level = Level.Debug;
                    logData.Properties = new PropertiesDictionary();
                    logData.Properties["HeapElapsedTicks"] = heapStopWatch.ElapsedTicks;
                    logData.Properties["QuickElapsedTicks"] = quickStopWatch.ElapsedTicks;
                    logData.Properties["QuickMinusHeapDifference"] = quickStopWatch.ElapsedTicks - heapStopWatch.ElapsedTicks;
                    logData.Properties["Size"] = size;
                    logData.Properties["Take"] = take;
                    logData.Properties["Samples"] = samples;
                    var logEvent = new LoggingEvent(logData);
                    Log.Logger.Log(logEvent);

                    // Increment take for the next test
                    take += increment;
                }
            } while (true);
        }
    }
}
