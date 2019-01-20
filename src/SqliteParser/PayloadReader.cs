namespace Vurdalakov.SqliteParser
{
    using System;
    using System.Text;

    internal class PayloadReader : PageReader
    {
        private UInt64 _payloadSize;
        private UInt64 _payloadSizeOnPage;
        private UInt64 _nextPage;
        private UInt64 _bytesRead;
        private UInt64 _bytesReadOnPage;
        private Boolean _isFirstPage;

        public PayloadReader(PageLoader pageLoader, UInt64 pageNumber, UInt64 payloadPosition, UInt64 payloadOffset, UInt64 payloadSize) : base(pageLoader)
        {
            this.LoadPage(pageNumber);
            this.Position = payloadPosition + payloadOffset;

            this._payloadSize = payloadSize;
            this._payloadSizeOnPage = this.CalculatePayloadSizeOfFirstPage(payloadSize);
            this._payloadSizeOnPage -= payloadOffset; // 0 for header, header length for body

            this._nextPage = 0;
            this._bytesRead = 0;
            this._bytesReadOnPage = 0;
            this._isFirstPage = true;
        }

        override public Byte Read8()
        {
            if (this._bytesReadOnPage < this._payloadSizeOnPage)
            {
                this._bytesReadOnPage++;
                this._bytesRead++;

                return base.Read8();
            }

            if (this._isFirstPage)
            {
                this._nextPage = this.Read32Raw();
                this._isFirstPage = false;
            }

            if (0 == this._nextPage)
            {
                throw new Exception("Not enough data");
            }

            // read overflow page
            this.LoadPage(this._nextPage);

            // first 4 bytes are next overflow page number, all other usable bytes are used
            this._nextPage = this.Read32Raw();
            this._payloadSizeOnPage = this.PageUsableSize - 4;

            this._bytesReadOnPage = 1;
            this._bytesRead++;

            return base.Read8();
        }

        public String ReadString(UInt64 length)
        {
            var decoder = this.GetCharDecoder();
            var stringBuilder = new StringBuilder((Int32)length);

            var bytes = new Byte[1];
            var chars = new char[16];

            for (var i = 0UL; i < length; i++)
            {
                bytes[0] = this.Read8();

                var charCount = decoder.GetChars(bytes, 0, 1, chars, 0);

                for (var j = 0; j < charCount; j++)
                {
                    stringBuilder.Append(chars[j]);
                }
            }

            return stringBuilder.ToString();
        }

        public Byte[] ReadBlob(UInt64 length)
        {
            var bytes = new Byte[length];

            for (var i = 0UL; i < length; i++)
            {
                bytes[i] = this.Read8();
            }

            return bytes;
        }

        public void Skip(UInt64 length)
        {
            for (var i = 0UL; i < length; i++)
            {
                this.Read8();
            }
        }

        private UInt64 CalculatePayloadSizeOfFirstPage(UInt64 payloadSize)
        {
            var x = this.PageUsableSize - 35;

            if (payloadSize <= x)
            {
                return payloadSize;
            }
            else
            {
                var m = ((this.PageUsableSize - 12) * 32 / 255) - 23;
                var k = m + ((payloadSize - m) % (this.PageUsableSize - 4));

                if (k <= x)
                {
                    return k;
                }
                else
                {
                    return m;
                }
            }
        }
    }
}
