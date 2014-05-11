using System;
using System.Linq;

namespace LargeFileViewer.Models.Sorting
{
    class SortInfoSerializer
    {
        private const char PropertySeparator = '\b';
        private const char NameValueSeparator = '\f';

        public string Serialize(SortInfo sortInfo)
        {
            return typeof(SortInfo).GetProperties()
                                   .Select(p => string.Format("{0}{1}{2}", p.Name, NameValueSeparator, p.GetValue(sortInfo)))
                                   .Aggregate((total, next) => string.Format("{0}{1}{2}", total, PropertySeparator, next));
        }

        public SortInfo Deserialize(string serializedSortInfo)
        {
            var properties = serializedSortInfo.Split(PropertySeparator)
                                               .Select(p => p.Split(NameValueSeparator))
                                               .ToDictionary(keyValue => keyValue[0], keyValue => keyValue[1]);

            var columnType = GetColumnType(properties["ColumnValueType"]);
            var index = Convert.ToInt32(properties["RowIndex"]);

            return SortInfo.Create(index, properties["ColumnValue"], columnType);
        }

        private Type GetColumnType(string type)
        {
            switch (type)
            {
                case "System.Int32":
                    return typeof (int);
                case "System.Single":
                    return typeof (double);
                case "System.Boolean":
                    return typeof (bool);
                default:
                    return typeof (string);
            }
        }
    }
}
