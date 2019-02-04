# SqliteParser class library

**[SqliteParser](https://github.com/vurdalakov/sqliteparser)** is a .NET class library to parse [SQLite](https://www.sqlite.org/index.html) database `.db` files using only binary file read operations.

### Examples

#### Print SQLite database master table ("sqlite_master")

```
using (var reader = new SqliteFileReader(dbFilePath))
{
    reader.MasterTableRecordRead += (s, e) => Console.WriteLine($"{e.Record.Type}\t{e.Record.Name}\t{e.Record.TableName}\t{e.Record.RootPage}\t{e.Record.Sql}");

    reader.ReadMasterTable();
}
```

#### Print any table from SQLite database

```
using (var reader = new SqliteFileReader(dbFilePath))
{
    reader.TableRecordRead += (s, e) =>
    {
        foreach (var field in e.Fields)
        {
            Console.Write($"{field.Value}\t");
        }
        Console.WriteLine();
    };

    reader.ReadTable(tableName);
}
```

#### Extract all strings from a SQLite database file

```
using (var parser = new SqliteFileParser(dbFilePath))
{
    parser.FieldRead += (s, e) =>
    {
        if (FieldType.String == e.Type)
        {
            Console.WriteLine(e.Value as String);
        }
    };

    parser.Parse();
}
```

### Some features

* Supports `SQLITE_UTF8`, `SQLITE_UTF16LE` and `SQLITE_UTF16BE` text encodings.
* By default only blob sizes are reported. Set `ReportBlobSizesOnly` property to `false` to receive blobs themselves.

### References

* [SQLite Database File Format](https://sqlite.org/fileformat2.html)

### License

`SqliteParser` library is distributed under the [MIT license](http://opensource.org/licenses/MIT).
