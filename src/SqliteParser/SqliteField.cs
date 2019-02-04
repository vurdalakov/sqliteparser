namespace Vurdalakov.SqliteParser
{
    using System;

    public class SqliteField
    {
        public SerialType Type { get; set; }
        public Object Value { get; set; }

        internal SqliteField(SerialType type, Object value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
