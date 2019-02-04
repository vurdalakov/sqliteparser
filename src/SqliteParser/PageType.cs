// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    public enum PageType
    {
        Unused = 0x00,
        IndexInterior = 0x02,
        TableInterior = 0x05,
        IndexLeaf = 0x0A,
        TableLeaf = 0x0D
    }
}
