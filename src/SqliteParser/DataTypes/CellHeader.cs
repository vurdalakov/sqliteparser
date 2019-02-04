// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class CellHeader
    {
        public PageType PageType { get; internal set; }
        public UInt64 LeftChildPage { get; internal set; }
        public UInt64 PayloadSize { get; internal set; }
        public UInt64 Rowid { get; internal set; }
    }
}
