using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LargeFileViewer.Models.StreamReading
{
    internal class IndexedStream : IDisposable
    {
        private readonly List<long> _lineIndexes;
        private readonly Stream _stream;

        public IndexedStream(string header, Stream stream, List<long> lineIndexes)
        {
            _stream = stream;
            _lineIndexes = lineIndexes;
            Header = header;
        }

        public string Header { get; private set; }

        public int RowsCount
        {
            get { return _lineIndexes.Count; }
        }

        public string GetLine(int lineNumber)
        {
            _stream.Position = _lineIndexes[lineNumber];

            var buffer = CalculateBuffer(lineNumber);

            _stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer).TrimEnd('\r', '\n');
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        private byte[] CalculateBuffer(int lineNumber)
        {
            long bufferSize;

            if (lineNumber != (_lineIndexes.Count - 1)) // check if request last line or not
            {
                bufferSize = _lineIndexes[lineNumber + 1] - _lineIndexes[lineNumber];
            }
            else
            {
                bufferSize = _stream.Length - _lineIndexes[lineNumber];
            }

            return new byte[bufferSize];
        }
    }
}