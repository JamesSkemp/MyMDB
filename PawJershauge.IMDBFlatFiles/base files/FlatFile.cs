using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public class FlatFile : IDataReader
    {
        #region events

        public event EventHandler<ProgressEventArgs> ReadingFile;

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

        private Encoding _encoding = Encoding.Default;

        private char _delimiter = '\t';
        /// <summary>
        /// Gets the delimiter character separating each field.
        /// </summary>
        /// <value>The delimiter character separating each field.</value>
        [DefaultValue('\t')]
        public char Delimiter
        {
            get
            {
                return _delimiter;
            }
        }

        private int _notifyAfter = 5120;
        /// <summary>
        /// Fire the ReadingFile Event after each n byte.
        /// Default = 5120 (5 Mb)
        /// </summary>
        [DefaultValue(5120)]
        public int NotifyAfter
        {
            get { return _notifyAfter; }
            set { _notifyAfter = value; }
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

        private long _lastStreamPosition = 0;
        private long _currentStreamPosition = 0;
        /// <summary>
        /// Get the current position of the stream.
        /// </summary>
        public long CurrentStreamPosition
        {
            get { return _currentStreamPosition; }
        }

        private long _totalStreamLength = -1;
        /// <summary>
        /// Get the total length of the stream.
        /// </summary>
        public long TotalStreamLength
        {
            get
            {
                return _totalStreamLength;
            }
        }

        public bool EndOfFile
        {
            get { return _streamreader.EndOfStream; }
        }

        #endregion

        StreamReader _streamreader = null;
        string _fileName;

        #region Ctor

        public FlatFile(string fileName, Encoding encoding)
        {
            _fileName = fileName;
            _encoding = encoding;
            _streamreader = new StreamReader(_fileName, _encoding);
            _totalStreamLength = _streamreader.BaseStream.Length;
        }
        public FlatFile(string fileName) : this(fileName, Encoding.Default) { }

        #endregion

        public void SetObjectArray(object[] value)
        {
            _rowObjectArray = value;
        }
        public virtual bool Read()
        {
            return false;
        }
        public string ReadLine()
        {
            string rl = _streamreader.ReadLine();
            HandleBaseStream();
            return rl;
        }

        //private bool ReadForIDataReader()
        //{
        //    bool read = true;
        //    bool readOn = false;
        //    do
        //    {
        //        try
        //        {
        //            readOn = false;
        //            if (ReadAndCountRow()) // Read the line...
        //            {
        //                CancelEventArgs cea = new CancelEventArgs();
        //                OnSkipRawRow(cea);
        //                if (cea.Cancel) // user indicated that we need to read on... skip converters and read the next line.
        //                {
        //                    readOn = true; //Flag the loop to read next line
        //                    WhyDidYouSkipLine("User skipped the line", 100);
        //                }
        //                else
        //                {
        //                    if (_currentRow == null) // Nothing more to read, we reached the end of the stream.
        //                        read = false;
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(_currentRow) && _currentRow.Trim()[0] == _comment) //if the line is a comment line then...
        //                        {
        //                            OnCommentRow(_currentRow); // Fire the comment line.
        //                            readOn = true; //Flag the loop to read next line.
        //                            WhyDidYouSkipLine("Line is comment line", 10);
        //                        }
        //                        else
        //                        {
        //                            if (_columnHeaders.Count == 0) // if no columnheaders has been set, then...
        //                            {
        //                                if (_firstRowIsHeaderNames) // if first row contains columnheaders...
        //                                {
        //                                    _RowArray = OnSplitRow(_currentRow);
        //                                    _columnHeaders.AddRange(OnConvertColumnHeader(_RowArray));
        //                                    readOn = true; //Flag the loop to read next line.
        //                                    WhyDidYouSkipLine("Header line (Converted)", 11);
        //                                }
        //                                else // if the file has no info about the columns, build fictional column names...
        //                                {
        //                                    _columnHeaders.AddRange(
        //                                        OnConvertColumnHeader(
        //                                        Enumerable.Range(0, _RowArray.Length).ToList().ConvertAll(i => string.Format("Column_{0}", i)).ToArray()
        //                                        ));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (_firstRowIsHeaderNames && _currentRowIndex == 1) //Skip Column header line...
        //                                {
        //                                    readOn = true;
        //                                    WhyDidYouSkipLine("Header line", 12);
        //                                }
        //                            }
        //                            if (!readOn) //If the line should not be skipped, then...
        //                            {
        //                                if (string.IsNullOrEmpty(_currentRow)) //Skip the line...
        //                                {
        //                                    readOn = true;
        //                                    WhyDidYouSkipLine("Empty line", 1);
        //                                }
        //                                else // Handle the line...
        //                                {
        //                                    _RowArray = OnSplitRow(_currentRow);
        //                                    _rowObjectArray = OnConvertColumn(_RowArray);
        //                                    CancelEventArgs convcea = new CancelEventArgs();
        //                                    OnSkipConvertedRow(convcea);
        //                                    readOn = convcea.Cancel;
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                read = false;
        //                OnReadingRow(_currentRowIndex);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    } while (readOn);
        //    if (!read) // theres nothing more to read, clear buffers
        //    {
        //        Array.Clear(_RowArray, 0, _RowArray.Length);
        //        Array.Clear(_rowObjectArray, 0, _rowObjectArray.Length);
        //        _currentRow = string.Empty;
        //        _currentRowIndex = 0;
        //    }
        //    return read;
        //}
        private void HandleBaseStream()
        {
            _currentStreamPosition = _streamreader.BaseStream.Position;
            if ((_currentStreamPosition - _lastStreamPosition) > _notifyAfter)
            {
                _lastStreamPosition = _currentStreamPosition;
                OnReadingFile(_currentStreamPosition);
            }
            if (_streamreader.EndOfStream)
                OnReadingFile(_streamreader.BaseStream.Length);
        }

        #region IDataReader

        void IDataReader.Close()
        {
            Array.Clear(_rowObjectArray, 0, _rowObjectArray.Length);
            if (_streamreader != null)
                _streamreader.Close();
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
            get { return _streamreader == null ? true : _streamreader.EndOfStream; }
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
            _streamreader.Dispose();
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
        
        public virtual void OnReadingFile(long currentpos)
        {
            try
            {
                EventHandler<ProgressEventArgs> handler = ReadingFile;
                if (handler != null)
                    handler(this, new ProgressEventArgs(currentpos, _totalStreamLength));
            }
            catch { throw; }

        }
    }
}
