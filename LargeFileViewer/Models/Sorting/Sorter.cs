using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Sorting
{
    class Sorter
    {
        private readonly Dictionary<string, SortedRowsProvider> _providersCache = new Dictionary<string, SortedRowsProvider>();
        private readonly MergingSorter _mergingSorter = new MergingSorter();
        private readonly ChunkCreator _chunkCreator;
        private readonly SortInfoSerializer _serializer = new SortInfoSerializer();

        private const int ChunkSize = 500000;

        public IItemsProvider<FileRow> OriginalProvider { get; private set; }

        public Sorter([NotNull] IItemsProvider<FileRow> originalProvider)
        {
            if (originalProvider == null) 
                throw new ArgumentNullException("originalProvider");

            OriginalProvider = originalProvider;
            _chunkCreator = new ChunkCreator(ChunkSize, originalProvider);
        }

        public Task<SortedRowsProvider> Sort(string column, ListSortDirection direction)
        {
            return Task.Run(() =>
                {
                    var provider = _providersCache.ContainsKey(column)
                                       ? _providersCache[column]
                                       : new SortedRowsProvider(OriginalProvider, SortByColumn(column));
                    
                    if(!_providersCache.ContainsKey(column))
                        _providersCache.Add(column, provider);

                    provider.Direction = direction;

                    return provider;
                });
        }

        private List<int> SortByColumn(string column)
        {
            if (IsChunkingReqiured())
                return ApplyExternalSort(column);

            return ApplyInMemorySort(column);
        }

        private List<int> ApplyExternalSort(string column)
        {
            var chunks = ConsumeChunks(column);

            var partialResults = SortChunks(chunks);

            return _mergingSorter.MergeAscendingSort(partialResults).ToList();
        }

        private List<int> ApplyInMemorySort(string column)
        {
            return _chunkCreator.Chunkify(column)
                                .SelectMany(c => c)
                                .OrderBy(si => si.ColumnValue)
                                .Select(si => si.RowIndex)
                                .ToList();
        }

        private List<Task<string>> SortChunks(IEnumerable<IEnumerable<SortInfo>> chunks)
        {
            var results = new List<Task<string>>();

            Task.Run(() =>
                {
                    foreach (var chunk in chunks)
                    {
                        var currentChunk = chunk; // avoid access to modified closure

                        var res = Task.Run(() => currentChunk.OrderBy(si => si.ColumnValue))
                                      .ContinueWith(t => SavePartialResult(t.Result));

                        results.Add(res);
                    }
                });

            return results;
        }

        private IEnumerable<IEnumerable<SortInfo>> ConsumeChunks(string column)
        {
            var blockingQueue = new BlockingCollection<IEnumerable<SortInfo>>();

            Task.Run(() =>
                {
                    foreach (var chunk in _chunkCreator.Chunkify(column))
                    {
                        blockingQueue.Add(chunk);
                    }

                    blockingQueue.CompleteAdding();
                });

            return blockingQueue.GetConsumingEnumerable();
        }

        private string SavePartialResult(IEnumerable<SortInfo> sortInfos)
        {
            var tempFile = Path.GetTempFileName();

            File.WriteAllLines(tempFile, sortInfos.Select(si => _serializer.Serialize(si)));

            return tempFile;
        }

        private bool IsChunkingReqiured()
        {
            const int halfOfChunkSize = ChunkSize/2;

            return OriginalProvider.FetchCount() > (ChunkSize + halfOfChunkSize);
        }
    }
}
