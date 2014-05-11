using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using LargeFileViewer.Annotations;
using LargeFileViewer.Models.Sorting;
using LargeFileViewer.ViewModel.CollectionBinding;

namespace LargeFileViewer.Models.Virtualization
{
    class VirtualizingCollection : IList<FileRow>, ITypedList, IBindingListView
    {
        private IItemsProvider<FileRow> _itemsProvider;
        private readonly Sorter _sorter;

        public VirtualizingCollection([NotNull] IItemsProvider<FileRow> itemsProvider)
        {
            if (itemsProvider == null) 
                throw new ArgumentNullException("itemsProvider");

            _itemsProvider = itemsProvider;
            _sorter = new Sorter(_itemsProvider);
        }

        public IEnumerator<FileRow> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((FileRow) value);
        }

        public int Count
        {
            get { return _itemsProvider.FetchCount(); }
        }

        public int IndexOf(FileRow item)
        {
            return -1;
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        public FileRow this[int index]
        {
            get { return _itemsProvider.FetchSingle(index); }
            set { throw new NotSupportedException(); }
        }

        #region ITypedLIst members

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            var item = _itemsProvider.FetchSingle(0);
            var cast = item as ICustomTypeDescriptor;
            return cast.GetProperties();
            //if (cast != null)
            //{
            //    return cast.GetProperties();
            //}

            //return TypeDescriptor.GetProperties(typeof (T));
        }

        #endregion

        #region Sorting

        public bool SupportsSorting
        {
            get { return true; }
        }

        public bool IsSorted
        {
            get { throw new NotImplementedException(); }
        }

        public PropertyDescriptor SortProperty
        {
            get { throw new NotImplementedException(); }
        }

        public ListSortDirection SortDirection
        {
            get { throw new NotImplementedException(); }
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            var a = sorts.OfType<ListSortDescription>().First();

            _sorter.Sort(a.PropertyDescriptor.Name, a.SortDirection)
                .ContinueWith(t =>
                    {
                        _itemsProvider = t.Result;
                        OnCollectionChanged(a.PropertyDescriptor);
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {

        }

        public ListSortDescriptionCollection SortDescriptions
        {
            get { throw new NotImplementedException(); }
        }

        public bool SupportsAdvancedSorting
        {
            get { return true; }
        }

        public void RemoveSort()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Searching and Filtering

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public bool SupportsSearching
        {
            get { return true; }
        }

        public void RemoveFilter()
        {
            throw new NotImplementedException();
        }

        public string Filter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool SupportsFiltering
        {
            get { return true; }
        }

        #endregion

        #region Notification

        public bool SupportsChangeNotification
        {
            get { return true; }
        }

        public event ListChangedEventHandler ListChanged;

        private void OnCollectionChanged(PropertyDescriptor descriptor)
        {
            var handler = ListChanged;

            if(handler != null)
                handler(this, new ListChangedEventArgs(ListChangedType.Reset, descriptor));
        }

        #endregion

        #region Not supported IList, IList<FileRow> and IBindingListView members

        public object SyncRoot
        {
            get { return this; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public void Add(FileRow item)
        {
            throw new NotSupportedException();
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public bool Contains(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, FileRow item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public bool Contains(FileRow item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(FileRow[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(FileRow item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(Array array, int index)
        {
            
        }

        public bool AllowNew
        {
            get { return false; }
        }

        public bool AllowEdit
        {
            get { return false; }
        }

        public bool AllowRemove
        {
            get { return false; }
        }

        public object AddNew()
        {
            throw new NotSupportedException();
        }

        public void AddIndex(PropertyDescriptor property)
        {

        }

        public void RemoveIndex(PropertyDescriptor property)
        {

        }

        #endregion
    }
}
