using Maple2Storage.Types;

namespace MapleServer2.Types;

public class HealingSpot
{
    public CoordS Coord { get; }

    public HealingSpot(CoordS coord)
    {
        Coord = coord;
    }
}
