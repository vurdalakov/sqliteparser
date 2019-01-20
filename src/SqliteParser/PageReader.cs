namespace Vurdalakov.SqliteParser
{
    using System;
    using System.Text;

    internal class PageReader
    {
        private PageLoader _pageLoader;
        private Byte[] _bytes;

        protected UInt64 PageUsableSize => this._pageLoader.PageUsableSize;

        public UInt64 Page { get; private set; }
        public UInt64 Position { get; set; }

        public PageReader(PageLoader pageLoader)
        {
            this._pageLoader = pageLoader;

            this._bytes = new Byte[this._pageLoader.PageSize];
        }

        public void LoadPage(UInt64 pageNumber)
        {
            this._pageLoader.LoadPage(pageNumber, this._bytes);
            this.Page = pageNumber;
            this.Position = 0;
        }

        protected Decoder GetCharDecoder() => this._pageLoader.Encoding.GetDecoder();

        private Byte Read8Raw()
        {
            var data = this._bytes[this.Position];
            this.Position++;
            return data;
        }

        public UInt64 Read32Raw() => (UInt64)((this.Read8Raw() << 24) | (this.Read8Raw() << 16) | (this.Read8Raw() << 8) | this.Read8Raw());

        virtual public Byte Read8() => Read8Raw();

        public UInt64 Read16() => (UInt64)(this.Read8() << 8 | this.Read8());

        public UInt64 Read24() => (UInt64)(this.Read16() << 8 | this.Read8());

        public UInt64 Read32() => (UInt64)(this.Read16() << 16 | this.Read16());

        public UInt64 Read48() => (UInt64)(this.Read32() << 16 | this.Read16());

        public UInt64 Read64() => (UInt64)(this.Read32() << 32 | this.Read32());

        public Double ReadDouble() => this.Read64().ToDouble();

        public UInt64 Decode64()
        {
            var bits = 0UL;

            while (true)
            {
                var b = this.Read8();

                bits <<= 7;
                bits |= (Byte)(b & 0x7F);

                if (0 == (b & 0x80))
                {
                    break;
                }
            }

            return bits;
        }
    }
}
