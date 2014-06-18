using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Detectors
{
    internal static class ColumnTypeDetector
    {
        public static Type DetectColumnType(IEnumerable<string> columnValues)
        {
            return IsColumnInterger(columnValues)
                       ? typeof(int)
                       : IsColumnDouble(columnValues)
                             ? typeof(double)
                             : IsColumnBoolean(columnValues)
                                   ? typeof(bool)
                                   : IsColumnDateTime(columnValues) ? typeof(DateTime) : typeof(string);
        }

        private static bool IsColumnInterger(IEnumerable<string> columnValues)
        {
            foreach (var columnValue in columnValues)
            {
                int value;
                if (!int.TryParse(columnValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    return false;
            }

            return true;
        }

        private static bool IsColumnDouble(IEnumerable<string> columnValues)
        {
            foreach (var columnValue in columnValues)
            {
                double value;
                if (!double.TryParse(columnValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    return false;
            }

            return true;
        }

        private static bool IsColumnBoolean(IEnumerable<string> columnValues)
        {
            foreach (var columnValue in columnValues)
            {
                bool value;
                if (!bool.TryParse(columnValue, out value))
                    return false;
            }

            return true;
        }

        private static bool IsColumnDateTime(IEnumerable<string> columnValues)
        {
            foreach (var columnValue in columnValues)
            {
                DateTime value;
                if (
                    !DateTime.TryParse(columnValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out value))
                    return false;
            }

            return true;
        }
    }
}
