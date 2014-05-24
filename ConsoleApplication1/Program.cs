using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = File.ReadAllLines(@"D:\csv file.txt");

            using (var writer = new StreamWriter(@"D:\csv file.txt"))
            {
                foreach (var s in file)
                {
                    writer.WriteLine(s);
                }

                var r = 1;

                for (int i = 0; i < 10000000; i++)
                {
                    writer.WriteLine(file[r]);
                    r++;

                    if (r == 5)
                        r = 1;
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
