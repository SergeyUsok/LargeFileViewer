using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Models.Virtualization;

namespace LargeFileViewer.Models.Sorting
{
    internal static class AuxiliaryExtensions
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

        public static IEnumerable<FileColumn> Deserialize(this FileInfo file)
        {
            using (var reader = new StreamReader(file.FullName))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return FileColumn.FromString(line);
                }
            }

            if (File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }
        }
    }
}
