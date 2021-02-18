namespace Maple2Storage.Types
{
    public static class Block
    {
        public const int BLOCK_SIZE = 150;

        public static CoordF ClosestBlock(CoordF coord)
        {
            return CoordF.From(
                ((int) coord.X + 75) / 150 * 150,
                ((int) coord.Y + 75) / 150 * 150,
                ((int) coord.Z + 75) / 150 * 150
            );
        }

        public static CoordS ClosestBlock(CoordS coord)
        {
            return CoordS.From(
                (short) ((coord.X + 75) / 150 * 150),
                (short) ((coord.Y + 75) / 150 * 150),
                (short) ((coord.Z + 75) / 150 * 150)
            );
        }
    }
}
