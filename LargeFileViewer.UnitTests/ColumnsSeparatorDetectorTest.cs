using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Detectors;
using NUnit.Framework;

namespace LargeFileViewer.UnitTests
{
    [TestFixture]
    class ColumnsSeparatorDetectorTest
    {
        [Test, TestCaseSource("TestCases")]
        public string DetectColumnSeparatorTest(IEnumerable<string> rows)
        {
            // Arrange, Act, Assert
            return ColumnsSeparatorDetector.DetectColumnSeparator(rows);
        }

        private static IEnumerable TestCases
        {
            get
            {
                yield return
                    new TestCaseData(new object[]
                        {new[] {"column1_column2_column3", "column1_column2_column3", "column1_column2_column3"}})
                        .SetName("Detects Underscore As Separator").Returns("_");
                yield return
                    new TestCaseData(new object[]
                        {new[] {"column1\tcolumn2\tcolumn3", "column1\tcolumn2\tcolumn3", "column1\tcolumn2\tcolumn3"}})
                        .SetName("Detects Tab As Separator").Returns("\t");
                yield return
                    new TestCaseData(new object[]
                        {
                            new[] {"col_umn1,col_umn2,col_umn3", "col_umn1,column2,c_olumn3", "colu_mn1,co_lumn2,co_lumn3"}
                        })
                        .SetName("Detects Comma As Separator").Returns(",");
            }
        }
    }
}
