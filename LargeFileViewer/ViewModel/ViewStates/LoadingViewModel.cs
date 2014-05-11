using System;
using System.Threading.Tasks;
using LargeFileViewer.Models;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.View;

namespace LargeFileViewer.ViewModel.ViewStates
{
    class LoadingViewModel : ViewStateViewModel
    {
        private readonly FileLoader _fileLoader;
        private int _loadedPercents;
        private long _counter;

        public LoadingViewModel(string filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath is null or empty string");

            _fileLoader = new FileLoader(filePath);
        }

        public int LoadedPercents
        {
            get { return _loadedPercents; }
            set
            {
                if (_loadedPercents != value)
                {
                    _loadedPercents = value;
                    OnPropertyChanged();
                } 
            }
        }

        public Task<FileRowsProvider> LoadFile()
        {
            return Task.Run(() => _fileLoader.LoadFile(() => ++Counter));
        }

        public bool DetectColumnsSeparator()
        {
            var window = new SeparatorSelectorView();
            var viewModel = new SeparatorSelectorViewModel(_fileLoader.ColumnsSeparator);
            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result.HasValue && result.Value)
            {
                _fileLoader.ColumnsSeparator = viewModel.ResultingSeparator;
                return true;
            }

            return false;
        }

        private long Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                LoadedPercents = ((int)(100 * _counter / _fileLoader.FileLength));
            }
        }
    }
}
