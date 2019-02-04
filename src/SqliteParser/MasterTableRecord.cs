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
