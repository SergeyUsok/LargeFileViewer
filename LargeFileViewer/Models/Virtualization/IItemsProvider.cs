using System.Collections.Generic;

namespace LargeFileViewer.Models.Virtualization
{
    internal interface IItemsProvider<T>
    {
        int FetchCount();

        IEnumerable<T> FetchRange(int start, int count);

        T FetchSingle(int itemNumber);
    }
}