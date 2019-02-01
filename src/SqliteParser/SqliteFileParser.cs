namespace Vurdalakov.SqliteParser
{
    using System;
    using System.IO;

    public class SqliteFileParser : IDisposable
    {
        private Stream _stream;
        private PageLoader _pageLoader;
        private PageReader _pageReader;

        public event EventHandler<PageEventArgs> PageStarted;
        public event EventHandler<PageEventArgs> PageFinished;
        public event EventHandler<CellEventArgs> CellStarted;
        public event EventHandler<CellEventArgs> CellFinished;
        public event EventHandler<FieldEventArgs> FieldRead;

        public SqliteFileHeader FileHeader { get; } = new SqliteFileHeader();

        public Boolean ReportBlobSizesOnly { get; set; } = true;

        public SqliteFileParser(String dbFilePath)
        {
            this._stream = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.Open(this._stream);
        }

        public SqliteFileParser(Stream stream)
        {
            this.Open(stream);
        }

        private void Open(Stream stream)
        {
            this._pageLoader = new PageLoader(stream, this.FileHeader);
            this._pageReader = new PageReader(this._pageLoader);
        }

        public void Parse()
        {
            for (var pageNumber = 1UL; pageNumber <= this._pageLoader.PageCount; pageNumber++)
            {
                this.ParsePage(pageNumber);
            }
        }

        public void ParsePage(UInt64 pageNumber)
        {
            this._pageReader.LoadPage(pageNumber);

            if (1 == pageNumber)
            {
                this._pageReader.Position = 100;
            }

            var pageType = (CellType)this._pageReader.Read8();
            if (CellType.Unused == pageType)
            {
                return;
            }

            var sqlitePageHeader = new SqlitePageHeader();
            sqlitePageHeader.PageType = pageType;
            sqlitePageHeader.FirstFreeblockOffset = this._pageReader.Read16();
            sqlitePageHeader.CellCount = this._pageReader.Read16();
            sqlitePageHeader.CellContentAreaOffset = this._pageReader.Read16();
            sqlitePageHeader.FragmentedFreeBytesCount = this._pageReader.Read8();

            if ((CellType.IndexInterior == pageType) || (CellType.TableInterior == pageType))
            {
                sqlitePageHeader.RightMostPointer = this._pageReader.Read32();
            }

            var pageEventArgs = new PageEventArgs(pageNumber, sqlitePageHeader);
            this.PageStarted?.Invoke(this, pageEventArgs);

            var cellPointerArrayOffset = this._pageReader.Position;

            for (var cellNumber = 0UL; cellNumber < sqlitePageHeader.CellCount; cellNumber++)
            {
                this._pageReader.Position = cellPointerArrayOffset;
                cellPointerArrayOffset += 2;

                var cellOffset = this._pageReader.Read16();

                this._pageReader.Position = cellOffset;

                var rowid = 0UL;

                if (CellType.IndexInterior == pageType)
                {
                    this._pageReader.Position += 4;
                    var payloadSize = this._pageReader.Decode64();
                    InvokeRowStartedEvent();

                    DumpPayload(pageNumber, payloadSize);
                }
                else if (CellType.TableInterior == pageType)
                {
                    this._pageReader.Position += 4;
                    rowid = this._pageReader.Decode64();
                    InvokeRowStartedEvent();
                }
                else if (CellType.IndexLeaf == pageType)
                {
                    var payloadSize = this._pageReader.Decode64();
                    InvokeRowStartedEvent();

                    DumpPayload(pageNumber, payloadSize);
                }
                else if (CellType.TableLeaf == pageType)
                {
                    var payloadSize = this._pageReader.Decode64();
                    rowid = this._pageReader.Decode64();
                    InvokeRowStartedEvent();

                    DumpPayload(pageNumber, payloadSize);
                }
                else
                {
                    InvokeRowStartedEvent();
                }

                this.CellFinished?.Invoke(this, new CellEventArgs(pageType, rowid));

                void InvokeRowStartedEvent()
                {
                    this.CellStarted?.Invoke(this, new CellEventArgs(pageType, rowid));
                }
            }

            this.PageFinished?.Invoke(this, pageEventArgs);
        }

        private void DumpPayload(UInt64 pageNumber, UInt64 payloadSize)
        {
            // create header reader
            var payloadPosition = this._pageReader.Position;
            var headerReader = new PayloadReader(this._pageLoader, pageNumber, payloadPosition, 0, payloadSize);

            // read header size
            var headerSize = headerReader.Decode64();

            // create body reader
            var dataReader = new PayloadReader(this._pageLoader, pageNumber, payloadPosition, headerSize, payloadSize);

            var bodyPosition = payloadPosition + headerSize; // header size includes header size field itself

            // read fields
            while (headerReader.Position < bodyPosition)
            {
                var serialTypeValue = headerReader.Decode64();
                var serialType = (SerialType)serialTypeValue;

                switch (serialType)
                {
                    case SerialType.Null:
                        InvokeFieldReadEvent(null);
                        break;
                    case SerialType.Integer8:
                        InvokeFieldReadEvent(dataReader.Read8());
                        break;
                    case SerialType.Integer16:
                        InvokeFieldReadEvent((UInt16)dataReader.Read16());
                        break;
                    case SerialType.Integer24:
                        InvokeFieldReadEvent((UInt32)dataReader.Read24());
                        break;
                    case SerialType.Integer32:
                        InvokeFieldReadEvent((UInt32)dataReader.Read32());
                        break;
                    case SerialType.Integer48:
                        InvokeFieldReadEvent((UInt64)dataReader.Read48());
                        break;
                    case SerialType.Integer64:
                        InvokeFieldReadEvent((UInt64)dataReader.Read64());
                        break;
                    case SerialType.Float64:
                        var integer = dataReader.Read64();
                        InvokeFieldReadEvent(integer.ToDouble());
                        break;
                    case SerialType.Zero:
                        InvokeFieldReadEvent(0);
                        break;
                    case SerialType.One:
                        InvokeFieldReadEvent(1);
                        break;
                    case SerialType.Internal1:
                        InvokeFieldReadEvent(null);
                        break;
                    case SerialType.Internal2:
                        InvokeFieldReadEvent(null);
                        break;
                    default:
                        if ((serialTypeValue >= 13) && (1 == (serialTypeValue % 2))) // string
                        {
                            var length = (serialTypeValue - 13) / 2;
                            var str = dataReader.ReadString(length);
                            InvokeFieldReadEvent2(SerialType.String, str);
                        }
                        else if ((serialTypeValue >= 12) && (0 == (serialTypeValue % 2))) // blob
                        {
                            var length = (serialTypeValue - 13) / 2;

                            if (this.ReportBlobSizesOnly)
                            {
                                dataReader.Skip(length);
                                InvokeFieldReadEvent2(SerialType.Blob, length);
                            }
                            else
                            {
                                var blob = dataReader.ReadBlob(length);
                                InvokeFieldReadEvent2(SerialType.Blob, blob);
                            }
                        }
                        else // unknown type
                        {
                            InvokeFieldReadEvent(null);
                        }
                        break;
                }

                void InvokeFieldReadEvent(Object value)
                {
                    InvokeFieldReadEvent2(serialType, value);
                }

                void InvokeFieldReadEvent2(SerialType serialType2, Object value2)
                {
                    this.FieldRead?.Invoke(this, new FieldEventArgs(serialType2, value2));
                }
            }
        }

        #region IDisposable Support

        private Boolean _isDisposed = false;

        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._pageReader = null;
                    this._pageLoader = null;

                    if (this._stream != null)
                    {
                        this._stream.Close();
                        this._stream.Dispose();
                        this._stream = null;
                    }
                }

                this._isDisposed = true;
            }
        }

        ~SqliteFileParser()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
