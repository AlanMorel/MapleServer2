using MapleServer2.Database.Classes;

namespace MapleServer2.Types
{
    public class HomeLayout
    {
        public readonly long Uid;
        public long HomeId;
        public int Id;
        public string Name;
        public byte Size;
        public byte Height;
        public long Timestamp;
        public List<Cube> Cubes;

        public HomeLayout() { }

        public HomeLayout(long homeId, int layoutId, string layoutName, byte size, byte height, long timestamp, List<Cube> cubes)
        {
            HomeId = homeId;
            Id = layoutId;
            Name = layoutName;
            Size = size;
            Height = height;
            Timestamp = timestamp;
            Uid = DatabaseHomeLayout.CreateHomeLayout(this);

            foreach (Cube cube in cubes)
            {
                _ = new Cube(cube.Item, cube.PlotNumber, cube.CoordF, cube.Rotation, Uid);
            }
        }
    }
}
