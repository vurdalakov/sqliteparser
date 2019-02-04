namespace Vurdalakov.SqliteParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class SqliteFileReader : IDisposable
    {
        private SqliteFileParser _parser;

        public event EventHandler<MasterTableRecordReadEventArgs> MasterTableRecordRead;
        public event EventHandler<TableRecordReadEventArgs> TableRecordRead;

        public SqliteFileReader(String dbFilePath)
        {
            this._parser = new SqliteFileParser(dbFilePath);
        }

        public SqliteFileReader(Stream stream)
        {
            this._parser = new SqliteFileParser(stream);
        }

        public SqliteFileReader(SqliteFileParser parser)
        {
            this._parser = parser;
        }

        public void ReadMasterTable()
        {
            using (var eventHandlerSaver = new EventHandlerSaver(this, nameof(this.TableRecordRead)))
            {
                this.TableRecordRead += (s, e) => this.MasterTableRecordRead?.Invoke(this, new MasterTableRecordReadEventArgs(e.Fields));

                this.ReadTable(1);
            }
        }

        public void ReadTable(String tableName)
        {
            var rootPage = 0UL;

            using (var eventHandlerSaver = new EventHandlerSaver(this, nameof(this.MasterTableRecordRead)))
            {
                this.MasterTableRecordRead += (s, e) =>
                {
                    if (e.Record.Type.Equals("table") && e.Record.Name.Equals(tableName))
                    {
                        rootPage = e.Record.RootPage;
                    }
                };

                this.ReadMasterTable();
            }

            if (rootPage > 0)
            {
                this.ReadTable(rootPage);
            }
        }

        public void ReadTable(UInt64 rootPage)
        {
            this.ReadTablePage(rootPage);
        }

        private void ReadTablePage(UInt64 page)
        {
            var childPages = new List<UInt64>();

            void AddPage(CellType cellType, UInt64 pageToAdd)
            {
                if (CellType.TableInterior == cellType)
                {
                    childPages.Add(pageToAdd);
                }
            }

            using (var eventHandlerSaver = new EventHandlerSaver(this._parser))
            {
                this._parser.CellStarted += (s, e) => AddPage(e.CellHeader.PageType, e.CellHeader.LeftChildPage);
                this._parser.PageFinished += (s, e) => AddPage(e.PageHeader.PageType, e.PageHeader.RightMostPointer);

                this._parser.PayloadRead += (s, e) => this.TableRecordRead?.Invoke(this, new TableRecordReadEventArgs(e.Fields));

                this._parser.ParsePage(page);
            }

            foreach (var childPage in childPages)
            {
                this.ReadTablePage(childPage);
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
                    this._parser.Dispose();
                    this._parser = null;
                }

                this._isDisposed = true;
            }
        }

        ~SqliteFileReader()
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
