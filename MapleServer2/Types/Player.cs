using System;
using System.Collections.Generic;
using System.Numerics;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data;
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
        public int JobGroupId { get; private set; }
        public bool Awakened { get; private set; }
        public int JobId => JobGroupId * 10 + (Awakened ? 1 : 0);

        // Mutable Values
        public int MapId;
        public short Level = 1;
        public long Experience;
        public long RestExperience;
        public int PrestigeLevel = 100;
        public long PrestigeExperience;
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

        public Currency Currency;

        public static Player Char1(long accountId, long characterId, string name = "Char1")
        {
            int job = 50; // Archer

            PlayerStats stats = PlayerStats.Default();
            StatDistribution StatPointDistribution = new StatDistribution();
            Currency currency = new Currency();

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
                Currency = currency,
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
            Currency currency = new Currency();

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
                Currency = currency,
                Stats = stats,
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox()
            };
        }

        public static Player NewCharacter(byte gender, int job, string name, SkinColor skinColor, object equips)
        {
            PlayerStats stats = PlayerStats.Default();
            Currency currency = new Currency();

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
                Currency = currency,
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

        public bool ModifyCurrency(CurrencyType type, long amount)
        {
            switch (type)
            {
                case CurrencyType.Meso:
                    if (amount + Currency.Meso < 0)
                    {
                        return false;
                    }
                    Currency.Meso += amount;
                    UpdateMesos();
                    break;
                case CurrencyType.Meret:
                    if (amount + Currency.Meret < 0)
                    {
                        return false;
                    }
                    Currency.Meret += amount;
                    UpdateMerets();
                    break;
                case CurrencyType.ValorToken:
                    if (amount + Currency.ValorToken < 0)
                    {
                        return false;
                    }
                    Currency.ValorToken += amount;
                    break;
                case CurrencyType.Treva:
                    if (amount + Currency.Treva < 0)
                    {
                        return false;
                    }
                    Currency.Treva += amount;
                    break;
                case CurrencyType.Rue:
                    if (amount + Currency.Rue < 0)
                    {
                        return false;
                    }
                    Currency.Rue += amount;
                    break;
                case CurrencyType.HaviFruit:
                    if (amount + Currency.HaviFruit < 0)
                    {
                        return false;
                    }
                    Currency.HaviFruit += amount;
                    break;
                case CurrencyType.MesoToken:
                    if (amount + Currency.MesoToken < 0)
                    {
                        return false;
                    }
                    Currency.MesoToken += amount;
                    break;
                default:
                    break;
            }
            return true;
        }

        public void SetCurrency(CurrencyType type, long amount)
        {
            if (amount < 0)
            {
                return;
            }

            switch (type)
            {
                case CurrencyType.Meso:
                    Currency.Meso = amount;
                    UpdateMesos();
                    break;
                case CurrencyType.Meret:
                    Currency.Meret = amount;
                    UpdateMerets();
                    break;
                case CurrencyType.ValorToken:
                    Currency.ValorToken = amount;
                    break;
                case CurrencyType.Treva:
                    Currency.Treva = amount;
                    break;
                case CurrencyType.Rue:
                    Currency.Rue = amount;
                    break;
                case CurrencyType.HaviFruit:
                    Currency.HaviFruit = amount;
                    break;
                case CurrencyType.MesoToken:
                    Currency.MesoToken = amount;
                    break;
                default:
                    break;
            }
        }
        public long GetCurrency(CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.Meso:
                    return Currency.Meso;
                case CurrencyType.Meret:
                    return Currency.Meret;
                case CurrencyType.ValorToken:
                    return Currency.ValorToken;
                case CurrencyType.Treva:
                    return Currency.Treva;
                case CurrencyType.Rue:
                    return Currency.Rue;
                case CurrencyType.HaviFruit:
                    return Currency.HaviFruit;
                case CurrencyType.MesoToken:
                    return Currency.MesoToken;
                default:
                    return -1;
            }
        }

        private void UpdateMesos()
        {
            Session.Send(MesosPacket.UpdateMesos(Session));
        }

        private void UpdateMerets()
        {
            Session.Send(MeretsPacket.UpdateMerets(Session));
        }
    }
}
