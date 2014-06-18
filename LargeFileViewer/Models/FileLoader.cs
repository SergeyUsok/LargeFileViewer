using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Detectors;
using LargeFileViewer.Models.StreamReading;
using LargeFileViewer.Models.Virtualization;

namespace LargeFileViewer.Models
{
    class FileLoader
    {
        private readonly string _filePath;
        private string _columnsSeparator;

        public FileLoader([NotNull] string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) 
                throw new ArgumentException("filePath is null or empty string");

            _filePath = filePath;
        }

        public FileRowsProvider LoadFile(Action counter)
        {
            var stream = OpenStream();

            FileLength = stream.Length;

            var indexer = new StreamIndexer();

            var indexedStream = indexer.GetIndexedStream(stream, counter);

            return new FileRowsProvider(ColumnsProvider.Create(indexedStream, ColumnsSeparator), indexedStream.RowsCount);
        }

        public long FileLength { get; private set; }

        public string ColumnsSeparator
        {
            get
            {
                if (_columnsSeparator == null)
                    _columnsSeparator = DetectColumnsSeparator();

                return _columnsSeparator;
            }
            set { _columnsSeparator = value; }
        }

        protected virtual Stream OpenStream()
        {
            return File.OpenRead(_filePath);
        }

        private string DetectColumnsSeparator()
        {
            return ColumnsSeparatorDetector.DetectColumnSeparator(GetRows());
        }

        private IEnumerable<string> GetRows()
        {
            var counter = 0;
            var linesLimit = 5;

            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && counter != linesLimit)
                {
                    yield return line;
                    counter++;
                }
            }
        }
    }
}
