using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Virtualization;

namespace LargeFileViewer.ViewModel.CollectionBinding
{
    class ColumnDescriptor : PropertyDescriptor
    {
        private readonly FileColumn _column;

        public ColumnDescriptor(FileColumn column) 
            : base(column.Name, new Attribute[0])
        {
            _column = column;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _column.Value;
        }

        public override void ResetValue(object component)
        {
            
        }

        public override void SetValue(object component, object value)
        {
            
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return typeof(FileRow); }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return _column.Type; }
        }

        public override string Name
        {
            get { return _column.Name; }
        }
    }
}
