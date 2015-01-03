using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawJershauge.IMDBFlatFiles
{
    public sealed class ColumnHeader
    {
        public string ColumnName { get; set; }
        public Type ColumnDataType { get; set; }

        public ColumnHeader(string columnName, Type columnDataType)
        {
            ColumnName = columnName;
            ColumnDataType = columnDataType;
        }
        public ColumnHeader(string columnName)
            : this(columnName, typeof(string))
        { }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", ColumnName, ColumnDataType.FullName);
        }
    }

}
