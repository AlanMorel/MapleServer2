using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database.Classes;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    // TODO: Implement architect expiration
    public class Home
    {
        public long Id;
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
        public Dictionary<long, Item> WarehouseInventory = new Dictionary<long, Item>();
        public Dictionary<long, Cube> FurnishingInventory = new Dictionary<long, Cube>();

        // Decor planner
        public byte DecorPlannerSize;
        public byte DecorPlannerHeight;
        public Dictionary<long, Cube> DecorPlannerInventory = new Dictionary<long, Cube>();

        // Budget wallet
        public long Mesos;
        public long Merets;

        public Home() { }

        public Home(long accountId, string houseName, int homeTemplate)
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

            // the Templates ids in the XMLs are from 1-3 and the client request 0-2
            HomeTemplateMetadata templateMetadata = HomeTemplateMetadataStorage.GetTemplate((homeTemplate + 1).ToString());
            if (templateMetadata == null)
            {
                Size = 10;
                Height = 4;
            }
            else
            {
                Size = templateMetadata.Size;
                Height = templateMetadata.Height;
            }

            Id = DatabaseHome.CreateHome(this);
            foreach (CubeTemplate cubeTemplate in templateMetadata.Cubes)
            {
                Cube cube = new Cube(new Item(cubeTemplate.ItemId), 1, cubeTemplate.CoordF, cubeTemplate.Rotation, 0, Id);
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

        public Item AddWarehouseItem(GameSession session, int itemId, int amount, Item item = default)
        {
            Item furnishingItem = WarehouseInventory.Values.FirstOrDefault(x => x.Id == itemId);
            if (furnishingItem == default)
            {
                furnishingItem = item == default ? new Item(itemId, amount) : item;
                WarehouseInventory[furnishingItem.Uid] = furnishingItem;
                session.Send(WarehouseInventoryPacket.Load(furnishingItem, WarehouseInventory.Values.Count));
                session.Send(WarehouseInventoryPacket.Count(WarehouseInventory.Values.Count + 1));
            }
            else
            {
                WarehouseInventory[furnishingItem.Uid].Amount += amount;
                session.Send(WarehouseInventoryPacket.UpdateAmount(furnishingItem.Uid, furnishingItem.Amount));
            }
            return furnishingItem;
        }
    }
}
