using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public abstract class DataReader : IDataReader
    {
        #region events

        public event EventHandler<ProgressEventArgs> Reading;

        #endregion

        #region fixed properties

        private ColumnHeaderCollection _columnHeaders = new ColumnHeaderCollection();
        /// <summary>
        /// Collection of header
        /// </summary>
        public ColumnHeaderCollection ColumnHeaders
        {
            get { return _columnHeaders; }
        }

        #endregion

        #region runtimechanging properties

        object[] _rowObjectArray;
        ///// <summary>
        ///// Get the current line object array
        ///// </summary>
        //public object[] CurrentRawObjectArray
        //{
        //    get { return _rowObjectArray; }
        //}

        /// <summary>
        /// Get the current item index in the collection.
        /// </summary>
        public long CurrentItem
        {
            get { return GetCurrentItems(); }
        }

        /// <summary>
        /// Get the total items of the collection.
        /// </summary>
        public long TotalItems
        {
            get
            {
                return GetTotalItems();
            }
        }

        #endregion
        
        #region Ctor

        public DataReader()
        {

        }

        #endregion

        public void SetObjectArray(object[] value)
        {
            _rowObjectArray = value;
        }

        #region IDataReader

        void IDataReader.Close()
        {
            Array.Clear(_rowObjectArray, 0, _rowObjectArray.Length);
        }

        int IDataReader.Depth
        {
            get { return 0; }
        }

        DataTable IDataReader.GetSchemaTable()
        {
            DataTable SchemaTable = new DataTable("SchemaTable");
            foreach (ColumnHeader clh in _columnHeaders)
                SchemaTable.Columns.Add(clh.ColumnName, clh.ColumnDataType);
            for (int i = 0; i < SchemaTable.Columns.Count; i++)
            {
                SchemaTable.Columns[i].AllowDBNull = true;
                SchemaTable.Columns[i].AutoIncrement = false;
                SchemaTable.Columns[i].AutoIncrementSeed = 1;
                SchemaTable.Columns[i].AutoIncrementStep = 1;
                SchemaTable.Columns[i].Caption = SchemaTable.Columns[i].ColumnName;
                SchemaTable.Columns[i].ReadOnly = true;
                SchemaTable.Columns[i].Unique = false;
            }
            return SchemaTable;
        }

        bool IDataReader.IsClosed
        {
            get { return GetIsClosed(); }
        }

        bool IDataReader.NextResult()
        {
            return false;
        }

        bool IDataReader.Read()
        {
            return Read();
        }

        int IDataReader.RecordsAffected
        {
            get { return -1; }
        }

        void IDisposable.Dispose()
        {
            Array.Clear(_rowObjectArray, 0, _rowObjectArray.Length);
        }

        int IDataRecord.FieldCount
        {
            get { return _columnHeaders.Count; }
        }

        bool IDataRecord.GetBoolean(int i)
        {

            bool rtn = default(bool);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is bool)
                rtn = (bool)_rowObjectArray[i];
            else
                bool.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        byte IDataRecord.GetByte(int i)
        {

            byte rtn = default(byte);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is byte)
                rtn = (byte)_rowObjectArray[i];
            else
                byte.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {

            throw new NotImplementedException();
        }

        char IDataRecord.GetChar(int i)
        {

            char rtn = default(char);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is char)
                rtn = (char)_rowObjectArray[i];
            else
                char.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {

            throw new NotImplementedException();
        }

        IDataReader IDataRecord.GetData(int i)
        {
            if (i == 0)
                return this;
            return null;
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            return _columnHeaders[i].ColumnDataType.FullName;
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            DateTime rtn = default(DateTime);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is DateTime)
                rtn = (DateTime)_rowObjectArray[i];
            else
                DateTime.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            decimal rtn = default(decimal);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is decimal)
                rtn = (decimal)_rowObjectArray[i];
            else
                decimal.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        double IDataRecord.GetDouble(int i)
        {
            double rtn = default(double);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is double)
                rtn = (double)_rowObjectArray[i];
            else
                double.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        Type IDataRecord.GetFieldType(int i)
        {
            return _columnHeaders[i].ColumnDataType;
        }

        float IDataRecord.GetFloat(int i)
        {
            float rtn = default(float);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is float)
                rtn = (float)_rowObjectArray[i];
            else
                float.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        Guid IDataRecord.GetGuid(int i)
        {
            Guid rtn = Guid.Empty;
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is Guid)
                rtn = (Guid)_rowObjectArray[i];
            else
            {
                try
                {
                    rtn = new Guid(_rowObjectArray[i].ToString());
                }
                catch (Exception)
                {
                    rtn = Guid.Empty;
                }
            }
            return rtn;
        }

        short IDataRecord.GetInt16(int i)
        {
            short rtn = default(short);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is short)
                rtn = (short)_rowObjectArray[i];
            else
                short.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        int IDataRecord.GetInt32(int i)
        {
            int rtn = default(int);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is int)
                rtn = (int)_rowObjectArray[i];
            else
                int.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        long IDataRecord.GetInt64(int i)
        {
            long rtn = default(long);
            if (_rowObjectArray[i] == null)
                return rtn;
            if (_rowObjectArray[i] is long)
                rtn = (long)_rowObjectArray[i];
            else
                long.TryParse(_rowObjectArray[i].ToString(), out rtn);
            return rtn;
        }

        string IDataRecord.GetName(int i)
        {
            return _columnHeaders[i].ColumnName;
        }

        int IDataRecord.GetOrdinal(string name)
        {
            return _columnHeaders.IndexOf(name);
        }

        string IDataRecord.GetString(int i)
        {
            return _rowObjectArray[i] == null ? null : _rowObjectArray[i].ToString();
        }

        object IDataRecord.GetValue(int i)
        {
            try
            {
                return _rowObjectArray[i];

            }
            catch (Exception)
            {

                throw;
            }
        }

        int IDataRecord.GetValues(object[] values)
        {
            try
            {
                Array.Copy(_rowObjectArray, values, _rowObjectArray.Length);
                return _rowObjectArray.Length;

            }
            catch (Exception)
            {

                throw;
            }
        }

        bool IDataRecord.IsDBNull(int i)
        {
            return _rowObjectArray[i] == null;
        }

        object IDataRecord.this[string name]
        {
            get
            {
                int i = _columnHeaders.IndexOf(name);

                if (i > -1 && i < _columnHeaders.Count)
                    return _rowObjectArray[i];
                return null;
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return _rowObjectArray[i];
            }
        }

        #endregion
        
        public virtual void OnReading(long currentitem, long totalitems)
        {
            try
            {
                EventHandler<ProgressEventArgs> handler = Reading;
                if (handler != null)
                    handler(this, new ProgressEventArgs(currentitem, totalitems));
            }
            catch { throw; }

        }

        public abstract long GetTotalItems();
        public abstract long GetCurrentItems();
        public abstract bool GetIsClosed();
        public abstract bool Read();
    }
}
