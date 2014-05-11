using System.Linq;
using System.Windows;
using LargeFileViewer.ViewModel;

namespace LargeFileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop)).First();

                var viewModel = (MainViewModel)this.DataContext;

                viewModel.OpenFileCommand.Execute(file);
            }
        }
    }
}
