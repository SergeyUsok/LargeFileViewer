using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Virtualization;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Sorting
{
    class ChunkCreator
    {
        private readonly int _chunkSize;
        private readonly IItemsProvider<FileRow> _provider;

        public ChunkCreator(int chunkSize, IItemsProvider<FileRow> provider)
        {
            _chunkSize = chunkSize;
            _provider = provider;
        }

        public IEnumerable<IEnumerable<SortInfo>> Chunkify(string column)
        {
            var accumulator = new List<SortInfo>(_chunkSize);

            for (int i = 0; i < _provider.FetchCount(); i++)
            {
                var fileColumn = _provider.FetchSingle(i)[column];
                accumulator.Add(SortInfo.Create(i, fileColumn.Value, fileColumn.Type));

                if (accumulator.Count == _chunkSize)
                {
                    yield return accumulator;
                    accumulator = new List<SortInfo>(_chunkSize);
                }
            }

            if(accumulator.Count > 0)
                yield return accumulator;
        }
    }
}
