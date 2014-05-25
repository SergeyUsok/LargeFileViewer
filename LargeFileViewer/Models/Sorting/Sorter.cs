using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Sorting.ExternalSort;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Sorting
{
    class Sorter
    {
        private readonly Dictionary<string, SortedRowsProvider> _providersCache = new Dictionary<string, SortedRowsProvider>();
        private readonly SortInfoFactory _sortInfoFactory = new SortInfoFactory();
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
            return
                _sortInfoFactory.GetSortInfos(OriginalProvider, column)
                                .Chunkify(ChunkSize)
                                .SortAscendingAsync(si => si.ColumnValue)
                                .SavePartialResult(si => si.ToString())
                                .Select(f => new SortInfoEnumerator(f))
                                .MergeAscending(si => si.ColumnValue)
                                .Select(si => si.RowIndex)
                                .ToList();
        }

        private List<int> ApplyInMemorySort(string column)
        {
            return
                _sortInfoFactory.GetSortInfos(OriginalProvider, column)
                                .OrderBy(si => si.ColumnValue)
                                .Select(si => si.RowIndex)
                                .ToList();
        }

        private bool IsChunkingReqiured()
        {
            const int halfOfChunkSize = ChunkSize/2;

            return OriginalProvider.FetchCount() > (ChunkSize + halfOfChunkSize);
        }
    }
}
