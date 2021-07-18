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
        public long DecorationExp { get; set; }
        public long DecorationLevel { get; set; }
        public List<int> InteriorRewardsClaimed { get; set; }

        private readonly long[] DecorationExpTable = new long[] { 0, 100, 300, 1000, 2100, 5500, 7700, 9900, 13200, 16500 };

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

        public Home(long accountId, string houseName, int homeTemplate)
        {
            AccountId = accountId;
            Name = houseName;
            Description = "Thanks for visiting. Come back soon!";
            MapId = (int) Map.PrivateResidence;
            PlotId = 0;
            PlotNumber = 0;
            ApartmentNumber = 0;
            DecorationLevel = 1;
            Size = 4;
            Height = 5;
            InteriorRewardsClaimed = new List<int>();
            Expiration = 32503561200; // Year 2999
            Password = "******";
            Permissions = new Dictionary<HomePermission, byte>();

            switch (homeTemplate)
            {
                case 0:
                    HomeTemplates.WoodlandPath().ForEach(x => FurnishingInventory.Add(x.Uid, x));
                    break;
                case 1:
                    HomeTemplates.PinkPerfection().ForEach(x => FurnishingInventory.Add(x.Uid, x));
                    break;
                case 2:
                    HomeTemplates.KerningBunker().ForEach(x => FurnishingInventory.Add(x.Uid, x));
                    break;
                default:
                    Size = 10;
                    Height = 5;
                    break;
            }

            Id = DatabaseManager.CreateHouse(this);
        }

        public void GainExp(long exp)
        {
            if (exp <= 0 || DecorationLevel > 9) // level 10 max
            {
                return;
            }

            if (DecorationExp + exp >= DecorationExpTable[DecorationLevel])
            {
                exp -= DecorationExpTable[DecorationLevel];
                DecorationLevel++;
            }
            DecorationExp += exp;
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

        public static List<Cube> PinkPerfection()
        {
            return new List<Cube>
            {
                new Cube(new Item(50100332), 1, CoordF.From(-450,-300,0), CoordF.From(0,0,180)),
                new Cube(new Item(50100332), 1, CoordF.From(-300,-300,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100332), 1, CoordF.From(-150,-300,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100332), 1, CoordF.From(0,-300,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100332), 1, CoordF.From(0,-450,0), CoordF.From(0,0,360)),
                new Cube(new Item(50100332), 1, CoordF.From(-150,-450,0), CoordF.From(0,0,270)),
                new Cube(new Item(50100332), 1, CoordF.From(-300,-450,0), CoordF.From(0,0,270)),
                new Cube(new Item(50100332), 1, CoordF.From(-450,-450,0), CoordF.From(0,0,0)),
                new Cube(new Item(50100365), 1, CoordF.From(-450,-150,150), CoordF.From(0,0,180)),
                new Cube(new Item(50100364), 1, CoordF.From(-450,-150,0), CoordF.From(0,0,180)),
                new Cube(new Item(50100364), 1, CoordF.From(-450,0,0), CoordF.From(0,0,180)),
                new Cube(new Item(50100364), 1, CoordF.From(-300,0,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100364), 1, CoordF.From(-300,-150,0), CoordF.From(0,0,360)),
                new Cube(new Item(50100364), 1, CoordF.From(-150,-150,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100364), 1, CoordF.From(0,-150,0), CoordF.From(0,0,90)),
                new Cube(new Item(50100364), 1, CoordF.From(-150,0,0), CoordF.From(0,0,270)),
                new Cube(new Item(50100365), 1, CoordF.From(-450,0,150), CoordF.From(0,0,180)),
                new Cube(new Item(50100365), 1, CoordF.From(-300,-300,150), CoordF.From(0,0,0)),
                new Cube(new Item(50100365), 1, CoordF.From(0,-300,150), CoordF.From(0,0,90)),
                new Cube(new Item(50100365), 1, CoordF.From(0,-150,150), CoordF.From(0,0,180)),
                new Cube(new Item(50100365), 1, CoordF.From(0,0,150), CoordF.From(0,0,270)),
                new Cube(new Item(50100365), 1, CoordF.From(-150,0,150), CoordF.From(0,0,270)),
                new Cube(new Item(50100365), 1, CoordF.From(-300,0,150), CoordF.From(0,0,270)),
                new Cube(new Item(50100365), 1, CoordF.From(-300,-150,150), CoordF.From(0,0,90)),
                new Cube(new Item(50100365), 1, CoordF.From(-150,-150,150), CoordF.From(0,0,90)),
                new Cube(new Item(50100000), 1, CoordF.From(-450,-300,150), CoordF.From(0,0,180)),
                new Cube(new Item(50100326), 1, CoordF.From(-450,0,300), CoordF.From(0,0,-180)),
                new Cube(new Item(50100326), 1, CoordF.From(-450,0,450), CoordF.From(0,0,180)),
                new Cube(new Item(50100326), 1, CoordF.From(-300,0,300), CoordF.From(0,0,180)),
                new Cube(new Item(50100326), 1, CoordF.From(-150,0,300), CoordF.From(0,0,-180)),
                new Cube(new Item(50100326), 1, CoordF.From(-150,0,450), CoordF.From(0,0,180)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-150,300), CoordF.From(0,0,90)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-150,450), CoordF.From(0,0,90)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-300,300), CoordF.From(0,0,90)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-450,150), CoordF.From(0,0,90)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-450,300), CoordF.From(0,0,90)),
                new Cube(new Item(50100325), 1, CoordF.From(0,-450,450), CoordF.From(0,0,90)),
                new Cube(new Item(50100326), 1, CoordF.From(0,0,450), CoordF.From(0,0,180)),
                new Cube(new Item(50100326), 1, CoordF.From(0,0,300), CoordF.From(0,0,180)),
                new Cube(new Item(50200632), 1, CoordF.From(-300,-150,300), CoordF.From(0,0,180)),
                new Cube(new Item(50200596), 1, CoordF.From(-150,-450,150), CoordF.From(0,0,-270)),
                new Cube(new Item(50100324), 1, CoordF.From(0,-300,450), CoordF.From(0,0,180)),
                new Cube(new Item(50100324), 1, CoordF.From(-300,0,450), CoordF.From(0,0,0)),
                new Cube(new Item(50200558), 1, CoordF.From(-450,-450,150), CoordF.From(0,0,-540)),
                new Cube(new Item(50200657), 1, CoordF.From(-150,-300,300), CoordF.From(0,0,90)),
                new Cube(new Item(50100365), 1, CoordF.From(-150,-300,150), CoordF.From(0,0,180)),
                new Cube(new Item(50200604), 1, CoordF.From(-150,-150,300), CoordF.From(0,0,-630))
            };
        }

        public static List<Cube> KerningBunker()
        {
            return new List<Cube>
            {
                new Cube(new Item(50100190), 1, CoordF.From(-450, -450, 0), CoordF.From(0, 0, -90)),
                new Cube(new Item(50100190), 1, CoordF.From(-450, -300, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100190), 1, CoordF.From(-450, -150, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100190), 1, CoordF.From(-450, 0, 0), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100097), 1, CoordF.From(-300, -450, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100097), 1, CoordF.From(-150, -450, 0), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100097), 1, CoordF.From(0, -450, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100097), 1, CoordF.From(0, -300, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100097), 1, CoordF.From(0, -150, 0), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100097), 1, CoordF.From(-150, -150, 0), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100097), 1, CoordF.From(-150, 0, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100097), 1, CoordF.From(-300, 0, 0), CoordF.From(0, 0, 360)),
                new Cube(new Item(50100097), 1, CoordF.From(-300, -150, 0), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100097), 1, CoordF.From(-300, -300, 0), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100097), 1, CoordF.From(-150, -300, 0), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100340), 1, CoordF.From(-450, 0, 300), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100340), 1, CoordF.From(-450, 0, 450), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100340), 1, CoordF.From(0, 0, 450), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100340), 1, CoordF.From(0, 0, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100340), 1, CoordF.From(0, -450, 300), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100340), 1, CoordF.From(0, -450, 450), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100291), 1, CoordF.From(-450, 0, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100294), 1, CoordF.From(-150, -450, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100294), 1, CoordF.From(0, -450, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100294), 1, CoordF.From(0, -300, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100294), 1, CoordF.From(0, -150, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100294), 1, CoordF.From(0, 0, 150), CoordF.From(0, 0, 180)),
                new Cube(new Item(50100294), 1, CoordF.From(-150, 0, 150), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100294), 1, CoordF.From(-300, 0, 150), CoordF.From(0, 0, 270)),
                new Cube(new Item(50100313), 1, CoordF.From(-150, -300, 150), CoordF.From(0, 0, 0)),
                new Cube(new Item(50100313), 1, CoordF.From(-150, -150, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50100297), 1, CoordF.From(-300, -150, 150), CoordF.From(0, 0, 90)),
                new Cube(new Item(50200583), 1, CoordF.From(-300, 0, 450), CoordF.From(0, 0, 0)),
                new Cube(new Item(50200583), 1, CoordF.From(-150, 0, 450), CoordF.From(0, 0, 0)),
                new Cube(new Item(50200583), 1, CoordF.From(0, -150, 450), CoordF.From(0, 0, -90)),
                new Cube(new Item(50200583), 1, CoordF.From(0, -300, 450), CoordF.From(0, 0, 270)),
                new Cube(new Item(50200584), 1, CoordF.From(-150, -450, 450), CoordF.From(0, 0, -270)),
                new Cube(new Item(50200584), 1, CoordF.From(-150, -450, 300), CoordF.From(0, 0, -270)),
                new Cube(new Item(50200418), 1, CoordF.From(0, -150, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50400069), 1, CoordF.From(-150, 0, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50200641), 1, CoordF.From(-300, 0, 300), CoordF.From(0, 0, 180)),
                new Cube(new Item(50200708), 1, CoordF.From(0, -300, 300), CoordF.From(0, 0, 90))
            };
        }
    }
}
