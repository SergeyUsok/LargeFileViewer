using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LargeFileViewer.Models.Sorting;

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

            var buffer = CalculateBuffer(lineNumber, 1);

            _stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer).TrimEnd('\r', '\n');
        }

        public IEnumerable<string> GetLines(int startLineNumber, int count)
        {
            _stream.Position = _lineIndexes[startLineNumber];

            var buffer = CalculateBuffer(startLineNumber, count);

            _stream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer)
                           .Split('\n').Select(line => line.TrimEnd('\r'));
        }

        public IEnumerable<string> GetColumnsSet(int columnNumber, int startLine, int count, string columnSeparator)
        {
            _stream.Position = _lineIndexes[startLine];

            var buffer = CalculateBuffer(startLine, count);

            _stream.Read(buffer, 0, buffer.Length);

            var separator = Encoding.UTF8.GetBytes(columnSeparator).First();

            return GetRawColumns(buffer, columnNumber, separator)
                    .Select(rawColumn => Encoding.UTF8.GetString(rawColumn).TrimEnd('\r', '\n'))
                    .ToList(); // enforce enumeration in order to avoid parrallel access to stream
        }

        private IEnumerable<byte[]> GetRawColumns(byte[] buffer, int columnNumber, byte separator)
        {
            var currentColumn = 0; // column enumeration starts from 0

            for (int i = 0; i < buffer.Length; i++)
            {
                if (currentColumn == columnNumber)
                {
                    var startIndex = i;
                    
                    while (buffer[i] != separator && buffer[i] != 10)
                    {
                        i++;
                    }

                    var length = i - startIndex;

                    yield return buffer.SubArray(startIndex, length);
                }

                if (buffer[i] == separator)
                    currentColumn++;

                if (buffer[i] == 10) // if end of line is reached reset column number to 0
                    currentColumn = 0;
            }
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        private byte[] CalculateBuffer(int lineNumber, int count)
        {
            int bufferSize;
            var endLine = lineNumber + count;

            if (endLine >= _lineIndexes.Count) // check if last line was requested
            {
                bufferSize = (int)(_stream.Length - _lineIndexes[lineNumber]);
            }
            else
            {
                bufferSize = (int)(_lineIndexes[endLine] - _lineIndexes[lineNumber]);
            }

            return new byte[bufferSize];
        }
    }
}