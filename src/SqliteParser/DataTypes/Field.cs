// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class Field
    {
        public FieldType Type { get; }
        public Object Value { get; }

        internal Field(FieldType type, Object value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
