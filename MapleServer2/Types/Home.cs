using System.Collections.Generic;
using Maple2Storage.Types;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Home
    {
        public readonly long Id;
        public long AccountId { get; set; }
        public int MapId { get; set; }
        public int PlotId { get; set; }
        public int PlotNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public long Expiration { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Size { get; set; }
        public byte Height { get; set; }
        public int ArchitectScoreCurrent { get; set; }
        public int ArchitectScoreTotal { get; set; }

        // Interior Settings
        public byte Lighting { get; set; }
        public byte Background { get; set; }
        public byte Camera { get; set; }

        // Permissions
        public bool IsPrivate => !Password.Equals("******");
        public string Password { get; set; }
        public Dictionary<HomePermission, byte> Permissions { get; set; }
        public List<long> BuildingPermissions = new List<long>();

        public readonly Dictionary<long, Item> WarehouseInventory = new Dictionary<long, Item>();
        public List<Item> WarehouseItems { get; set; }
        public readonly Dictionary<long, Cube> FurnishingInventory = new Dictionary<long, Cube>();
        public List<Cube> FurnishingCubes { get; set; }

        public Home() { }

        public Home(Account account, string houseName, int homeTemplate)
        {
            AccountId = account.Id;
            Name = houseName;
            Description = "Thanks for visiting. Come back soon!";
            MapId = (int) Map.PrivateResidence;
            PlotId = 0;
            PlotNumber = 0;
            ApartmentNumber = 0;
            Expiration = 32503561200; // Year 2999
            Password = "******";
            Permissions = new Dictionary<HomePermission, byte>();
            GenerateTemplate(this, homeTemplate);

            Id = DatabaseManager.CreateHouse(this);
        }

        private static void GenerateTemplate(Home home, int homeTemplate)
        {
            if (homeTemplate == 0)
            {
                home.Size = 4;
                home.Height = 5;
                HomeTemplates.WoodlandPath().ForEach(x => home.FurnishingInventory.Add(x.Uid, x));
            }
        }
    }

    public static class HomeTemplates
    {
        public static List<Cube> WoodlandPath()
        {
            return new List<Cube>
            {
                new Cube(new Item(50100045), 1, CoordF.From(-300, -450, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100079), 1, CoordF.From(-150, -450, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100079), 1, CoordF.From(0, -450, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100079), 1, CoordF.From(-450, -150, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100079), 1, CoordF.From(-450, 0, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100079), 1, CoordF.From(-300, 0, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100079), 1, CoordF.From(-150, 0, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100079), 1, CoordF.From(0, -150, 0), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100079), 1, CoordF.From(0, -300, 0), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100079), 1, CoordF.From(-150, -300, 0), CoordF.From(0, 0, 270)),
                new Cube(new Item(50200524), 1, CoordF.From(-450, 0, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50200524), 1, CoordF.From(-300, 0, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100247), 1, CoordF.From(-150, 0, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100247), 1, CoordF.From(0, 0, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100000), 1, CoordF.From(-150, -300, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50200004), 1, CoordF.From(-300, -450, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50200378), 1, CoordF.From(-450, -150, 300), CoordF.From(0, 0, 270)),
                new Cube(new Item(50200373), 1, CoordF.From(-300, -150, 300), CoordF.From(0, 0, 270)),
                new Cube(new Item(50200368), 1, CoordF.From(-150, 0, 450), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100290), 1, CoordF.From(-450, -300, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100290), 1, CoordF.From(-150, -450, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100289), 1, CoordF.From(0, -450, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100289), 1, CoordF.From(-150, -150, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100289), 1, CoordF.From(-450, -150, 150), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100290), 1, CoordF.From(-300, -150, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100290), 1, CoordF.From(-150, 0, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100289), 1, CoordF.From(0, 0, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100289), 1, CoordF.From(-450, -450, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50200591), 1, CoordF.From(-150, -450, 300), CoordF.From(0, 0, 90)),
                new Cube(new Item(50200592), 1, CoordF.From(0, -450, 300), CoordF.From(0, 0, 90)),
                new Cube(new Item(50400109), 1, CoordF.From(0, 0, 450), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100006), 1, CoordF.From(0, -300, 150), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100006), 1, CoordF.From(-300, -300, 0), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100000), 1, CoordF.From(0, -150, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100006), 1, CoordF.From(0, -150, 150), CoordF.From(0, 0, 180))
            };
        }
    }
}
