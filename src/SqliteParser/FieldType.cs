// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public enum FieldType : UInt64
    {
        Null = 0,
        Integer8 = 1,
        Integer16 = 2,
        Integer24 = 3,
        Integer32 = 4,
        Integer48 = 5,
        Integer64 = 6,
        Float64 = 7,
        Zero = 8,
        One = 9,
        Internal1 = 10,
        Internal2 = 11,
        Blob = 12,
        String = 13
    }
}
