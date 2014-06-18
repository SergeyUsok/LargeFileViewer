using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Detectors
{
    internal static class ColumnsSeparatorDetector
    {
        public static string DetectColumnSeparator(IEnumerable<string> rows)
        {
            return rows.Select(line => line.ToCharArray()
                                                 .Where(c => !char.IsLetterOrDigit(c))
                                                 .GroupBy(c => c)
                                                 .Select(c => new { Key = c.Key, Count = c.Count() })
                                                 .Where(gr => gr.Count > 1)
                                                 .ToDictionary(gr => gr.Key, gr => gr.Count)
                                    )
                             .Aggregate((total, next) =>
                             {
                                 var dic = new Dictionary<char, int>();

                                 foreach (var charCount in total)
                                 {
                                     if (next.ContainsKey(charCount.Key) && charCount.Value == next[charCount.Key])
                                         dic.Add(charCount.Key, charCount.Value);
                                 }

                                 return dic;
                             })
                             .Keys
                             .FirstOrDefault()
                             .ToString();
        }
    }
}
