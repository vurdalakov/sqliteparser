namespace Vurdalakov.SqliteParser
{
    using System;

    public class MasterTableRecordReadEventArgs : EventArgs
    {
        public MasterTableRecord Record { get; }

        internal MasterTableRecordReadEventArgs(SqliteField[] fields)
        {
            this.Record = new MasterTableRecord(fields[0].Value as String, fields[1].Value as String, fields[2].Value as String, (Byte)fields[3].Value, fields[4].Value as String);
        }
    }

    public class TableRecordReadEventArgs : EventArgs
    {
        public SqliteField[] Fields { get; }

        internal TableRecordReadEventArgs(SqliteField[] fields)
        {
            this.Fields = fields;
        }
    }
}
