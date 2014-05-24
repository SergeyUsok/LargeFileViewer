using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
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

            return CreateColumns(row, rowNumber);
        }

        public IEnumerable<IEnumerable<FileColumn>> GetColumnsForRange(int rowNumber, int count)
        {
            return _indexedStream.GetLines(rowNumber, count)
                                 .Zip(Enumerable.Range(rowNumber, count), (row, number) => CreateColumns(row, number));
        }

        private IEnumerable<FileColumn> CreateColumns(string row, int rowNumber)
        {
            var columns = row.Split(_columnsSeparator, StringSplitOptions.None)
                            .Zip(Header, (value, header) => FileColumn.Create(header, value, typeof (string)));

            yield return FileColumn.Create("Row #", rowNumber, typeof(int)); // in order to show row number

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
