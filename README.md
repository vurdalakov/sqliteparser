# SqliteParser class library

**[SqliteParser](https://github.com/vurdalakov/sqliteparser)** is a .NET class library to parse [SQLite](https://www.sqlite.org/index.html) database `.db` files using only binary file read operations.

### Example

The following C# code extracts all strings from a SQLite database file.

```
using Vurdalakov.SqliteParser;

...

public static void ExtractStrings(String dbFilePath)
{
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
```

### Some features

* Supports `SQLITE_UTF8`, `SQLITE_UTF16LE` and `SQLITE_UTF16BE` text encodings.
* By default only blob sizes are reported. Set `ReportBlobSizesOnly` property to `false` to receive blobs themselves.

### References

* [SQLite Database File Format](https://sqlite.org/fileformat2.html)

* [Parsing SQLite Database Schema in sqlitedb file?](https://stackoverflow.com/questions/21936528/parsing-sqlite-database-schema-in-sqlitedb-file)

### License

`SqliteParser` library is distributed under the [MIT license](http://opensource.org/licenses/MIT).
