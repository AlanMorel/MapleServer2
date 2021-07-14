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
            Size = 10;
            Height = 5;
            Password = "******";
            Permissions = new Dictionary<HomePermission, byte>();
            // GenerateTemplate(this, homeTemplate);

            Id = DatabaseManager.CreateHouse(this);
        }

        private static void GenerateTemplate(Home home, int homeTemplate)
        {
            if (homeTemplate == 0)
            {
                for (int x = 0; x < home.Size; x++)
                {
                    float coordX = -1 * (x * Block.BLOCK_SIZE);
                    for (int y = 0; y < home.Size; y++)
                    {
                        float coordY = -1 * (y * Block.BLOCK_SIZE);
                        Cube defaultCube = new Cube(new Item(), 1, CoordF.From(coordX, coordY, 0), CoordF.From(0, 0, 0));
                        home.FurnishingInventory[defaultCube.Uid] = defaultCube;
                    }
                }
            }
        }
    }
}
