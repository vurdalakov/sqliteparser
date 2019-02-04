// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class PageHeader
    {
        public PageType PageType { get; internal set; }
        public UInt64 FirstFreeblockOffset { get; internal set; }
        public UInt64 CellCount { get; internal set; }
        public UInt64 CellContentAreaOffset { get; internal set; }
        public UInt64 FragmentedFreeBytesCount { get; internal set; }
        public UInt64 RightMostPointer { get; internal set; }
    }
}
