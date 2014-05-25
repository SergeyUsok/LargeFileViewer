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
        private bool _isViewEnabled = true;

        public FileViewViewModel([NotNull] FileRowsProvider itemsProvider)
        {
            if (itemsProvider == null) 
                throw new ArgumentNullException("itemsProvider");

            _itemsProvider = itemsProvider;
            _fileRowsCollection = new VirtualizingCollection(itemsProvider);
            _fileRowsCollection.SortingStatusChanged += (sender, args) => IsViewEnabled = !args.IsCurrentlySorting;
        }

        public VirtualizingCollection FileRowsCollection
        {
            get { return _fileRowsCollection; }
        }

        public bool IsViewEnabled
        {
            get { return _isViewEnabled; }
            private set
            {
                _isViewEnabled = value;
                OnPropertyChanged();
            }
        }

        public int RowsCount
        {
            get { return _fileRowsCollection.Count; }
        }

        public void Dispose()
        {
            if (_itemsProvider != null)
                _itemsProvider.Dispose();
        }
    }
}
