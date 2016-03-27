using System;
using System.Threading.Tasks;
using LargeFileViewer.Models;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.View;

namespace LargeFileViewer.ViewModel.ViewStates
{
    class FileLoadingViewModel : ViewStateViewModel
    {
        private readonly FileLoader _fileLoader;
        private int _loadedPercents;

        public FileLoadingViewModel(string filePath)
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
            return Task.Run(() => _fileLoader.LoadFile(progress => LoadedPercents = progress));
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
    }
}
