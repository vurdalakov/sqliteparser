namespace Vurdalakov.SqliteParser
{
    using System;

    public class SqliteFileHeader
    {
        public UInt16 PageSize { get; internal set; }
        public Byte FileFormatWriteVersion { get; internal set; }
        public Byte FileFormatReadVersion { get; internal set; }
        public Byte PageReservedSize { get; internal set; }
        public Byte MaximumEmbeddedPayloadFraction { get; internal set; }
        public Byte MinimumEmbeddedPayloadFraction { get; internal set; }
        public Byte LeafPayloadFraction { get; internal set; }
        public UInt32 FileChangeCounter { get; internal set; }
        public UInt32 PageCount { get; internal set; }
        public UInt32 FirstFreelistPage { get; internal set; }
        public UInt32 FreelistPageCount { get; internal set; }
        public UInt32 SchemaCookie { get; internal set; }
        public UInt32 SchemaFormatNumber { get; internal set; }
        public UInt32 DefaultPageCacheSize { get; internal set; }
        public UInt32 LargestRootBtreePageNumber { get; internal set; }
        public UInt32 DatabaseTextEncoding { get; internal set; }
        public UInt32 UserVersion { get; internal set; }
        public UInt32 IncrementalVacuumMode { get; internal set; }
        public UInt32 ApplicationId { get; internal set; }
        public UInt32 VersionValidForNumber { get; internal set; }
        public UInt32 SqliteVersionNumber { get; internal set; }
        public String SqliteVersionNumberParsed { get; internal set; }
    }
}
