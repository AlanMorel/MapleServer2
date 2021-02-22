namespace Maple2Storage.Types
{
    public static class Block
    {
        public const int BLOCK_SIZE = 150;

        public static CoordF ClosestBlock(CoordF coord)
        {
            return CoordF.From(
                ((int) coord.X + 75) / BLOCK_SIZE * BLOCK_SIZE,
                ((int) coord.Y + 75) / BLOCK_SIZE * BLOCK_SIZE,
                ((int) coord.Z + 75) / BLOCK_SIZE * BLOCK_SIZE
            );
        }

        public static CoordS ClosestBlock(CoordS coord)
        {
            return CoordS.From(
                (short) ((coord.X + 75) / BLOCK_SIZE * BLOCK_SIZE),
                (short) ((coord.Y + 75) / BLOCK_SIZE * BLOCK_SIZE),
                (short) ((coord.Z + 75) / BLOCK_SIZE * BLOCK_SIZE)
            );
        }
    }
}
