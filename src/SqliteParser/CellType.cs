namespace Vurdalakov.SqliteParser
{
    public enum CellType
    {
        Unused = 0x00,
        IndexInterior = 0x02,
        TableInterior = 0x05,
        IndexLeaf = 0x0A,
        TableLeaf = 0x0D
    }
}
