using System;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.ViewModel.ViewStates
{
    class FileViewViewModel : ViewStateViewModel, IDisposable
    {
        private readonly VirtualizingCollection _fileRowsCollection;
        private readonly FileRowsProvider _itemsProvider;

        public FileViewViewModel([NotNull] FileRowsProvider itemsProvider)
        {
            if (itemsProvider == null) 
                throw new ArgumentNullException("itemsProvider");

            _itemsProvider = itemsProvider;
            _fileRowsCollection = new VirtualizingCollection(itemsProvider);
        }

        public VirtualizingCollection FileRowsCollection
        {
            get { return _fileRowsCollection; }
        }

        public void Dispose()
        {
            if (_itemsProvider != null)
                _itemsProvider.Dispose();
        }
    }
}
