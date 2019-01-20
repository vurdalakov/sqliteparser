namespace Vurdalakov.SqliteParser
{
    public enum CellType
    {
        IndexInterior = 0x02,
        TableInterior = 0x05,
        IndexLeaf = 0x0A,
        TableLeaf = 0x0D
    }
}
