using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Sorting
{
    class SortInfoFactory
    {
        public IEnumerable<SortInfo> GetSortInfos(IItemsProvider<FileRow> provider, string column)
        {
            var index = 0;

            return GenerateRanges(provider.FetchCount(), 500000)
                .SelectMany(range => provider.FetchRange(range.StartIndex, range.Count),
                            (range, row) => CreateSortInfo(++index, row[column]));
        }

        private SortInfo CreateSortInfo(int index, FileColumn column)
        {
            return SortInfo.Create(index, column.Value, column.Type);
        }

        private IEnumerable<Range> GenerateRanges(int totalCount, int rangeCount)
        {
            var startIndex = 0;

            for (; (startIndex + rangeCount) < totalCount; startIndex += rangeCount)
            {
                yield return new Range {Count = rangeCount, StartIndex = startIndex};
            }

            if (totalCount > startIndex)
            {
                yield return new Range {StartIndex = startIndex, Count = totalCount - startIndex};
            }
        }
    }
}
