namespace Vurdalakov.SqliteParser
{
    using System;

    class Program
    {
        static void Main(String[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: SqliteFileStrings <path to SQLite database file>");
                return;
            }

            ExtractStrings(args[0]);
        }

        private static void ExtractStrings(String dbFilePath)
        {
            Console.WriteLine($"Database file path: '{dbFilePath}'");
            Console.WriteLine("-----------------------------------------------------------");

            var parser = new SqliteFileParser();

            parser.FieldRead += (s, e) =>
            {
                if (SerialType.String == e.Type)
                {
                    Console.WriteLine(e.Value as String);
                }
            };

            parser.Parse(dbFilePath);
        }
    }
}
