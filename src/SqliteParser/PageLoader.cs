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

        public PageLoader(Stream stream)
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

            var encoding = dbHeader.ToInt32(56);
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
