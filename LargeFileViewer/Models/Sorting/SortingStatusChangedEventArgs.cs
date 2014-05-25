using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting
{
    class SortingStatusChangedEventArgs : EventArgs
    {
        public bool IsCurrentlySorting { get; private set; }

        public SortingStatusChangedEventArgs(bool isCurrentlySorting)
        {
            IsCurrentlySorting = isCurrentlySorting;
        }
    }
}
