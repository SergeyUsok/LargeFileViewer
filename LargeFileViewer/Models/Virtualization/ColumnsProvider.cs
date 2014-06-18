using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Detectors;
using LargeFileViewer.Models.Sorting;
using LargeFileViewer.Models.StreamReading;

namespace LargeFileViewer.Models.Virtualization
{
    class ColumnsProvider : IDisposable
    {
        private readonly IndexedStream _indexedStream;
        private readonly Dictionary<string, Type> _columnTypes;
        private readonly string[] _columnsSeparator;

        private ColumnsProvider([NotNull] IndexedStream indexedStream, [NotNull] string columnsSeparator, Dictionary<string, Type> columnTypes)
        {
            if (indexedStream == null) 
                throw new ArgumentNullException("indexedStream");

            if (columnsSeparator == null) 
                throw new ArgumentNullException("columnsSeparator");

            _indexedStream = indexedStream;
            _columnTypes = columnTypes;
            _columnsSeparator = new string[] { columnsSeparator };
        }

        public static ColumnsProvider Create(IndexedStream indexedStream, string columnsSeparator)
        {
            var headers = indexedStream.Header.Split(new[] { columnsSeparator }, StringSplitOptions.None);

            var columnTypes = DetectColumnTypes(indexedStream, columnsSeparator, headers);

            var provider = new ColumnsProvider(indexedStream, columnsSeparator, columnTypes);

            provider.Header = headers;

            return provider;
        }

        private static Dictionary<string, Type> DetectColumnTypes(IndexedStream indexedStream, string columnsSeparator, string[] headers)
        {
            var types = new Dictionary<string, Type>();

            for (int columnNumber = 0; columnNumber < headers.Length; columnNumber++)
            {
                var columnValues = indexedStream.GetColumnsSet(columnNumber, 0, 10, columnsSeparator); // take top 10 rows for a investigation

                var type = ColumnTypeDetector.DetectColumnType(columnValues);

                types.Add(headers[columnNumber], type);
            }

            return types;
        }

        public IEnumerable<FileColumn> GetColumns(int rowNumber)
        {
            var row = _indexedStream.GetLine(rowNumber);

            return CreateColumns(row, ++rowNumber);
        }

        public IEnumerable<FileColumn> GetColumnsSet(string column, Range range)
        {
            var columnIndex = Array.IndexOf(Header, column);

            var index = range.StartIndex - 1;

            return _indexedStream.GetColumnsSet(columnIndex, range.StartIndex, range.Count, _columnsSeparator.First())
                .Select(value => FileColumn.Create(++index, column, value, _columnTypes[column]));
        }

        public IEnumerable<IEnumerable<FileColumn>> GetColumnsForRange(int rowNumber, int count)
        {
            return _indexedStream.GetLines(rowNumber, count)
                                 .Zip(Enumerable.Range(rowNumber, count), (row, number) => CreateColumns(row, number));
        }

        private IEnumerable<FileColumn> CreateColumns(string row, int rowNumber)
        {
            var columns = row.Split(_columnsSeparator, StringSplitOptions.None)
                            .Zip(Header, (value, header) => FileColumn.Create(rowNumber, header, value, typeof (string)));

            yield return FileColumn.Create(rowNumber, "Row #", rowNumber.ToString(), typeof(int)); // in order to show row number

            foreach (var fileColumn in columns)
            {
                yield return fileColumn;
            }
        }

        public void Dispose()
        {
            if (_indexedStream != null)
            {
                _indexedStream.Dispose();
            }
        }

        private string[] Header { get; set; }
     }
}
