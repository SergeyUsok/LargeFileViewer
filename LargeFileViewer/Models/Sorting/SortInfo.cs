using System;
using System.Linq;

namespace LargeFileViewer.Models.Sorting
{
    class SortInfo
    {
        private const char PropertySeparator = '\b';
        private const char NameValueSeparator = '\f';

        private SortInfo(int rowIndex, IComparable columnValue, string columnValueType)
        {
            RowIndex = rowIndex;
            ColumnValue = columnValue;
            ColumnValueType = columnValueType;
        }

        public int RowIndex { get; private set; }

        public IComparable ColumnValue { get; private set; }

        public string ColumnValueType { get; private set; }

        public static SortInfo Create(int rowIndex, object columnValue, Type columnValueType)
        {
            return new SortInfo(rowIndex, ConvertValue(columnValue, columnValueType), columnValueType.FullName);
        }

        private static IComparable ConvertValue(object value, Type type)
        {
            return (IComparable)Convert.ChangeType(value, type);
        }

        public override string ToString()
        {
            return typeof(SortInfo).GetProperties()
                                   .Select(p => string.Format("{0}{1}{2}", p.Name, NameValueSeparator, p.GetValue(this)))
                                   .Aggregate((total, next) => string.Format("{0}{1}{2}", total, PropertySeparator, next));
        }

        public static SortInfo FromString(string sortInfoString)
        {
            var properties = sortInfoString.Split(PropertySeparator)
                                               .Select(p => p.Split(NameValueSeparator))
                                               .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1]);

            var columnType = GetColumnType(properties["ColumnValueType"]);
            var index = Convert.ToInt32(properties["RowIndex"]);

            return Create(index, properties["ColumnValue"], columnType);
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
    }
}
