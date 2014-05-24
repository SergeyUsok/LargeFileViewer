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

        private const int ChunkSize = 500000;

        public IItemsProvider<FileRow> OriginalProvider { get; private set; }

        public Sorter([NotNull] IItemsProvider<FileRow> originalProvider)
        {
            if (originalProvider == null) 
                throw new ArgumentNullException("originalProvider");

            OriginalProvider = originalProvider;
            //_chunkCreator = new ChunkCreator(ChunkSize, originalProvider);
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
            throw new NotImplementedException();
        }

        private List<int> ApplyInMemorySort(string column)
        {
            throw new NotImplementedException();
        }

        private bool IsChunkingReqiured()
        {
            const int halfOfChunkSize = ChunkSize/2;

            return OriginalProvider.FetchCount() > (ChunkSize + halfOfChunkSize);
        }
    }
}
