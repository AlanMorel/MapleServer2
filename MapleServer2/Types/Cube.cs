using Maple2Storage.Types;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class Cube
    {
        public long Uid;
        public Item Item;
        public int PlotNumber;
        public CoordF CoordF;
        public CoordF Rotation;
        public long HomeId;
        public long LayoutUid;

        public Cube() { }

        public Cube(Item item, int plotNumber, CoordF coordF, CoordF rotation, long homeLayoutId = 0, long homeId = 0)
        {
            Item = item;
            PlotNumber = plotNumber;
            CoordF = coordF;
            Rotation = rotation;
            HomeId = homeId;
            LayoutUid = homeLayoutId;

            Uid = DatabaseManager.Cubes.Insert(this);
        }

        public Cube(long uid, Item item, int plotNumber, CoordF coordF, float rotation, long homeLayoutUid = 0, long homeId = 0)
        {
            Uid = uid;
            Item = item;
            PlotNumber = plotNumber;
            CoordF = coordF;
            HomeId = homeId;
            LayoutUid = homeLayoutUid;
            Rotation = CoordF.From(0, 0, rotation);
        }
    }
}
