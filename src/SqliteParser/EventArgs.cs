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
        public CellType CellType { get; }
        public UInt64 CellCount { get; }

        public PageEventArgs(UInt64 pageNumber, CellType cellType, UInt64 cellCount)
        {
            this.PageNumber = pageNumber;
            this.CellType = cellType;
            this.CellCount = cellCount;
        }
    }
}
