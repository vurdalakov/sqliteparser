// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;

    public class MasterTableRecord
    {
        public String Type { get; }
        public String Name { get; }
        public String TableName { get; }
        public UInt64 RootPage { get; }
        public String Sql { get; }

        internal MasterTableRecord(String type, String name, String tableName, UInt64 rootPage, String sql)
        {
            this.Type = type;
            this.Name = name;
            this.TableName = tableName;
            this.RootPage = rootPage;
            this.Sql = sql;
        }
    }
}
