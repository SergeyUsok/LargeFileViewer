using System;
using System.Collections.Generic;
using LargeFileViewer.Annotations;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Virtualization
{
    class FileRowsProvider : IItemsProvider<FileRow>, IDisposable
    {
        private readonly ColumnsProvider _columnsProvider;
        private readonly int _itemsCount;

        public FileRowsProvider([NotNull] ColumnsProvider columnsProvider, int itemsCount)
        {
            if (columnsProvider == null) 
                throw new ArgumentNullException("columnsProvider");

            _columnsProvider = columnsProvider;
            _itemsCount = itemsCount;
        }

        public int FetchCount()
        {
            return _itemsCount;
        }

        public IEnumerable<FileRow> FetchRange(int start, int count)
        {
            throw new NotImplementedException();
        }

        public FileRow FetchSingle(int itemNumber)
        {
            return new FileRow(_columnsProvider.GetColumns(itemNumber));
        }

        public void Dispose()
        {
            if (_columnsProvider != null)
                _columnsProvider.Dispose();
        }
    }
}
