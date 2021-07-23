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
        public Home Home;
        public HomeLayout Layout;

        public Cube() { }

        public Cube(Item item, int plotNumber, CoordF coordF, CoordF rotation)
        {
            Item = item;
            PlotNumber = plotNumber;
            CoordF = coordF;
            Rotation = rotation;

            Uid = DatabaseManager.CreateCube(this);
        }
    }
}
