using Maple2Storage.Types;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Cube
{
    public long Uid;
    public Item Item;
    public int PlotNumber;
    public CoordF CoordF;
    public CoordF Rotation;
    public long HomeId;
    public long LayoutUid;
    public CubePortalSettings PortalSettings;
    public bool InUse;

    public Cube() { }

    public Cube(Item item, int plotNumber, CoordF coordF, CoordF rotation, long homeLayoutId = 0, long homeId = 0)
    {
        Item = item;
        PlotNumber = plotNumber;
        CoordF = coordF;
        Rotation = rotation;
        HomeId = homeId;
        LayoutUid = homeLayoutId;
        if (item.Id == 50400158) // Portal cube
        {
            PortalSettings = new(coordF.ToByte());
        }

        Uid = DatabaseManager.Cubes.Insert(this);
    }

    public Cube(long uid, Item item, int plotNumber, CoordF coordF, float rotation, long homeLayoutUid, long homeId, CubePortalSettings portalSettings)
    {
        Uid = uid;
        Item = item;
        PlotNumber = plotNumber;
        CoordF = coordF;
        HomeId = homeId;
        LayoutUid = homeLayoutUid;
        Rotation = CoordF.From(0, 0, rotation);
        PortalSettings = portalSettings;
    }
}
public class CubePortalSettings
{
    public string PortalName { get; set; }
    public UGCPortalMethod Method { get; set; }
    public UGCPortalDestination Destination { get; set; }
    public string DestinationTarget { get; set; }
    public int PortalObjectId { get; set; }

    public CubePortalSettings() { }

    public CubePortalSettings(CoordB coordB)
    {
        PortalName = $"Portal_{Math.Abs(coordB.X):D2}.{Math.Abs(coordB.Y):D2}.{Math.Abs(coordB.Z):D2}";
    }
}
