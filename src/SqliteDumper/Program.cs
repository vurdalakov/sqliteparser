namespace Vurdalakov.SqliteParser
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(String[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: SqliteDumper <path to SQLite database file>");
                return;
            }

            ExtractStrings(args[0]);
        }

        private static void ExtractStrings(String dbFilePath)
        {
            Console.WriteLine($"File path:\t{dbFilePath}");
            var fileInfo = new FileInfo(dbFilePath);
            Console.WriteLine($"File size:\t{fileInfo.Length:N0}");
            Console.WriteLine($"File created:\t{fileInfo.CreationTime}");
            Console.WriteLine($"File modified:\t{fileInfo.LastWriteTime}");
            Console.WriteLine($"File accessed:\t{fileInfo.LastAccessTime}");
            Console.WriteLine("----------------------------------------------------------------");

            using (var parser = new SqliteFileParser(dbFilePath))
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
                Console.WriteLine("----------------------------------------------------------------");

                parser.Parse();
            }
        }
    }
}
