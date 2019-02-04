// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class MasterTableRecordReadEventArgs : EventArgs
    {
        public MasterTableRecord Record { get; }

        internal MasterTableRecordReadEventArgs(Field[] fields)
        {
            this.Record = new MasterTableRecord(fields[0].Value as String, fields[1].Value as String, fields[2].Value as String, (Byte)fields[3].Value, fields[4].Value as String);
        }
    }

    public class TableRecordReadEventArgs : EventArgs
    {
        public Field[] Fields { get; }

        internal TableRecordReadEventArgs(Field[] fields)
        {
            this.Fields = fields;
        }
    }
}
