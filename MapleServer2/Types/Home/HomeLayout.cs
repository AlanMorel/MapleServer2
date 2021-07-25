using System.Collections.Generic;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class HomeLayout
    {
        public readonly long Uid;
        public Home Home;
        public int Id;
        public string Name;
        public byte Size;
        public byte Height;
        public long Timestamp;
        public List<Cube> Cubes;

        public HomeLayout() { }

        public HomeLayout(Home home, int layoutId, string layoutName, byte size, byte height, long timestamp, List<Cube> cubes)
        {
            Home = home;
            Id = layoutId;
            Name = layoutName;
            Size = size;
            Height = height;
            Timestamp = timestamp;
            Uid = DatabaseManager.AddLayout(this);

            Cubes = new List<Cube>();
            foreach (Cube cube in cubes)
            {
                Cubes.Add(new Cube(cube.Item, cube.PlotNumber, cube.CoordF, cube.Rotation, homeLayout: this));
            }
        }
    }
}
