using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Virtualization;

namespace LargeFileViewer.Models.Sorting
{
    class FileColumnEnumerator : IEnumerator<FileColumn>
    {
        private readonly string _file;
        private FileColumn _current;
        private readonly StreamReader _streamReader;

        public FileColumnEnumerator([NotNull] Stream stream)
        {
            if (stream == null) 
                throw new ArgumentNullException("stream");

            _streamReader = new StreamReader(stream);
        }

        public FileColumnEnumerator(string file)
            :this(File.OpenRead(file))
        {
            _file = file;
        }

        public void Dispose()
        {
            if (_streamReader != null)
            {
                _streamReader.Dispose();
            }

            if (File.Exists(_file))
            {
                File.Delete(_file);
            }
        }

        public bool MoveNext()
        {
            var line = _streamReader.ReadLine();

            var moveNext = line != null;

            if (moveNext)
                _current = FileColumn.FromString(line);

            return moveNext;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public FileColumn Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
