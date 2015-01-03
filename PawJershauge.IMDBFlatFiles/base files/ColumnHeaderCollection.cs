using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public sealed class ColumnHeaderCollection : IList<ColumnHeader>
    {
        private List<ColumnHeader> _columnHeaders = new List<ColumnHeader>();

        public void AddRange(IEnumerable<ColumnHeader> columnHeaders)
        {
            _columnHeaders.AddRange(columnHeaders);
        }
        public void Add(ColumnHeader columnHeader)
        {
            _columnHeaders.Add(columnHeader);
        }
        public void Add(string columnName)
        {
            Add(new ColumnHeader(columnName));
        }
        public void Add(string columnName, Type columnDataType)
        {
            Add(new ColumnHeader(columnName, columnDataType));
        }

        public int IndexOf(ColumnHeader item)
        {
            return _columnHeaders.IndexOf(item);
        }
        public int IndexOf(string columnName)
        {
            return _columnHeaders.FindIndex(c => c.ColumnName == columnName);
        }

        public void Insert(int index, ColumnHeader item)
        {
            _columnHeaders.Insert(index, item);
        }
        public void Insert(string columnName, ColumnHeader item)
        {
            _columnHeaders.Insert(_columnHeaders.FindIndex(c => c.ColumnName == columnName), item);
        }

        public void RemoveByColumnName(string columnName)
        {
            _columnHeaders.RemoveAt(_columnHeaders.FindIndex(c => c.ColumnName == columnName));
        }
        public void RemoveAt(int index)
        {
            _columnHeaders.RemoveAt(index);
        }

        public ColumnHeader this[int index]
        {
            get
            {
                return _columnHeaders[index];
            }
            set
            {
                _columnHeaders[index] = value;
            }
        }
        public ColumnHeader this[string columnName]
        {
            get
            {
                return _columnHeaders[_columnHeaders.FindIndex(c => c.ColumnName == columnName)];
            }
            set
            {
                _columnHeaders[_columnHeaders.FindIndex(c => c.ColumnName == columnName)] = value;
            }
        }

        public void Clear()
        {
            _columnHeaders.Clear();
        }

        public bool Contains(ColumnHeader item)
        {
            return _columnHeaders.Contains(item);
        }
        public bool Contains(string columnName)
        {
            return _columnHeaders.Exists(c => c.ColumnName == columnName);
        }

        public void CopyTo(ColumnHeader[] array, int arrayIndex)
        {
            _columnHeaders.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _columnHeaders.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ColumnHeader item)
        {
            return _columnHeaders.Remove(item);
        }

        public IEnumerator<ColumnHeader> GetEnumerator()
        {
            return _columnHeaders.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _columnHeaders.GetEnumerator();
        }
    }
}
