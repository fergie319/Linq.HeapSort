using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Linq.HeapSort.Tests
{
    [TestFixture]
    public class PriorityEnumerableTests
    {
        [Test]
        public void PriorityEnumerable_BuildHeap_Success()
        {
            var intArray = new[] { 3, 1, 6, 23, 2, 4, 7, 34, 12, 4, 3, 5, 8, 99, 98, 6, 3, 21, 43 };
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test
            underTest.BuildHeap();

            // Verify Results
            Assert.AreEqual(99, intArray[underTest.HeapMap[0]]);
        }

        [Test]
        public void PriorityEnumerable_Pop_Success()
        {
            var intArray = new[] { 3, 1, 6, 23, 2, 4, 7, 34, 12, 4, 3, 5, 8, 99, 98, 6, 3, 21, 43 };
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test
            underTest.BuildHeap();
            var result = underTest.Pop();

            // Verify Results
            Assert.AreEqual(99, result);
            Assert.AreEqual(98, intArray[underTest.HeapMap[0]]);
        }

        [Test]
        public void PriorityEnumerable_PopMultiple_Success()
        {
            var intArray = new[] { 3, 1, 6, 23, 2, 4, 7, 34, 12, 4, 3, 5, 8, 99, 98, 6, 3, 21, 43 };
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test and verify
            underTest.BuildHeap();
            var result = underTest.Pop();
            Assert.AreEqual(99, result);
            result = underTest.Pop();
            Assert.AreEqual(98, result);
            result = underTest.Pop();
            Assert.AreEqual(43, result);
            result = underTest.Pop();
            Assert.AreEqual(34, result);
        }

        [Test]
        public void PriorityEnumerable_Sort_Success()
        {
            var intArray = new List<int>();
            var expected = new List<int>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 10000; i++)
            {
                var val = rand.Next(10000);
                intArray.Add(val);
                expected.Add(val);
            }
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test and verify
            var result = underTest.Sort().ToList();
            expected.Sort();
            expected.Reverse();
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void PriorityEnumerable_TakeN_OnlyNSorted_Success()
        {
            var intArray = new List<int>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 1000; i++)
            {
                var val = rand.Next(100000);
                intArray.Add(val);
            }
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test and verify
            var result = underTest.Sort().Take(100).ToList();

            Assert.AreEqual(100, underTest.TotalSorted);
        }

        [Test]
        public void PriorityEnumerable_SpeedTest_100_of_100K_Success()
        {
            var intArray = new List<int>();
            var expected = new List<int>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 100000; i++)
            {
                var val = rand.Next(100000);
                intArray.Add(val);
                expected.Add(val);
            }
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = underTest.Sort().Take(100).ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderByDescending(k => k).Take(100).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
        }

        [Test]
        public void PriorityEnumerable_SpeedTest_100K_of_100K_Success()
        {
            var intArray = new List<int>();
            var expected = new List<int>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 100000; i++)
            {
                var val = rand.Next(100000);
                intArray.Add(val);
                expected.Add(val);
            }
            var underTest = new PriorityEnumerable<int, int>(intArray, i => i);

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = underTest.Sort().ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderByDescending(k => k).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
        }
    }
}
