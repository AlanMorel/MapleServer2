using Maple2Storage.Types;

namespace MapleServer2.Types;

public class HealingSpot
{
    public int ObjectId { get; set; }
    public CoordS Coord { get; set; }

    public HealingSpot(int objectId, CoordS coord)
    {
        ObjectId = objectId;
        Coord = coord;
    }
}
