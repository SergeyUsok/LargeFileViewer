using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting
{
    class MergingSorter
    {
        public IEnumerable<int> MergeAscendingSort(List<Task<string>> partialResults)
        {
            var enumerators = partialResults.Select(t => new SortInfoEnumerator(t.Result))
                                   .Where(e => e.MoveNext())
                                   .ToList();

            while (enumerators.Count > 0)
            {
                var minEnumerator = GetMinEnumerator(enumerators);

                yield return minEnumerator.Current.RowIndex;

                if (!minEnumerator.MoveNext())
                {
                    minEnumerator.Dispose();
                    enumerators.Remove(minEnumerator);
                }
            }
        }

        private SortInfoEnumerator GetMinEnumerator(IEnumerable<SortInfoEnumerator> enumerators)
        {
            var minEnumerator = enumerators.First();

            foreach (var enumerator in enumerators.Skip(1))
            {
                if (enumerator.Current.ColumnValue.CompareTo(minEnumerator.Current.ColumnValue) == -1)
                    minEnumerator = enumerator;
            }

            return minEnumerator;
        }
    }
}
