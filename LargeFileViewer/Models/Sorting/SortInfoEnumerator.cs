using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;

namespace LargeFileViewer.Models.Sorting
{
    class SortInfoEnumerator : IEnumerator<SortInfo>
    {
        private readonly string _file;
        private SortInfo _current;
        private readonly StreamReader _streamReader;
        private readonly SortInfoSerializer _serializer = new SortInfoSerializer();

        public SortInfoEnumerator([NotNull] Stream stream)
        {
            if (stream == null) 
                throw new ArgumentNullException("stream");

            _streamReader = new StreamReader(stream);
        }

        public SortInfoEnumerator(string file)
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
                _current = _serializer.Deserialize(line);

            return moveNext;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public SortInfo Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
