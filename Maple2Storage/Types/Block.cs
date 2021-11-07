namespace Maple2Storage.Types;

public static class Block
{
    public const int BLOCK_SIZE = 150;

    public static CoordF ClosestBlock(CoordF coord)
    {
        return CoordF.From(
            MathF.Round(coord.X / BLOCK_SIZE) * BLOCK_SIZE,
            MathF.Round(coord.Y / BLOCK_SIZE) * BLOCK_SIZE,
            MathF.Floor(coord.Z / BLOCK_SIZE) * BLOCK_SIZE
        );
    }

    public static CoordS ClosestBlock(CoordS coord)
    {
        return CoordS.From(
            (short) (MathF.Round((float) coord.X / BLOCK_SIZE) * BLOCK_SIZE),
            (short) (MathF.Round((float) coord.Y / BLOCK_SIZE) * BLOCK_SIZE),
            (short) (MathF.Floor((float) coord.Z / BLOCK_SIZE) * BLOCK_SIZE)
        );
    }
}
