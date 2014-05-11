using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LargeFileViewer.Models.StreamReading
{
    class StreamIndexer
    {
        public IndexedStream GetIndexedStream(Stream stream, Action counter)
        {
            DetectHeaderStartPosition(stream);

            var header = GetHeader(stream);

            var rowIndexes = GetRowsIndexes(stream, counter);

            return new IndexedStream(header, stream, rowIndexes);
        }

        private string GetHeader(Stream stream)
        {
            var headerBytes = new List<byte>();
            var byteRead = 0;
            
            while ((byteRead = stream.ReadByte()) != -1 && byteRead != 10)
            {
                headerBytes.Add((byte)byteRead);
            }

            return Encoding.UTF8.GetString(headerBytes.ToArray()).TrimEnd('\r','\n');
        }

        private void DetectHeaderStartPosition(Stream stream)
        {
            var maybeBom = new byte[3];
            stream.Read(maybeBom, 0, maybeBom.Length);

            if (!IsBomUsed(maybeBom))
            {
                stream.Position = 0; // if BOM does not exist set Position back to beginning
            }
        }

        private bool IsBomUsed(IEnumerable<byte> maybeBom)
        {
            return Encoding.UTF8.GetPreamble().SequenceEqual(maybeBom);
        }

        private List<long> GetRowsIndexes(Stream stream, Action counter)
        {
            var rowIndexes = new List<long>();
            var byteRead = 0; 
            rowIndexes.Add(stream.Position); // remember first line start position

            while ((byteRead = stream.ReadByte()) != -1) // where -1 - is end of stream
            {
                if (IsEndOfLine(byteRead))
                {
                    rowIndexes.Add(stream.Position);
                }

                counter();
            }

            return rowIndexes;
        }

        private bool IsEndOfLine(int byteToCheck)
        {
            return byteToCheck == 10; // where 10 - byte representation of end of line '\n'
        }
    }
}
