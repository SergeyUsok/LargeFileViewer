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
    class FileRow : ICustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection _properties;
        private readonly Dictionary<string, FileColumn> _columns;

        public FileRow([NotNull] IEnumerable<FileColumn> columns)
        {
            if (columns == null) 
                throw new ArgumentNullException("columns");

            _columns = columns.ToDictionary(c => c.Name, c => c);
            _properties = GetPropertiesCollection(columns);
        }

        public AttributeCollection GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        public string GetClassName()
        {
            return "FileRow";
        }

        public string GetComponentName()
        {
            return string.Empty;
        }

        public TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return _properties;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return PropertyDescriptorCollection.Empty;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        public FileColumn this[string columnName]
        {
            get { return _columns[columnName]; }
        }

        private PropertyDescriptorCollection GetPropertiesCollection(IEnumerable<FileColumn> columns)
        {
            var descriptors = columns.Select(c => new ColumnDescriptor(c)).ToArray();

            return new PropertyDescriptorCollection(descriptors);
        }
    }
}
