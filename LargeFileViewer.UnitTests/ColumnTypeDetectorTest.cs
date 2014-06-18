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
    class ColumnTypeDetectorTest
    {
        [Test, TestCaseSource("TestCases")]
        public Type DetectColumnTypeTest(IEnumerable<string> columnValues)
        {
            //  Arrange
            //var detector = new ColumnTypeDetector();

            // Act, Assert
            return ColumnTypeDetector.DetectColumnType(columnValues);
        }

        private static IEnumerable TestCases
        {
            get
            {
                yield return
                    new TestCaseData(new object[] {new[] {"1", "2", "3", "4", "5"}}).SetName("DetectsThatColumnIsInteger").Returns(typeof (int));
                yield return
                    new TestCaseData(new object[] {new[] {"1,4", "7", "3", "4", "2.3"}}).SetName("DetectsThatColumnIsDouble").Returns(typeof (double));
                yield return
                    new TestCaseData(new object[] { new[] { "2014-05-11", "2014-05-11 12:00:00", "07-12-2014", "2014/03/11" } })
                        .SetName("DetectsThatColumnIsDateTime").Returns(typeof (DateTime));
                yield return
                    new TestCaseData(new object[] { new[] { "True", "trUE", "true", "FaLsE", "false" } }).SetName("DetectsThatColumnIsBoolean").Returns(typeof(bool));
                yield return
                    new TestCaseData(new object[] {new[] {"1", "2", "3", "4", "Some string"}}).SetName("DetectsThatColumnIsString").Returns(typeof (string));
            }
        }   
    }
}
