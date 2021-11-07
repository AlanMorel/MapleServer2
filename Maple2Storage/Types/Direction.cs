namespace Maple2Storage.Types;

public static class Direction
{
    public const int SOUTH_EAST = 0;
    public const int NORTH_EAST = 90;
    public const int NORTH_WEST = 180;
    public const int SOUTH_WEST = 270;

    public static int GetClosestDirection(CoordF rotation)
    {
        int[] directions = new int[4]
        {
            SOUTH_EAST, NORTH_EAST, NORTH_WEST, SOUTH_WEST
        };

        return directions.OrderBy(x => Math.Abs(x - rotation.Z)).First();
    }
}
