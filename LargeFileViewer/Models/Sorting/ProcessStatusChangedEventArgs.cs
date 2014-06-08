using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting
{
    class ProcessStatusChangedEventArgs : EventArgs
    {
        public bool IsCurrentlyProcessing { get; private set; }

        public ProcessStatusChangedEventArgs(bool isCurrentlyProcessing)
        {
            IsCurrentlyProcessing = isCurrentlyProcessing;
        }
    }
}
