namespace Vurdalakov.SqliteParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class SqliteFileReader : IDisposable
    {
        private SqliteFileParser _parser;

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

        public MasterTableRecord[] ReadMasterTable()
        {
            // get master table pages

            var masterTablePages = new List<UInt64>();

            this._parser.CellFinished += (s, e) => masterTablePages.Add(e.CellHeader.LeftChildPage);
            this._parser.PageFinished += (s, e) => masterTablePages.Add(e.PageHeader.RightMostPointer);

            this._parser.ParsePage(1);

            // read sqlite_master

            var masterTableRecords = new List<MasterTableRecord>();

            this._parser.ClearAllEvents();
            this._parser.PayloadRead += (s, e) =>
                masterTableRecords.Add(new MasterTableRecord(e.Fields[0].Value as String, e.Fields[1].Value as String, e.Fields[2].Value as String, (Byte)e.Fields[3].Value, e.Fields[4].Value as String));

            foreach (var masterTablePage in masterTablePages)
            {
                this._parser.ParsePage(masterTablePage);
            }

            return masterTableRecords.ToArray();
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
