using System.Collections.Generic;

namespace LargeFileViewer.Models.Sorting.ExternalSort
{
    internal class PartialResult<TSource>
    {
        public IEnumerable<TSource> Part { get; private set; }

        public PartialResult(IEnumerable<TSource> part)
        {
            Part = part;
        }
    }
}