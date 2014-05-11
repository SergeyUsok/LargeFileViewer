using System.Collections.Generic;
using System.Linq;

namespace LargeFileViewer.ViewModel
{
    class SeparatorSelectorViewModel : ViewModelBase
    {
        private readonly List<RadioButtonViewModel> _separators;
        private string _inputedSeparator;

        public SeparatorSelectorViewModel(string maybeSeparator)
        {
            _separators = GetSeparators(maybeSeparator);
        }

        public List<RadioButtonViewModel> Separators
        {
            get { return _separators; }
        }

        public string InputedSeparator
        {
            get { return _inputedSeparator; }
            set
            {
                _inputedSeparator = value;
                 OnPropertyChanged();
            }
        }

        public string ResultingSeparator 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(InputedSeparator))
                {
                    var rbvm = Separators.FirstOrDefault(rb => rb.IsSelected);

                    return rbvm != null ? rbvm.OriginalChar : string.Empty;
                }

                return InputedSeparator;
            }
        }

        private List<RadioButtonViewModel> GetSeparators(string maybeSeparator)
        {
            return new List<RadioButtonViewModel>
                {
                    new RadioButtonViewModel("Tab", "\t", maybeSeparator.Equals("\t")),
                    new RadioButtonViewModel("Comma", ",", maybeSeparator.Equals(",")),
                    new RadioButtonViewModel("Semicolon", ";", maybeSeparator.Equals(";")),
                    new RadioButtonViewModel("Underscore", "_", maybeSeparator.Equals("_"))
                };
        }
    }
}
