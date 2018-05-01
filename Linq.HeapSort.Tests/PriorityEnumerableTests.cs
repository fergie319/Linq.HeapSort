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
        public void PriorityEnumerable_Sort_Success()
        {
            for (var j = 0; j < 100; j++)
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

                var underTest = new PriorityEnumerable<int, int>(intArray, i => i, true);

                // Execute Test and verify
                var result = underTest.Sort().ToList();
                expected.Sort();
                CollectionAssert.AreEqual(expected, result);
            }
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
            var quickResult = expected.OrderBy(k => k).Take(100).ToList();
            quickStopWatch.Stop();
            CollectionAssert.AreEqual(quickResult, result);

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
        }

        [Test]
        public void PriorityEnumerable_SpeedTest_5K_of_10K_Success()
        {
            var rand = new Random(DateTime.Now.Millisecond);

            var heapStopWatch = new Stopwatch();
            var quickStopWatch = new Stopwatch();
            for (var i = 0; i < 100; i++)
            {
                var intArray = new List<int>();
                var expected = new List<int>();

                for (var j = 0; j < 10000; j++)
                {
                    var val = rand.Next(100000);
                    intArray.Add(val);
                    expected.Add(val);
                }
                var underTest = new PriorityEnumerable<int>(intArray, true);

                // Execute Test and verify
                heapStopWatch.Start();
                var result = underTest.Sort().Take(2500).ToList();
                heapStopWatch.Stop();

                quickStopWatch.Start();
                var quickResult = expected.OrderBy(k => k).Take(2500).ToList();
                quickStopWatch.Stop();
                CollectionAssert.AreEqual(quickResult, result);
            }

            Assert.Greater(quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderBy_SpeedTest_2500K_of_10K_Success()
        {
            var rand = new Random(DateTime.Now.Millisecond);

            var heapStopWatch = new Stopwatch();
            var quickStopWatch = new Stopwatch();
            for (var i = 0; i < 100; i++)
            {
                var intArray = new List<int>();
                var expected = new List<int>();

                for (var j = 0; j < 10000; j++)
                {
                    var val = rand.Next(100000);
                    intArray.Add(val);
                    expected.Add(val);
                }

                // Execute Test and verify
                heapStopWatch.Start();
                var result = intArray.PriorityOrderBy().Take(2500).ToList();
                heapStopWatch.Stop();

                quickStopWatch.Start();
                var quickResult = expected.OrderBy(k => k).Take(2500).ToList();
                quickStopWatch.Stop();
                CollectionAssert.AreEqual(quickResult, result);
            }

            Assert.Greater(quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderBy_KeySelector_SpeedTest_100_of_10K_Success()
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

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = intArray.PriorityOrderBy(k => k).Take(100).ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderBy(k => k).Take(100).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
            CollectionAssert.AreEqual(quickResult, result);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderBy_KeySelector_WithComparer_SpeedTest_100_of_10K_Success()
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

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = intArray.PriorityOrderBy(k => k, Comparer<int>.Default).Take(100).ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderBy(k => k).Take(100).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
            CollectionAssert.AreEqual(quickResult, result);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderByDescending_SpeedTest_2500K_of_10K_Success()
        {
            var rand = new Random(DateTime.Now.Millisecond);

            var heapStopWatch = new Stopwatch();
            var quickStopWatch = new Stopwatch();
            for (var i = 0; i < 100; i++)
            {
                var intArray = new List<int>();
                var expected = new List<int>();

                for (var j = 0; j < 10000; j++)
                {
                    var val = rand.Next(100000);
                    intArray.Add(val);
                    expected.Add(val);
                }

                // Execute Test and verify
                heapStopWatch.Start();
                var result = intArray.PriorityOrderByDescending().Take(2500).ToList();
                heapStopWatch.Stop();

                quickStopWatch.Start();
                var quickResult = expected.OrderByDescending(k => k).Take(2500).ToList();
                quickStopWatch.Stop();
                CollectionAssert.AreEqual(quickResult, result);
            }

            Assert.Greater(quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks / 100, heapStopWatch.ElapsedTicks / 100);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderByDescending_KeySelector_SpeedTest_100_of_10K_Success()
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

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = intArray.PriorityOrderByDescending(k => k).Take(100).ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderByDescending(k => k).Take(100).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
            CollectionAssert.AreEqual(quickResult, result);
        }

        [Test]
        public void PriorityExtensions_PriorityOrderByDescending_KeySelector_WithComparer_SpeedTest_100_of_10K_Success()
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

            // Execute Test and verify
            var heapStopWatch = new Stopwatch();
            heapStopWatch.Start();
            var result = intArray.PriorityOrderByDescending(k => k, Comparer<int>.Default).Take(100).ToList();
            heapStopWatch.Stop();

            var quickStopWatch = new Stopwatch();
            quickStopWatch.Start();
            var quickResult = expected.OrderByDescending(k => k).Take(100).ToList();
            quickStopWatch.Stop();

            Assert.Greater(quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks, "Expected {0} to be greater than {1}", quickStopWatch.ElapsedTicks, heapStopWatch.ElapsedTicks);
            CollectionAssert.AreEqual(quickResult, result);
        }
    }
}
