using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileViewer.Models.Virtualization
{
    class FileColumn
    {
        private const char PropertySeparator = '\b';
        private const char NameValueSeparator = '\f';

        private FileColumn()
        {
        }

        public string Name { get; private set; }

        public IComparable Value { get; private set; }

        public Type Type { get; private set; }

        public int ParentRowIndex { get; private set; }

        public static FileColumn Create(int index, string name, string value, Type type)
        {
            return new FileColumn
                {
                    Name = name,
                    Value = ConvertValue(value, type),
                    Type = type,
                    ParentRowIndex = index
                };
        }

        public override string ToString()
        {
            return typeof(FileColumn).GetProperties()
                                   .Select(p => string.Format("{0}{1}{2}", p.Name, NameValueSeparator, p.GetValue(this)))
                                   .Aggregate((total, next) => string.Format("{0}{1}{2}", total, PropertySeparator, next));
        }

        public static FileColumn FromString(string sortInfoString)
        {
            var properties = sortInfoString.Split(PropertySeparator)
                                               .Select(p => p.Split(NameValueSeparator))
                                               .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1]);

            var columnType = GetColumnType(properties["Type"]);
            var index = Convert.ToInt32(properties["ParentRowIndex"]);

            return Create(index, properties["Name"], properties["Value"], columnType);
        }

        private static Type GetColumnType(string type)
        {
            switch (type)
            {
                case "System.Int32":
                    return typeof(int);
                case "System.Single":
                    return typeof(double);
                case "System.Boolean":
                    return typeof(bool);
                default:
                    return typeof(string);
            }
        }

        private static IComparable ConvertValue(string value, Type type)
        {
            return (IComparable) Convert.ChangeType(value, type);
        }
    }
}
