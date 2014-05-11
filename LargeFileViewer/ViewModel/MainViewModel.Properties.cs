using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LargeFileViewer.ViewModel.ViewStates;

namespace LargeFileViewer.ViewModel
{
    partial class MainViewModel
    {
        private string _fileName;
        private ViewStateViewModel _currentViewState;
        private string _memoryUsage;

        private ICommand _openFileCommand;
        private ICommand _closeFileCommand;
        private ICommand _saveFileCommand;

        #region Properties

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public ViewStateViewModel CurrentViewState
        {
            get { return _currentViewState; }
            set
            {
                _currentViewState = value;
                OnPropertyChanged();
            }
        }

        public string MemoryUsage
        {
            get { return _memoryUsage; }
            set
            {
                _memoryUsage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand OpenFileCommand
        {
            get
            {
                if (_openFileCommand == null)
                    _openFileCommand = new RelayCommand(LoadFile);

                return _openFileCommand;
            }
        }

        public ICommand CloseFileCommand
        {
            get
            {
                if (_closeFileCommand == null)
                    _closeFileCommand = new RelayCommand(p => ChangeViewState(new InitialViewModel()), p => CurrentViewState is FileViewViewModel);

                return _closeFileCommand;
            }
        }

        public ICommand SaveFileCommand
        {
            get
            {
                if (_saveFileCommand == null)
                    _saveFileCommand = new RelayCommand(p => {}, p => false);

                return _saveFileCommand;
            }
        }

        #endregion
    }
}
