// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
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
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: SqliteStrings <path to SQLite database file>");
                return;
            }

            ExtractStrings(args[0]);
        }

        private static void ExtractStrings(String dbFilePath)
        {
            using (var parser = new SqliteFileParser(dbFilePath))
            {
                parser.FieldRead += (s, e) =>
                {
                    if (FieldType.String == e.Type)
                    {
                        var text = e.Value as String;
                        if (!String.IsNullOrEmpty(text))
                        {
                            Console.WriteLine(text);
                        }
                    }
                };

                parser.ParseAllPages();
            }
        }
    }
}
