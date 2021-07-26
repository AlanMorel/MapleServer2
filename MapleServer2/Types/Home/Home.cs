using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    // TODO: Implement architect expiration
    public class Home
    {
        public readonly long Id;
        public long InstanceId;
        public long AccountId { get; set; }
        public int MapId { get; set; }
        public int PlotMapId { get; set; }
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
        public long DecorationRewardTimestamp { get; set; }
        public List<int> InteriorRewardsClaimed { get; set; }
        public List<HomeLayout> Layouts;

        private readonly long[] DecorationExpTable = new long[] { 0, 100, 300, 1000, 2100, 5500, 7700, 9900, 13200, 16500 };

        // Interior Settings
        public byte Lighting { get; set; }
        public byte Background { get; set; }
        public byte Camera { get; set; }

        // Permissions
        public const byte PERMISSION_COUNT = 9;
        public bool IsPrivate => !Password.Equals("******");
        public string Password { get; set; }
        public Dictionary<HomePermission, byte> Permissions { get; set; }
        public List<long> BuildingPermissions = new List<long>(); // account ids

        // Inventories
        public readonly Dictionary<long, Item> WarehouseInventory = new Dictionary<long, Item>();
        public List<Item> WarehouseItems { get; set; } // DB ONLY
        public readonly Dictionary<long, Cube> FurnishingInventory = new Dictionary<long, Cube>();
        public List<Cube> FurnishingCubes { get; set; } // DB ONLY

        // Decor planner
        public byte DecorPlannerSize;
        public byte DecorPlannerHeight;
        public Dictionary<long, Cube> DecorPlannerInventory = new Dictionary<long, Cube>();

        // Budget wallet
        public long Mesos;
        public long Merets;

        public Home() { }

        public Home(long accountId, string houseName, string homeTemplate)
        {
            AccountId = accountId;
            Name = houseName;
            Description = "Thanks for visiting. Come back soon!";
            MapId = (int) Map.PrivateResidence;
            DecorationLevel = 1;
            InteriorRewardsClaimed = new List<int>();
            Expiration = 32503561200; // Year 2999
            Password = "******";
            Permissions = new Dictionary<HomePermission, byte>();
            Layouts = new List<HomeLayout>();

            SetTemplate(homeTemplate);

            Id = DatabaseManager.CreateHouse(this);
        }

        private void SetTemplate(string homeTemplate)
        {
            HomeTemplateMetadata templateMetadata = HomeTemplateMetadataStorage.GetTemplate(homeTemplate.ToString());
            if (templateMetadata == null)
            {
                Size = 10;
                Height = 4;
                return;
            }

            Size = templateMetadata.Size;
            Height = templateMetadata.Height;
            foreach (CubeTemplate cubeTemplate in templateMetadata.Cubes)
            {
                Cube cube = new Cube(new Item(cubeTemplate.ItemId), 1, cubeTemplate.CoordF, cubeTemplate.Rotation);
                FurnishingInventory.Add(cube.Uid, cube);
            }
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
}
