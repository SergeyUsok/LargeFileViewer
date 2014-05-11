using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Sorting
{
    class SortedRowsProvider : IItemsProvider<FileRow>
    {
        private readonly IItemsProvider<FileRow> _itemsProvider;
        private readonly List<int> _sortedIndexes; 

        public SortedRowsProvider(IItemsProvider<FileRow> itemsProvider, List<int> sortedIndexes)
        {
            _itemsProvider = itemsProvider;
            _sortedIndexes = sortedIndexes;
        }

        public int FetchCount()
        {
            return _itemsProvider.FetchCount();
        }

        public IEnumerable<FileRow> FetchRange(int start, int count)
        {
            return Enumerable.Range(start, count)
                             .Select(i => FetchSingle(i));
        }

        public FileRow FetchSingle(int itemNumber)
        {
            itemNumber = Direction == ListSortDirection.Ascending
                             ? itemNumber
                             : _itemsProvider.FetchCount() - 1 - itemNumber;

            return _itemsProvider.FetchSingle(_sortedIndexes[itemNumber]);
        }

        public ListSortDirection Direction { get; set; }
    }
}
