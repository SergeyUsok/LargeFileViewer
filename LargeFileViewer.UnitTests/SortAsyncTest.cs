using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Sorting.ExternalSort;
using NUnit.Framework;

namespace LargeFileViewer.UnitTests
{
    [TestFixture]
    class SortAsyncTest
    {
        [Test]
        public void SortAscendingAsyncTest()
        {
            // Arrange
            var dataForTest = GetDataForTest();

            var sw = Stopwatch.StartNew();
            // Act
            var result = ExternalSortPipeline.SortAscendingAsync(dataForTest, i => i).ToList();

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);

            // Assert
            foreach (var partialResult in result)
            {
                CollectionAssert.AreEqual(Enumerable.Range(0, 500000), partialResult.Part);
            }
        }

        public IEnumerable<IEnumerable<int>> GetDataForTest()
        {
            var test = Enumerable.Range(0, 3).Select(i => Enumerable.Range(0, 500000).ToList());

            foreach (var list in test)
            {
                list.Reverse();
                yield return list;
            }
        }
    }
}
