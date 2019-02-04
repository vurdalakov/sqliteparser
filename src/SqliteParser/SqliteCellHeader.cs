namespace Vurdalakov.SqliteParser
{
    using System;

    public class SqliteCellHeader
    {
        public CellType PageType { get; internal set; }
        public UInt64 LeftChildPage { get; internal set; }
        public UInt64 PayloadSize { get; internal set; }
        public UInt64 Rowid { get; internal set; }
    }
}
