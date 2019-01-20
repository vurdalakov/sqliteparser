namespace Vurdalakov.SqliteParser
{
    using System;

    public enum SerialType : UInt64
    {
        Null = 0,
        Integer8 = 1,
        Integer16 = 2,
        Integer24 = 3,
        Integer32 = 4,
        Integer48 = 5,
        Integer64 = 6,
        Float64 = 7,
        Zero = 8,
        One = 9,
        Internal1 = 10,
        Internal2 = 11,
        Blob = 12,
        String = 13
    }
}
