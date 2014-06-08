using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Sorting
{
    internal static class ArrayExtensions
    {
        public static TSource[] SubArray<TSource>(this TSource[] source, int startIndex, int length)
        {
            if(source.Length <= (startIndex + length))
                throw new ArgumentOutOfRangeException("startIndex", @"Sum of startIndex and length is greater than length of source array");

            var subArray = new TSource[length];

            for (int i = 0; i < length; i++)
            {
                subArray[i] = source[i + startIndex];
            }

            return subArray;
        }
    }
}
