using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using LargeFileViewer.ViewModel.ViewStates;
using Microsoft.Win32;

namespace LargeFileViewer.ViewModel
{
    partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            // use a timer to periodically update the memory usage
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
            timer.Start();
            CurrentViewState = new InitialViewModel();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            MemoryUsage = string.Format("Memory Usage: {0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }

        private void LoadFile(object fileObject)
        {
            var fileName = fileObject as string ?? LoadFromDialog();

            if (fileName != null)
            {
                StartLoading(fileName);
            }
        }

        private string LoadFromDialog()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Data files (.txt)|*.txt|(.csv)|*.csv|(.tsv)|*.tsv|All files|*.*",
                Multiselect = false
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void StartLoading(string fileName)
        {
            var loadingViewModel = new LoadingViewModel(fileName);

            if (loadingViewModel.DetectColumnsSeparator())
            {
                ChangeViewState(loadingViewModel);
                
                loadingViewModel.LoadFile()
                            .ContinueWith(t => ChangeViewState(new FileViewViewModel(t.Result)));
            }
        }

        private void ChangeViewState(ViewStateViewModel viewModel)
        {
            var oldViewState = CurrentViewState as IDisposable;

            CurrentViewState = viewModel;

            DisposeIfNecessary(oldViewState);
        }

        private void DisposeIfNecessary(IDisposable disposable)
        {
            if(disposable != null)
                disposable.Dispose();
        }
    }
}
