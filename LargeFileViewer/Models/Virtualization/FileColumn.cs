using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Virtualization
{
    class FileColumn
    {
        private FileColumn()
        {
        }

        public string Name { get; private set; }

        public object Value { get; private set; }

        public Type Type { get; private set; }

        public static FileColumn Create(string name, object value, Type type)
        {
            return new FileColumn
                {
                    Name = name,
                    Value = value,
                    Type = type
                };
        }
    }
}
