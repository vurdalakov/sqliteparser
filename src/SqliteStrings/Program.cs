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

            using (var parser = new SqliteFileParser(dbFilePath))
            {
                parser.FieldRead += (s, e) =>
                {
                    if (SerialType.String == e.Type)
                    {
                        var text = e.Value as String;
                        if (!String.IsNullOrEmpty(text))
                        {
                            Console.WriteLine(text);
                        }
                    }
                };

                parser.Parse();
            }
        }
    }
}
