﻿using System;
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
        private Currency Meso;
        private Currency Meret;
        private Currency ValorToken;
        private Currency Treva;
        private Currency Rue;
        private Currency HaviFruit;
        private Currency MesoToken;

        public static Player Char1(long accountId, long characterId, string name = "Char1")
        {
            int job = 50; // Archer

            PlayerStats stats = PlayerStats.Default();
            StatDistribution StatPointDistribution = new StatDistribution();

            Currency meso = new Currency(20000);
            Currency meret = new Currency(2000);
            Currency valorToken = new Currency(5);
            Currency treva = new Currency(5);
            Currency rue = new Currency(5);
            Currency haviFruit = new Currency(5);
            Currency mesoToken = new Currency(5);

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
                Meso = meso,
                Meret = meret,
                ValorToken = valorToken,
                Treva = treva,
                Rue = rue,
                HaviFruit = haviFruit,
                MesoToken = mesoToken,
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

            Currency meso = new Currency(20000);
            Currency meret = new Currency(2000);
            Currency valorToken = new Currency(5);
            Currency treva = new Currency(5);
            Currency rue = new Currency(5);
            Currency haviFruit = new Currency(5);
            Currency mesoToken = new Currency(5);

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
                Meso = meso,
                Meret = meret,
                ValorToken = valorToken,
                Treva = treva,
                Rue = rue,
                HaviFruit = haviFruit,
                MesoToken = mesoToken,
                Stats = stats,
                GameOptions = new GameOptions(),
                Inventory = new Inventory(48),
                Mailbox = new Mailbox()
            };
        }

        public static Player NewCharacter(byte gender, int job, string name, SkinColor skinColor, object equips)
        {
            PlayerStats stats = PlayerStats.Default();

            Currency meso = new Currency(20000);
            Currency meret = new Currency(2000);
            Currency valorToken = new Currency(5);
            Currency treva = new Currency(5);
            Currency rue = new Currency(5);
            Currency haviFruit = new Currency(5);
            Currency mesoToken = new Currency(5);

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
                Meso = meso,
                Meret = meret,
                ValorToken = valorToken,
                Treva = treva,
                Rue = rue,
                HaviFruit = haviFruit,
                MesoToken = mesoToken,
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


        public bool ModifyCurrency(CurrencyType type, long amount)
        {
            switch (type)
            {
                case CurrencyType.Meso:
                    if (Meso.Modify(amount))
                    {
                        UpdateMesos();
                        return true;
                    }
                    return false;
                case CurrencyType.Meret:
                    if (Meret.Modify(amount))
                    {
                        UpdateMerets();
                        return true;
                    }
                    return false;
                case CurrencyType.ValorToken:
                    if (ValorToken.Modify(amount))
                    {
                        // missing packet to update UI.
                        return true;
                    }
                    return false;
                case CurrencyType.Treva:
                    if (Treva.Modify(amount))
                    {
                        // missing packet to update UI.
                        return true;
                    }
                    return false;
                case CurrencyType.Rue:
                    if (Rue.Modify(amount))
                    {
                        // missing packet to update UI.
                        return true;
                    }
                    return false;
                case CurrencyType.HaviFruit:
                    if (HaviFruit.Modify(amount))
                    {
                        // missing packet to update UI.
                        return true;
                    }
                    return false;
                case CurrencyType.MesoToken:
                    if (MesoToken.Modify(amount))
                    {
                        // missing packet to update UI.
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public long GetCurrency(CurrencyType type)
        {
            return type switch
            {
                CurrencyType.Meso => Meso.GetAmount(),
                CurrencyType.Meret => Meret.GetAmount(),
                CurrencyType.ValorToken => ValorToken.GetAmount(),
                CurrencyType.Treva => Treva.GetAmount(),
                CurrencyType.Rue => Rue.GetAmount(),
                CurrencyType.HaviFruit => HaviFruit.GetAmount(),
                CurrencyType.MesoToken => MesoToken.GetAmount(),
                _ => -1,
            };
        }

        public void SetCurrency(CurrencyType type, long amount)
        {
            switch (type)
            {
                case CurrencyType.Meso:
                    Meso.SetAmount(amount);
                    UpdateMesos();
                    break;
                case CurrencyType.Meret:
                    Meret.SetAmount(amount);
                    UpdateMerets();
                    break;
                case CurrencyType.ValorToken:
                    ValorToken.SetAmount(amount);
                    // missing packet to update UI.
                    break;
                case CurrencyType.Treva:
                    Treva.SetAmount(amount);
                    // missing packet to update UI.
                    break;
                case CurrencyType.Rue:
                    Rue.SetAmount(amount);
                    // missing packet to update UI.
                    break;
                case CurrencyType.HaviFruit:
                    HaviFruit.SetAmount(amount);
                    // missing packet to update UI.
                    break;
                case CurrencyType.MesoToken:
                    MesoToken.SetAmount(amount);
                    // missing packet to update UI.
                    break;
                default:
                    break;
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
