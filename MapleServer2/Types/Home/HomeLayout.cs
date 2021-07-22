using System.Collections.Generic;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class HomeLayout
    {
        private readonly long Uid;
        public Home Home;
        public int Id;
        public string Name;
        public byte Size;
        public byte Height;
        public List<Cube> Cubes;

        public HomeLayout() { }

        public HomeLayout(Home home, int layoutId, string layoutName, byte size, byte height, List<Cube> cubes)
        {
            Home = home;
            Id = layoutId;
            Name = layoutName;
            Size = size;
            Height = height;
            Cubes = cubes;

            // Uid = DatabaseManager.AddLayout(this);
        }
    }
}
