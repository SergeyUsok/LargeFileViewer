using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting.ExternalSort
{
    class AsyncEnumerator<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, TResult> _projector;
        private readonly BlockingCollection<Task<TResult>> _intermediateResults;
        private IEnumerator<Task<TResult>> _enumerator;

        public AsyncEnumerator(IEnumerable<TSource> source, Func<TSource, TResult> projector, int degreeOfParallelism)
        {
            _source = source;
            _projector = projector;
            _intermediateResults = new BlockingCollection<Task<TResult>>(degreeOfParallelism);
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            Task.Run(() =>
                {
                    foreach (var item in _source)
                    {
                        var current = item;

                        var task = Task.Factory.StartNew(() => _projector(current), TaskCreationOptions.LongRunning);//.Run();

                        _intermediateResults.Add(task);
                    }

                    _intermediateResults.CompleteAdding();
                });

            _enumerator = _intermediateResults.GetConsumingEnumerable().GetEnumerator();

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            if(_enumerator != null)
                _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public TResult Current
        {
            get 
            { 
                return _enumerator.Current.Result;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
