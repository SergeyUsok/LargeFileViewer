using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Sorting;
using LargeFileViewer.Models.StreamReading;

namespace LargeFileViewer.Models.Virtualization
{
    class ColumnsProvider : IDisposable
    {
        private readonly IndexedStream _indexedStream;
        private readonly string[] _columnsSeparator;

        private ColumnsProvider([NotNull] IndexedStream indexedStream, [NotNull] string columnsSeparator)
        {
            if (indexedStream == null) 
                throw new ArgumentNullException("indexedStream");

            if (columnsSeparator == null) 
                throw new ArgumentNullException("columnsSeparator");

            _indexedStream = indexedStream;
            _columnsSeparator = new string[] { columnsSeparator };
        }

        public static ColumnsProvider Create(IndexedStream indexedStream, string columnsSeparator)
        {
            var provider = new ColumnsProvider(indexedStream, columnsSeparator);

            provider.Header = indexedStream.Header.Split(provider._columnsSeparator, StringSplitOptions.None);

            return provider;
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
            var type = GetColumnType(column);

            return _indexedStream.GetColumnsSet(columnIndex, range.StartIndex, range.Count, _columnsSeparator.First())
                .Select(value => FileColumn.Create(++index, column, value, type));
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

        private Type GetColumnType(string column)
        {
            return typeof (string);
        }

        private string[] Header { get; set; }
     }
}
