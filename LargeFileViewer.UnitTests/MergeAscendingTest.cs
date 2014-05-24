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
    public class MergeAscendingTest
    {
        [Test]
        public void MergeTest()
        {
            // Arrange
            var enumerators = GetDataForTest().Select(e => e.GetEnumerator());
            var expected = GetExpectedResult();

            // Act
            var sw = Stopwatch.StartNew();
            var actual = ExternalSortPipeline.MergeAscending(enumerators, i => i).ToList(); // force enumeration in order to get actual time of merging
            sw.Stop();
            
            // Assert
            CollectionAssert.AreEqual(expected, actual);
            
            Console.WriteLine(@"Merge took {0} milliseconds", sw.ElapsedMilliseconds);
        }

        private IEnumerable<IEnumerable<int>> GetDataForTest()
        {
            for (int i = 0; i < 100; i++)
            {
                yield return Enumerable.Range(0, 200000);
            }
        }

        private IEnumerable<int> GetExpectedResult()
        {
            for (int i = 0; i < 200000; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    yield return i;
                }
            }
        }
    }
}
