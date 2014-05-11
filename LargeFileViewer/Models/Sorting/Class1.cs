using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithms.ConsoleRunner
{
    class Program
    {
        //private const int SequenceCount = 34;
        //private const int SequenceLength = 50000;

        //private static IEnumerable<int>[] _sampleData;

        //private static IEnumerable<int>[] GetSampleSequences()
        //{
        //    return Enumerable.Range(1, SequenceCount)
        //        .Select(x => Enumerable.Range(1, SequenceLength)).ToArray();
        //    //.Select(x => GenerateRandomSequence().OrderBy(e => e).AsEnumerable()).ToArray();
        //}

        //private static IEnumerable<int>[] GetShortSortedSequences()
        //{
        //    return Enumerable.Range(1, 34)
        //        .Select(x => GenerateRandomSequence(100).OrderBy(e => e).AsEnumerable()).ToArray();
        //}

        //private static Random _random = new Random(42);
        //private static IEnumerable<int> GenerateRandomSequence(int upperBound)
        //{
        //    for (int i = 0; i < upperBound; i++)
        //        yield return _random.Next();
        //}
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Starting application...");
        //    var count = FSharpVersion.Merge.imperativeMergeSequences<int>(GetShortSortedSequences()).Count();
        //    var count2 = CSharpVersion.NaiveMerge.MergeSort(GetShortSortedSequences().ToList()).Count();

        //    Console.WriteLine("Starting performance test...");
        //    var sw = Stopwatch.StartNew();

        //    var countFSharp = FSharpVersion.Merge.imperativeMergeSequences<int>(GetSampleSequences()).Count();

        //    sw.Stop();
        //    Console.WriteLine("F# Merge finished: {0}ms, merged {1}", sw.ElapsedMilliseconds, countFSharp);

        //    Console.WriteLine("Starting performance test...");
        //    var sw2 = Stopwatch.StartNew();

        //    var countCSharp = CSharpVersion.NaiveMerge.MergeSort(GetSampleSequences().ToList()).Count();

        //    sw2.Stop();
        //    Console.WriteLine("Merge finished: {0}ms, merged {1}", sw2.ElapsedMilliseconds, countCSharp);

        //    Console.ReadLine();

        //}
    }
}