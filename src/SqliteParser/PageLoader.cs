namespace Vurdalakov.SqliteParser
{
    using System;
    using System.IO;
    using System.Text;

    internal class PageLoader
    {
        private BinaryReader _streamReader;

        public UInt64 PageSize { get; private set; }
        public UInt64 PageUsableSize { get; private set; }
        public UInt64 PageCount { get; private set; }
        public Encoding Encoding { get; private set; }

        public PageLoader(Stream stream, SqliteFileHeader fileHeader)
        {
            stream.Seek(0, SeekOrigin.End);
            var fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            this._streamReader = new BinaryReader(stream);

            var dbHeader = this._streamReader.ReadBytes(100);

            var magicHeaderString = new Byte[] { 0x53, 0x51, 0x4c, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6f, 0x72, 0x6d, 0x61, 0x74, 0x20, 0x33, 0x00 };
            for (var i = 0; i < magicHeaderString.Length; i++)
            {
                if (dbHeader[i] != magicHeaderString[i])
                {
                    throw new Exception("Magic header string not found");
                }
            }

            this.PageSize = dbHeader.ToInt16(16);
            this.PageUsableSize = this.PageSize - dbHeader[20];
            this.PageCount = (UInt64)fileSize / this.PageSize;

            var encoding = (UInt32)dbHeader.ToInt32(56);
            switch (encoding)
            {
                case 1:
                    this.Encoding = Encoding.UTF8;
                    break;
                case 2:
                    this.Encoding = Encoding.Unicode;
                    break;
                case 3:
                    this.Encoding = Encoding.BigEndianUnicode;
                    break;
                default:
                    throw new Exception($"Not supported text encoding '{encoding}'");
            }

            fileHeader.PageSize = (UInt16)dbHeader.ToInt16(16);
            fileHeader.FileFormatWriteVersion = dbHeader[18];
            fileHeader.FileFormatReadVersion = dbHeader[19];
            fileHeader.PageReservedSize = dbHeader[20];
            fileHeader.MaximumEmbeddedPayloadFraction = dbHeader[21];
            fileHeader.MinimumEmbeddedPayloadFraction = dbHeader[22];
            fileHeader.LeafPayloadFraction = dbHeader[23];
            fileHeader.FileChangeCounter = (UInt32)dbHeader.ToInt32(24);
            fileHeader.PageCount = (UInt32)dbHeader.ToInt32(28);
            fileHeader.FirstFreelistPage = (UInt32)dbHeader.ToInt32(32);
            fileHeader.FreelistPageCount = (UInt32)dbHeader.ToInt32(36);
            fileHeader.SchemaCookie = (UInt32)dbHeader.ToInt32(40);
            fileHeader.SchemaFormatNumber = (UInt32)dbHeader.ToInt32(44);
            fileHeader.DefaultPageCacheSize = (UInt32)dbHeader.ToInt32(48);
            fileHeader.LargestRootBtreePageNumber = (UInt32)dbHeader.ToInt32(52);
            fileHeader.DatabaseTextEncoding = (UInt32)dbHeader.ToInt32(56);
            fileHeader.UserVersion = (UInt32)dbHeader.ToInt32(60);
            fileHeader.IncrementalVacuumMode = (UInt32)dbHeader.ToInt32(64);
            fileHeader.ApplicationId = (UInt32)dbHeader.ToInt32(68);
            fileHeader.VersionValidForNumber = (UInt32)dbHeader.ToInt32(92);
            fileHeader.SqliteVersionNumber = (UInt32)dbHeader.ToInt32(96);

            fileHeader.SqliteVersionNumberParsed = $"{fileHeader.SqliteVersionNumber / 1000000}.{(fileHeader.SqliteVersionNumber % 1000000) / 1000}.{fileHeader.SqliteVersionNumber % 1000}";
        }

        public void LoadPage(UInt64 pageNumber, Byte[] bytes)
        {
            if (bytes.Length < (Int32)this.PageSize)
            {
                throw new Exception("Buffer is too small");
            }
            if ((0 == pageNumber) || (pageNumber > this.PageCount))
            {
                throw new Exception("Invalid page number");
            }

            this._streamReader.BaseStream.Position = (Int64)((pageNumber - 1) * this.PageSize);
            this._streamReader.Read(bytes, 0, (Int32)this.PageSize);
        }
    }
}
