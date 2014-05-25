using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Sorting.ExternalSort;
using NUnit.Framework;

namespace LargeFileViewer.PerformanceTests
{
    [TestFixture]
    public class SortAsyncTest
    {
        [Test]
        public void SortAscendingAsyncTest_WithDegreeOfParallelismEqualsToProsessorsCount()
        {
            // Arrange
            var dataForTest = GetDataForTest();

            var sw = Stopwatch.StartNew();
            // Act
            var result = ExternalSortPipeline.SortAscendingAsync(dataForTest, i => i).ToList();

            sw.Stop();
            Console.WriteLine(result.Count);
            Console.WriteLine(@"Sort took {0} milliseconds", sw.ElapsedMilliseconds);
            // Assert

        }

        [Test]
        public void SortAscendingAsyncTest_WithDegreeOfParallelismGreaterThanProcessorsCount()
        {
            // Arrange
            var dataForTest = GetDataForTest();
            var degreeOfParallelism = Environment.ProcessorCount*2;

            var sw = Stopwatch.StartNew();
            // Act
            var result = ExternalSortPipeline.SortAscendingAsync(dataForTest, i => i, degreeOfParallelism).ToList();

            sw.Stop();

            Console.WriteLine(@"Sort took {0} milliseconds", sw.ElapsedMilliseconds);
            // Assert
        }

        [Test]
        public void SortAscendingAsyncTest_WithDegreeOfParallelismEqualsTo1()
        {
            // Arrange
            var dataForTest = GetDataForTest();

            var sw = Stopwatch.StartNew();
            // Act
            var result = ExternalSortPipeline.SortAscendingAsync(dataForTest, i => i, 1).ToList();

            sw.Stop();

            Console.WriteLine(@"Sort took {0} milliseconds", sw.ElapsedMilliseconds);
            // Assert
        }

        public IEnumerable<IEnumerable<int>> GetDataForTest()
        {
            var random = new Random(0);

            return Enumerable.Range(0, 20).Select(i => Enumerable.Range(0, 200000).Select(j => random.Next()));
        }
    }
}
