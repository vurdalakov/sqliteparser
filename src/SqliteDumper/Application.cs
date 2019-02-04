// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov.SqliteParser
{
    using System;
    using System.IO;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            var command = "";
            var dbFilePath = "";

            if (1 == this._commandLineParser.FileNames.Length)
            {
                command = "all";
                dbFilePath = this._commandLineParser.FileNames[0];
            }
            else if (2 == this._commandLineParser.FileNames.Length)
            {
                command = this._commandLineParser.FileNames[0];
                dbFilePath = this._commandLineParser.FileNames[1];
            }
            else
            {
                this.Help();
            }

            using (var parser = new SqliteFileParser(dbFilePath))
            {
                switch (command)
                {
                    case "all":
                        this.DumpFileInfo(dbFilePath);
                        PrintLine();
                        this.DumpFileHeader(parser);
                        PrintLine();
                        this.DumpFilePages(parser);
                        break;
                    case "h":
                    case "header":
                        this.DumpFileHeader(parser);
                        break;
                    case "p":
                    case "pages":
                        this.DumpFilePages(parser);
                        break;
                    case "m":
                    case "master":
                        this.DumpTables(parser);
                        return 0;
                    case "t":
                    case "table":
                        if (this._commandLineParser.OptionHasValue("name"))
                        {
                            this.DumpTable(parser, this._commandLineParser.GetOptionString("name"));
                        }
                        else
                        {
                            this.Help();
                        }
                        return 0;
                    default:
                        this.Help();
                        return 0;
                }

                parser.ParseAllPages();
            }

            return 0;

            void PrintLine() => Console.WriteLine("------------------------------------------------------------------------------");
        }

        protected override void Help()
        {
            Console.WriteLine("SqliteDumper {0} | https://github.com/vurdalakov/sqliteparser\n", this.ApplicationVersion);
            Console.WriteLine("Dumps an SQLite database file using only binary file reading operations.\n");
            Console.WriteLine("Usage:\n\tSqliteDumper [command] <filename.db> [-silent]\n");
            Console.WriteLine("Commands:\n\tall - show all info\n\theader - show file header\n\tpages - show file pages\n\tmaster - print sqlite_master table\n\ttable - print specific table\n");
            Console.WriteLine("Options:\n\t-name=<table_name> - table name for 'table' command\n\t-silent - no error messages are shown; check exit code\n");
            Console.WriteLine("Exit codes:\n\t0 - conversion succeeded\n\t1 - conversion failed\n\t-1 - invalid command line syntax\n");
            
            base.Help();
        }

        private void DumpFileInfo(String dbFilePath)
        {
            Console.WriteLine($"File path:\t{dbFilePath}");

            var fileInfo = new FileInfo(dbFilePath);
            Console.WriteLine($"File size:\t{fileInfo.Length:N0}");
            Console.WriteLine($"File created:\t{fileInfo.CreationTime}");
            Console.WriteLine($"File modified:\t{fileInfo.LastWriteTime}");
            Console.WriteLine($"File accessed:\t{fileInfo.LastAccessTime}");
        }

        private void DumpFileHeader(SqliteFileParser parser)
        {
            Console.WriteLine($"The database page size in bytes:\t\t{parser.FileHeader.PageSize}");
            Console.WriteLine($"File format write version:\t\t\t{parser.FileHeader.FileFormatWriteVersion}");
            Console.WriteLine($"File format read version:\t\t\t{parser.FileHeader.FileFormatReadVersion}");
            Console.WriteLine($"Bytes of unused space:\t\t\t\t{parser.FileHeader.PageReservedSize}");
            Console.WriteLine($"Maximum embedded payload fraction:\t\t{parser.FileHeader.MaximumEmbeddedPayloadFraction}");
            Console.WriteLine($"Minimum embedded payload fraction:\t\t{parser.FileHeader.MinimumEmbeddedPayloadFraction}");
            Console.WriteLine($"Leaf payload fraction:\t\t\t\t{parser.FileHeader.LeafPayloadFraction}");
            Console.WriteLine($"File change counter:\t\t\t\t{parser.FileHeader.FileChangeCounter}");
            Console.WriteLine($"Size of the database file in pages:\t\t{parser.FileHeader.PageCount}");
            Console.WriteLine($"Page number of the first freelist trunk page:\t{parser.FileHeader.FirstFreelistPage}");
            Console.WriteLine($"Total number of freelist pages:\t\t\t{parser.FileHeader.FreelistPageCount}");
            Console.WriteLine($"The schema cookie:\t\t\t\t{parser.FileHeader.SchemaCookie}");
            Console.WriteLine($"The schema format number:\t\t\t{parser.FileHeader.SchemaFormatNumber}");
            Console.WriteLine($"Default page cache size:\t\t\t{parser.FileHeader.DefaultPageCacheSize}");
            Console.WriteLine($"Page number of the largest root b-tree page:\t{parser.FileHeader.LargestRootBtreePageNumber}");
            Console.WriteLine($"The database text encoding:\t\t\t{parser.FileHeader.DatabaseTextEncoding}");
            Console.WriteLine($"The 'user version':\t\t\t\t{parser.FileHeader.UserVersion}");
            Console.WriteLine($"Incremental-vacuum mode:\t\t\t{parser.FileHeader.IncrementalVacuumMode}");
            Console.WriteLine($"The 'Application ID':\t\t\t\t{parser.FileHeader.ApplicationId}");
            Console.WriteLine($"The version-valid-for number:\t\t\t{parser.FileHeader.VersionValidForNumber}");
            Console.WriteLine($"SQLITE_VERSION_NUMBER:\t\t\t\t{parser.FileHeader.SqliteVersionNumber} ({parser.FileHeader.SqliteVersionNumberParsed})");
        }

        private void DumpFilePages(SqliteFileParser parser)
        {
            parser.PageStarted += (s, e) =>
            {
                Console.WriteLine($"--- Page {e.PageNumber}");
                Console.WriteLine($"The b-tree page type:\t\t\t{e.PageHeader.PageType} ({(Int32)e.PageHeader.PageType})");
                Console.WriteLine($"The start of the first freeblock:\t{e.PageHeader.FirstFreeblockOffset}");
                Console.WriteLine($"The number of cells:\t\t\t{e.PageHeader.CellCount}");
                Console.WriteLine($"The start of the cell content area:\t{e.PageHeader.CellContentAreaOffset}");
                Console.WriteLine($"The number of fragmented free bytes:\t{e.PageHeader.FragmentedFreeBytesCount}");

                if ((PageType.IndexInterior == e.PageHeader.PageType) || (PageType.TableInterior == e.PageHeader.PageType))
                {
                    Console.WriteLine($"The right-most pointer:\t\t\t{e.PageHeader.RightMostPointer}");
                }
            };
        }

        private void DumpTables(SqliteFileParser parser)
        {
            using (var reader = new SqliteFileReader(parser))
            {
                reader.MasterTableRecordRead += (s, e) => Console.WriteLine($"'{e.Record.Type}'\t'{e.Record.Name}'\t'{e.Record.TableName}'\t{e.Record.RootPage}\t'{e.Record.Sql}'");

                reader.ReadMasterTable();
            }
        }

        private void DumpTable(SqliteFileParser parser, String tableName)
        {
            using (var reader = new SqliteFileReader(parser))
            {
                reader.TableRecordRead += (s, e) =>
                {
                    foreach (var field in e.Fields)
                    {
                        Console.Write($"'{field.Value}'\t");
                    }
                    Console.WriteLine();
                };

                reader.ReadTable(tableName);
            }
        }
    }
}
