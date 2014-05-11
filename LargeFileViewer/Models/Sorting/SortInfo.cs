using System;

namespace LargeFileViewer.Models.Sorting
{
    class SortInfo
    {
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
            return new SortInfo(rowIndex, ConvertValue(columnValue, columnValueType), columnValueType.Name);
        }

        private static IComparable ConvertValue(object value, Type type)
        {
            return (IComparable)Convert.ChangeType(value, type);
        }
    }
}
