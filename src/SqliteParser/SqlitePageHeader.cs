namespace Vurdalakov.SqliteParser
{
    using System;

    public class SqlitePageHeader
    {
        public CellType PageType { get; internal set; }
        public UInt64 FirstFreeblockOffset { get; internal set; }
        public UInt64 CellCount { get; internal set; }
        public UInt64 CellContentAreaOffset { get; internal set; }
        public UInt64 FragmentedFreeBytesCount { get; internal set; }
        public UInt64 RightMostPointer { get; internal set; }
    }
}
