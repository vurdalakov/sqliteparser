namespace Vurdalakov.SqliteParser
{
    using System;

    public class FieldEventArgs : EventArgs
    {
        public UInt64 FieldNumber { get; }
        public SerialType Type { get; }
        public Object Value { get; }

        internal FieldEventArgs(UInt64 fieldNumber, SerialType type, Object value)
        {
            this.FieldNumber = fieldNumber;
            this.Type = type;
            this.Value = value;
        }
    }

    public class PayloadEventArgs : EventArgs
    {
        public UInt64 CellNumber { get; }
        public SqliteCellHeader CellHeader { get; }
        public SqliteField[] Fields { get; }

        internal PayloadEventArgs(UInt64 cellNumber, SqliteCellHeader cellHeader, SqliteField[] fields)
        {
            this.CellNumber = cellNumber;
            this.CellHeader = cellHeader;
            this.Fields = fields;
        }
    }

    public class CellEventArgs : EventArgs
    {
        public UInt64 CellNumber { get; }
        public SqliteCellHeader CellHeader { get; }

        internal CellEventArgs(UInt64 cellNumber, SqliteCellHeader cellHeader)
        {
            this.CellNumber = cellNumber;
            this.CellHeader = cellHeader;
        }
    }

    public class PageEventArgs : EventArgs
    {
        public UInt64 PageNumber { get; }
        public SqlitePageHeader PageHeader { get; }

        internal PageEventArgs(UInt64 pageNumber, SqlitePageHeader pageHeader)
        {
            this.PageNumber = pageNumber;
            this.PageHeader = pageHeader;
        }
    }
}
