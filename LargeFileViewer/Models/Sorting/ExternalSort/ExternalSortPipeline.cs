using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting.ExternalSort
{
    internal static class ExternalSortPipeline
    {
        public static IEnumerable<IList<TSource>> Chunkify<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            var accumulator = new List<TSource>(chunkSize);

            foreach(var item in source)
            {
                accumulator.Add(item);

                if (accumulator.Count != chunkSize) 
                    continue;

                yield return accumulator;
                accumulator = new List<TSource>(chunkSize);
            }

            if (accumulator.Count > 0)
                yield return accumulator;
        }

        public static IEnumerable<PartialResult<TSource>> SortAscendingAsync<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        {
            return SortAscendingAsync(source, keySelector, CalculateDegreeOfParallelism());
        }

        public static IEnumerable<PartialResult<TSource>> SortAscendingAsync<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey>  keySelector, int degreeOfParallelism)
        {
            if (source == null) 
                throw new ArgumentNullException("source");

            if (keySelector == null) 
                throw new ArgumentNullException("keySelector");

            Func<IEnumerable<TSource>, PartialResult<TSource>> projector = part => new PartialResult<TSource>(part.OrderBy(keySelector).ForceEnumeration());

            return new AsyncEnumerator<IEnumerable<TSource>, PartialResult<TSource>>(source, projector, degreeOfParallelism);
        }

        public static IEnumerable<string> SavePartialResult<TSource>(this IEnumerable<PartialResult<TSource>> partialResults, Func<TSource, string> converter)
        {
            foreach (var partialResult in partialResults)
            {
                var fileToSave = Path.GetTempFileName();

                File.WriteAllLines(fileToSave, partialResult.Part.Select(pr => converter(pr)));

                yield return fileToSave;
            }
        }

        public static IEnumerable<TSource> MergeAscending<TSource, TKey>(this IEnumerable<IEnumerator<TSource>> source, Func<TSource, TKey> keySelector)
            where TKey : IComparable
        {
            var sourceEnumerators = source.Where(en => en.MoveNext()).OrderBy(en => keySelector(en.Current));

            var enumerators = new LinkedList<IEnumerator<TSource>>(sourceEnumerators);

            while (enumerators.Count > 0)
            {
                var minEnumerator = enumerators.First.Value;

                yield return minEnumerator.Current;

                if (!minEnumerator.MoveNext())
                {
                    enumerators.RemoveFirst();
                    minEnumerator.Dispose();
                }
                else
                {
                    if (enumerators.Count > 1) // no need remove and add action when only one enumerator remains
                    {
                        enumerators.RemoveFirst();
                        enumerators.AddInSortedOrder(minEnumerator, keySelector);
                    }
                }
            }
        }

        #region Helper methods

        private static IEnumerable<TSource> ForceEnumeration<TSource>(this IEnumerable<TSource> source)
        {
            return source.ToList();
        }

        private static void AddInSortedOrder<TSource, TKey>(this LinkedList<IEnumerator<TSource>> list, IEnumerator<TSource> enumerator, Func<TSource, TKey> keySelector)
            where TKey : IComparable
        {
            var currentKey = keySelector(enumerator.Current);

            if (keySelector(list.First.Value.Current).CompareTo(currentKey) >= 0)
            {
                list.AddFirst(enumerator);
                return;
            }

            if (keySelector(list.Last.Value.Current).CompareTo(currentKey) <= 0)
            {
                list.AddLast(enumerator);
                return;
            }

            var current = list.First.Next; // we have already checked first element

            while (current != null)
            {
                if (keySelector(current.Value.Current).CompareTo(currentKey) >= 0)
                {
                    list.AddBefore(current, enumerator);
                    return;
                }

                current = current.Next;
            }
        }

        private static int CalculateDegreeOfParallelism()
        {
            return Environment.ProcessorCount == 1 ? 2 : Environment.ProcessorCount;
        }

        #endregion
    }
}
