// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class FieldEventArgs : EventArgs
    {
        public UInt64 FieldNumber { get; }
        public FieldType Type { get; }
        public Object Value { get; }

        internal FieldEventArgs(UInt64 fieldNumber, FieldType type, Object value)
        {
            this.FieldNumber = fieldNumber;
            this.Type = type;
            this.Value = value;
        }
    }

    public class PayloadEventArgs : EventArgs
    {
        public UInt64 CellNumber { get; }
        public CellHeader CellHeader { get; }
        public Field[] Fields { get; }

        internal PayloadEventArgs(UInt64 cellNumber, CellHeader cellHeader, Field[] fields)
        {
            this.CellNumber = cellNumber;
            this.CellHeader = cellHeader;
            this.Fields = fields;
        }
    }

    public class CellEventArgs : EventArgs
    {
        public UInt64 CellNumber { get; }
        public CellHeader CellHeader { get; }

        internal CellEventArgs(UInt64 cellNumber, CellHeader cellHeader)
        {
            this.CellNumber = cellNumber;
            this.CellHeader = cellHeader;
        }
    }

    public class PageEventArgs : EventArgs
    {
        public UInt64 PageNumber { get; }
        public PageHeader PageHeader { get; }

        internal PageEventArgs(UInt64 pageNumber, PageHeader pageHeader)
        {
            this.PageNumber = pageNumber;
            this.PageHeader = pageHeader;
        }
    }
}
