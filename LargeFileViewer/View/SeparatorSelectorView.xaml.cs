using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LargeFileViewer.View
{
    /// <summary>
    /// Interaction logic for SeparatorSelectorView.xaml
    /// </summary>
    public partial class SeparatorSelectorView : Window
    {
        public SeparatorSelectorView()
        {
            InitializeComponent();
        }

        private void OnClickOk(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
