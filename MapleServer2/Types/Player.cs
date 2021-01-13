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

        private readonly short MAX_LEVEL = 70; // Max Level can't be greater than 99

        // Constant Values
        public long AccountId { get; private set; }
        public long CharacterId { get; private set; }
        public long CreationTime { get; private set; }
        public string Name { get; private set; }
        // Gender - 0 = male, 1 = female
        public byte Gender { get; private set; }

        // Job Group, according to jobgroupname.xml
        public int JobGroupId { get; private set; }
        public bool Awakened { get; private set; }
        public int JobId => JobGroupId * 10 + (Awakened ? 1 : 0);

        // Mutable Values
        public int MapId;
        public short Level { get; private set; }
        public long Experience { get; private set; }
        public long RestExp { get; private set; }
        public int PrestigeLevel { get; private set; }
        public long PrestigeExp { get; private set; }
        public int TitleId;
        public short InsigniaId;
        public byte Animation;
        public PlayerStats Stats;
        public IFieldObject<Mount> Mount;

        // Combat, Adventure, Lifestyle
        public int[] Trophy = new int[3];

        public CoordF Coord;
        public CoordF Rotation; // Rotation?

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

        public Job jobType;
        public JobCode JobCode => jobType != Job.GameMaster ? (JobCode) ((int) jobType / 10) : JobCode.GameMaster;

        public GameOptions GameOptions { get; private set; }

        public Inventory Inventory;
        public Mailbox Mailbox;

        public long PartyId;

        public long ClubId;
        // TODO make this as an array

        public long GuildId;
        public Wallet Wallet { get; private set; }

        public Player()
        {
            Wallet = new Wallet(this);
        }

        public static Player Char1(long accountId, long characterId, string name = "Char1")
        {
            int job = 50; // Archer

            PlayerStats stats = PlayerStats.Default();
            StatDistribution StatPointDistribution = new StatDistribution();
            List<SkillTab> skillTabs = new List<SkillTab>
            {
                XmlParser.ParseSkills(job)
            };

            Player player = new Player
            {
                SkillTabs = skillTabs,
                StatPointDistribution = StatPointDistribution,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Level = 70,
                RestExp = 0,
                PrestigeLevel = 100,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800), // Lith Harbor (2000062)
                // Coord = CoordF.From(500, 500, 15000), // Tria
                JobGroupId = job,
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
                TitleId = 10000292,
                InsigniaId = 29
            };
            player.Equips.Add(ItemSlot.RH, Item.TutorialBow(player));
            return player;
        }

        public static Player Char2(long accountId, long characterId, string name = "Char2")
        {
            int job = 50;
            PlayerStats stats = PlayerStats.Default();

            List<SkillTab> skillTabs = new List<SkillTab>
            {
                XmlParser.ParseSkills(job)
            };

            return new Player
            {
                SkillTabs = skillTabs,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Level = 70,
                Name = name,
                Gender = 0,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800),
                JobGroupId = job,
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
                Mailbox = new Mailbox()
            };
        }

        public static Player NewCharacter(byte gender, int job, string name, SkinColor skinColor, object equips)
        {
            PlayerStats stats = PlayerStats.Default();

            List<SkillTab> skillTabs = new List<SkillTab>
            {
                XmlParser.ParseSkills(job)
            };

            return new Player
            {
                SkillTabs = skillTabs,
                AccountId = AccountStorage.DEFAULT_ACCOUNT_ID,
                CharacterId = GuidGenerator.Long(),
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + AccountStorage.TickCount,
                Name = name,
                Gender = gender,
                JobGroupId = job,
                Level = 1,
                MapId = 52000065,
                Stats = stats,
                SkinColor = skinColor,
                Equips = (Dictionary<ItemSlot, Item>) equips,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(-675, 525, 600), // Intro map (52000065)
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox()
            };
        }

        public void Warp(MapPlayerSpawn spawn, int mapId)
        {
            MapId = mapId;
            Coord = spawn.Coord.ToFloat();
            Rotation = spawn.Rotation.ToFloat();
            Session.Send(FieldPacket.RequestEnter(Session.FieldPlayer));
        }

        public void SetPrestigeLevel(int level)
        {
            PrestigeLevel = level;
            Session.Send(PrestigePacket.SendPrestigeLevelUp(Session.FieldPlayer, PrestigeLevel));
        }

        public void SetLevel(short level)
        {
            Level = level;
            Experience = 0;
            Session.Send(ExperiencePacket.SendExpUp(0, Experience, 0));
            Session.Send(ExperiencePacket.SendLevelUp(Session.FieldPlayer, Level));
        }

        public void GainExp(int amount)
        {
            if (amount <= 0 || Level >= MAX_LEVEL)
            {
                return;
            }

            long newExp = Experience + amount + RestExp;

            if (RestExp > 0)
            {
                RestExp -= amount;

            }
            else
            {
                RestExp = 0;
            }

            while (newExp >= ExpMetadataStorage.GetExpToLevel(Level))
            {
                newExp -= ExpMetadataStorage.GetExpToLevel(Level);
                HandleLevelUp();
                if (Level >= MAX_LEVEL)
                {
                    newExp = 0;
                    break;
                }
            }

            Experience = newExp;
            Session.Send(ExperiencePacket.SendExpUp(amount, newExp, RestExp));
        }

        private void HandleLevelUp()
        {
            Level++;
            // TODO: Gain max HP and heal to max hp
            StatPointDistribution.AddTotalStatPoints(5);
            Session.Send(StatPointPacket.WriteTotalStatPoints(this));
            Session.Send(ExperiencePacket.SendLevelUp(Session.FieldPlayer, Level));
        }

        public void GainPrestigeExp(long amount)
        {
            if (Level < 50) // Can only gain prestige exp after level 50.
            {
                return;
            }
            // Prestige exp can only be earned 1M exp per day. 
            // TODO: After 1M exp, reduce the gain and reset the exp gained every midnight.

            long newPrestigeExp = PrestigeExp + amount;

            if (newPrestigeExp >= 1000000)
            {
                newPrestigeExp -= 1000000;
                PrestigeLevel++;
                Session.Send(PrestigePacket.SendPrestigeLevelUp(Session.FieldPlayer, PrestigeLevel));
            }
            PrestigeExp = newPrestigeExp;
            Session.Send(PrestigePacket.SendPrestigeExpUp(this, amount));
        }
    }
}
