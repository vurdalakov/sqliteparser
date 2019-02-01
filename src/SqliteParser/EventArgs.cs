namespace Vurdalakov.SqliteParser
{
    using System;

    public class FieldEventArgs : EventArgs
    {
        public SerialType Type { get; }
        public Object Value { get; }

        public FieldEventArgs(SerialType type, Object value)
        {
            this.Type = type;
            this.Value = value;
        }
    }

    public class CellEventArgs : EventArgs
    {
        public CellType CellType { get; }
        public UInt64 Rowid { get; }

        public CellEventArgs(CellType cellType, UInt64 rowid)
        {
            this.CellType = cellType;
            this.Rowid = rowid;
        }
    }

    public class PageEventArgs : EventArgs
    {
        public UInt64 PageNumber { get; }
        public SqlitePageHeader PageHeader { get; }

        public PageEventArgs(UInt64 pageNumber, SqlitePageHeader pageHeader)
        {
            this.PageNumber = pageNumber;
            this.PageHeader = pageHeader;
        }
    }
}
