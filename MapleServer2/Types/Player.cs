using System;
using System.Collections.Generic;
using System.Numerics;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Player
    {
        // Bypass Key is constant PER ACCOUNT, unsure how it is validated
        // Seems like as long as it's valid, it doesn't matter though
        public readonly long UnknownId = 0x01EF80C2; //0x01CC3721;
        public GameSession Session;

        // Constant Values
        public long AccountId { get; private set; }
        public long CharacterId { get; private set; }
        public long CreationTime { get; private set; }
        public string Name { get; private set; }
        // Gender - 0 = male, 1 = female
        public byte Gender { get; private set; }

        // Job Group, according to jobgroupname.xml
        public bool Awakened { get; private set; }
        public Job Job { get; private set; }
        public JobCode JobCode => (JobCode) ((int) Job * 10 + (Awakened ? 1 : 0));

        // Mutable Values
        public Levels Levels { get; private set; }
        public int MapId;
        public int TitleId;
        public List<int> Titles = new List<int> { 0 };
        public List<short> Insignias = new List<short> { 0 };
        public short InsigniaId;
        public byte Animation;
        public PlayerStats Stats;
        public IFieldObject<Mount> Mount;
        public bool IsVIP;

        // Combat, Adventure, Lifestyle
        public int[] Trophy = new int[3];

        public List<short> Stickers = new List<short> { 0 };

        public CoordF Coord;
        public CoordF Rotation;
        public CoordF SafeCoord = CoordF.From(0, 0, 0);
        public bool OnAirMount = false;

        // Appearance
        public SkinColor SkinColor;

        public string GuildName = "";
        public string ProfileUrl = ""; // profile/e2/5a/2755104031905685000/637207943431921205.png
        public string Motto = "";
        public string HomeName = "";

        public Vector3 ReturnPosition;

        public int MaxSkillTabs;
        public long ActiveSkillTabId;

        public int ActiveSkillId = 1;
        public short ActiveSkillLevel = 1;

        public List<SkillTab> SkillTabs = new List<SkillTab>();
        public StatDistribution StatPointDistribution = new StatDistribution();

        public Dictionary<ItemSlot, Item> Equips = new Dictionary<ItemSlot, Item>();
        public Dictionary<ItemSlot, Item> Cosmetics = new Dictionary<ItemSlot, Item>();
        public List<Item> Badges = new List<Item>();
        public ItemSlot[] EquipSlots { get; }
        private ItemSlot DefaultEquipSlot => EquipSlots.Length > 0 ? EquipSlots[0] : ItemSlot.NONE;
        public bool IsBeauty => DefaultEquipSlot == ItemSlot.HR
                        || DefaultEquipSlot == ItemSlot.FA
                        || DefaultEquipSlot == ItemSlot.FD
                        || DefaultEquipSlot == ItemSlot.CL
                        || DefaultEquipSlot == ItemSlot.PA
                        || DefaultEquipSlot == ItemSlot.SH
                        || DefaultEquipSlot == ItemSlot.ER;

        public GameOptions GameOptions { get; private set; }

        public Inventory Inventory;
        public Mailbox Mailbox;

        public long PartyId;

        public long ClubId;
        // TODO make this as an array

        public long GuildId;
        public int GuildContribution;
        public Wallet Wallet { get; private set; }

        public Player()
        {
            Wallet = new Wallet(this);
            Levels = new Levels(this, 70, 0, 0, 100, 0);
        }

        public static Player Char1(long accountId, long characterId, string name = "Char1")
        {
            int job = 50; // Archer
            PlayerStats stats = PlayerStats.Default();
            StatDistribution StatPointDistribution = new StatDistribution(totalStats: 18);
            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab((Job) (job))
            };

            Player player = new Player
            {
                SkillTabs = skillTabs,
                StatPointDistribution = StatPointDistribution,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800), // Lith Harbor (2000062)
                // Coord = CoordF.From(500, 500, 15000), // Tria
                Job = (Job) job,
                SkinColor = new SkinColor()
                {
                    Primary = Color.Argb(0xFF, 0xEA, 0xBF, 0xAE)
                },
                CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount,
                Equips = new Dictionary<ItemSlot, Item> {
                    { ItemSlot.ER, Item.Ear() },
                    { ItemSlot.HR, Item.Hair() },
                    { ItemSlot.FA, Item.Face() },
                    { ItemSlot.FD, Item.FaceDecoration() }
                },
                Stats = stats,
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox(),
                Stickers = new List<short>
                {
                    1, 2, 3, 4, 5, 6, 7
                },
                TitleId = 10000503,
                InsigniaId = 33,
                Titles = new List<int> {
                    10000569, 10000152, 10000570, 10000171, 10000196, 10000195, 10000571, 10000331, 10000190,
                    10000458, 10000465, 10000503, 10000512, 10000513, 10000514, 10000537, 10000565, 10000602,
                    10000603, 10000638, 10000644
                },
                IsVIP = false,
            };
            player.Equips.Add(ItemSlot.RH, Item.TutorialBow(player));
            return player;
        }

        public static Player Char2(long accountId, long characterId, string name = "Char2")
        {
            int job = 50; // Archer
            PlayerStats stats = PlayerStats.Default();

            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab((Job) (job))
            };

            return new Player
            {
                SkillTabs = skillTabs,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Name = name,
                Gender = 0,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800),
                Job = (Job) job,
                SkinColor = new SkinColor()
                {
                    Primary = Color.Argb(0xFF, 0xEA, 0xBF, 0xAE)
                },
                CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount,
                Equips = new Dictionary<ItemSlot, Item> {
                    { ItemSlot.ER, Item.EarMale() },
                    { ItemSlot.HR, Item.HairMale() },
                    { ItemSlot.FA, Item.FaceMale() },
                    { ItemSlot.FD, Item.FaceDecorationMale() },
                    { ItemSlot.CL, Item.CloathMale() },
                    { ItemSlot.SH, Item.ShoesMale() },

                },
                Stats = stats,
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox(),
                IsVIP = false,
            };
        }

        public static Player NewCharacter(byte gender, int job, string name, SkinColor skinColor, object equips)
        {
            PlayerStats stats = PlayerStats.Default();

            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab((Job) (job))
            };

            return new Player
            {
                SkillTabs = skillTabs,
                AccountId = AccountStorage.DEFAULT_ACCOUNT_ID,
                CharacterId = GuidGenerator.Long(),
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + AccountStorage.TickCount,
                Name = name,
                Gender = gender,
                Job = (Job) job,
                MapId = 52000065,
                Stats = stats,
                SkinColor = skinColor,
                Equips = (Dictionary<ItemSlot, Item>) equips,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(-675, 525, 600), // Intro map (52000065)
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox(),
                IsVIP = false,
            };
        }

        public void Warp(MapPlayerSpawn spawn, int mapId)
        {
            MapId = mapId;
            Coord = spawn.Coord.ToFloat();
            Rotation = spawn.Rotation.ToFloat();
            Session.Send(FieldPacket.RequestEnter(Session.FieldPlayer));
        }
    }
}
