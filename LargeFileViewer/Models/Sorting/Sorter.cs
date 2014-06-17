using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Sorting.ExternalSort;
using LargeFileViewer.Models.Virtualization;

namespace LargeFileViewer.Models.Sorting
{
    class Sorter
    {
        private readonly Dictionary<string, SortedRowsProvider> _providersCache = new Dictionary<string, SortedRowsProvider>();
        
        private const int ChunkSize = 500000;

        public FileRowsProvider OriginalProvider { get; private set; }

        public Sorter([NotNull] FileRowsProvider originalProvider)
        {
            if (originalProvider == null) 
                throw new ArgumentNullException("originalProvider");

            OriginalProvider = originalProvider;
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
            return GenerateRanges(OriginalProvider.FetchCount(), ChunkSize)
                                //.Chunkify(ChunkSize)
                                .Select(range => OriginalProvider.ColumnsProvider.GetColumnsSet(column, range))
                                .SortAscendingAsync(fc => fc.Value)
                                .SavePartialResult(fc => fc.ToString())
                                .Select(file => new FileInfo(file).Deserialize().GetEnumerator())
                                .MergeAscending(fc => fc.Value)
                                .Select(fc => fc.ParentRowIndex)
                                .ToList();
        }

        private List<int> ApplyInMemorySort(string column)
        {
            return GenerateRanges(OriginalProvider.FetchCount(), OriginalProvider.FetchCount())
                                .SelectMany(range => OriginalProvider.ColumnsProvider.GetColumnsSet(column, range))
                                .OrderBy(fc => fc.Value)
                                .Select(fc => fc.ParentRowIndex)
                                .ToList();
        }

        private bool IsChunkingReqiured()
        {
            const int halfOfChunkSize = ChunkSize/2;

            return OriginalProvider.FetchCount() > (ChunkSize + halfOfChunkSize);
        }

        private IEnumerable<Range> GenerateRanges(int totalCount, int rangeCount)
        {
            var startIndex = 0;

            for (; (startIndex + rangeCount) < totalCount; startIndex += rangeCount)
            {
                yield return new Range { Count = rangeCount, StartIndex = startIndex };
            }

            if (totalCount > startIndex)
            {
                yield return new Range { StartIndex = startIndex, Count = totalCount - startIndex };
            }
        }
    }
}
