using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.ViewModel
{
    class RadioButtonViewModel : ViewModelBase
    {
        public RadioButtonViewModel(string name, string originalChar, bool isSelected)
        {
            Name = name;
            OriginalChar = originalChar;
            IsSelected = isSelected;
        }

        public string Name { get; private set; }

        public string OriginalChar { get; private set; }

        public bool IsSelected { get; private set; }
    }
}
