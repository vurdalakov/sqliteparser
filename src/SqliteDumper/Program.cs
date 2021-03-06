﻿// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    class Program
    {
        static void Main(String[] args)
        {
            var application = new Application();
            application.Run();
        }
    }
}
