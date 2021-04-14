using Maple2Storage.Enums;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Cube
    {
        public long Id;
        public Item Item;
        public int PlotNumber;

        public Cube(Item item, int plotNumber)
        {
            Id = GuidGenerator.Long();
            Item = item;
        }
    }
}
