﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
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
        public IFieldObject<Pet> Pet;
        public long VIPExpiration = 0;

        // Combat, Adventure, Lifestyle
        public int[] Trophy = new int[3] { 0, 1, 2 };
        public Dictionary<int, Achieve> Achieves = new Dictionary<int, Achieve>();

        public List<ChatSticker> ChatSticker = new List<ChatSticker>() { };
        public List<int> FavoriteStickers = new List<int> { };
        public List<int> Emotes = new List<int> { 0 };

        public CoordF Coord;
        public CoordF Rotation;
        public CoordF SafeBlock = CoordF.From(0, 0, 0);
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
        public SkillCast SkillCast = new SkillCast();

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

        public Inventory Inventory = new Inventory();
        public BankInventory BankInventory = new BankInventory();
        public DismantleInventory DismantleInventory = new DismantleInventory();
        public LockInventory LockInventory = new LockInventory();

        public Mailbox Mailbox = new Mailbox();

        public long PartyId;

        public long ClubId;
        // TODO make this as an array

        public int[] GroupChatId = new int[3];

        public long GuildId;
        public int GuildContribution;
        public Wallet Wallet { get; private set; }
        public List<QuestStatus> QuestList = new List<QuestStatus>();

        private Task HpRegenThread;
        private Task SpRegenThread;
        private Task StaRegenThread;

        public Dictionary<int, PlayerStat> GatheringCount = new Dictionary<int, PlayerStat>();

        public Player()
        {
            GameOptions = new GameOptions();
            Wallet = new Wallet(this);
            Levels = new Levels(this, 70, 0, 0, 100, 0, new List<MasteryExp>());
        }

        public static Player Char1(long accountId, long characterId, string name = "Char1")
        {
            Job job = Job.Archer;
            PlayerStats stats = new PlayerStats();
            StatDistribution statPointDistribution = new StatDistribution(totalStats: 18);
            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab(job)
            };

            Player player = new Player
            {
                SkillTabs = skillTabs,
                StatPointDistribution = statPointDistribution,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800), // Lith Harbor (2000062)
                // Coord = CoordF.From(500, 500, 15000), // Tria
                Job = job,
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
                Emotes = new List<int>
                {
                    90200011, 90200004, 90200024, 90200041, 90200042,
                90200057, 90200043, 90200022, 90200031, 90200005,
                90200006, 90200003, 90200092, 90200077, 90200073,
                90200023, 90200001, 90200019, 90200020, 90200021,
                90200009, 90200027, 90200010, 90200028, 90200051,
                90200015, 90200016, 90200055, 90200060, 90200017,
                90200018, 90200093, 90220033, 90220012, 90220001, 90220033
                },
                TitleId = 10000503,
                InsigniaId = 33,
                Titles = new List<int> {
                    10000569, 10000152, 10000570, 10000171, 10000196, 10000195, 10000571, 10000331, 10000190,
                    10000458, 10000465, 10000503, 10000512, 10000513, 10000514, 10000537, 10000565, 10000602,
                    10000603, 10000638, 10000644
                }
            };
            player.Equips.Add(ItemSlot.RH, Item.TutorialBow(player));
            return player;
        }

        public static Player Char2(long accountId, long characterId, string name = "Char2")
        {
            Job job = Job.Archer;
            PlayerStats stats = new PlayerStats();

            int mapId = (int) Map.Queenstown;
            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);
            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab(job)
            };

            return new Player
            {
                SkillTabs = skillTabs,
                MapId = mapId,
                AccountId = accountId,
                CharacterId = characterId,
                Name = name,
                Gender = 0,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(spawn.Coord.X, spawn.Coord.Y, spawn.Coord.Z),
                Job = job,
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
                Stats = stats
            };
        }

        public static Player NewCharacter(byte gender, Job job, string name, SkinColor skinColor, object equips)
        {
            PlayerStats stats = new PlayerStats();

            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab(job)
            };

            return new Player
            {
                SkillTabs = skillTabs,
                AccountId = AccountStorage.DEFAULT_ACCOUNT_ID,
                CharacterId = GuidGenerator.Long(),
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + AccountStorage.TickCount,
                Name = name,
                Gender = gender,
                Job = job,
                MapId = 52000065,
                Stats = stats,
                SkinColor = skinColor,
                Equips = (Dictionary<ItemSlot, Item>) equips,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(-675, 525, 600) // Intro map (52000065)
            };
        }

        public static Player Priest(long accountId, long characterId, string name = "Priest")
        {
            Job job = Job.Priest;
            PlayerStats stats = new PlayerStats();
            StatDistribution statPointDistribution = new StatDistribution(totalStats: 18);
            List<SkillTab> skillTabs = new List<SkillTab>
            {
                new SkillTab(job)
            };

            Player player = new Player
            {
                SkillTabs = skillTabs,
                StatPointDistribution = statPointDistribution,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800), // Lith Harbor (2000062)
                // Coord = CoordF.From(500, 500, 15000), // Tria
                Job = job,
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
                Mailbox = new Mailbox(),
                TitleId = 10000503,
                InsigniaId = 33,
                Titles = new List<int> {
                    10000569, 10000152, 10000570, 10000171, 10000196, 10000195, 10000571, 10000331, 10000190,
                    10000458, 10000465, 10000503, 10000512, 10000513, 10000514, 10000537, 10000565, 10000602,
                    10000603, 10000638, 10000644
                },
            };
            player.Equips.Add(ItemSlot.RH, Item.DefaultScepter(player));
            player.Equips.Add(ItemSlot.LH, Item.DefaultCodex(player));
            return player;
        }

        public void Warp(MapPlayerSpawn spawn, int mapId)
        {
            MapId = mapId;
            Coord = spawn.Coord.ToFloat();
            Rotation = spawn.Rotation.ToFloat();
            Session.Send(FieldPacket.RequestEnter(Session.FieldPlayer));
        }

        public Dictionary<ItemSlot, Item> GetEquippedInventory(InventoryTab tab)
        {
            switch (tab)
            {
                case InventoryTab.Gear:
                    return Equips;
                case InventoryTab.Outfit:
                    return Cosmetics;
                default:
                    break;
            }
            return null;
        }

        public void ConsumeHp(int amount)
        {
            Stats.Decrease(PlayerStatId.Hp, amount);

            // TODO: merge regen updates with larger packets
            if (HpRegenThread == null || HpRegenThread.IsCompleted)
            {
                HpRegenThread = StartRegen(PlayerStatId.Hp, PlayerStatId.HpRegen, PlayerStatId.HpRegenTime);
            }
        }

        public void ConsumeSp(int amount)
        {
            Stats.Decrease(PlayerStatId.Spirit, amount);

            // TODO: merge regen updates with larger packets
            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(PlayerStatId.Spirit, PlayerStatId.SpRegen, PlayerStatId.SpRegenTime);
            }
        }

        public void ConsumeStamina(int amount)
        {
            Stats.Decrease(PlayerStatId.Stamina, amount);

            // TODO: merge regen updates with larger packets
            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(PlayerStatId.Stamina, PlayerStatId.StaRegen, PlayerStatId.StaRegenTime);
            }
        }

        private Task StartRegen(PlayerStatId statId, PlayerStatId regenStatId, PlayerStatId timeStatId)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(Stats[timeStatId].Current);

                // TODO: Check if regen-enabled
                while (Stats[statId].Current < Stats[statId].Max)
                {
                    lock (Stats)
                    {
                        Stats[statId] = AddStatRegen(statId, regenStatId);
                        Session.Send(StatPacket.UpdateStats(Session.FieldPlayer, statId));
                    }
                    await Task.Delay(Stats[timeStatId].Current);
                }
            });
        }

        private PlayerStat AddStatRegen(PlayerStatId statIndex, PlayerStatId regenStatIndex)
        {
            PlayerStat stat = Stats[statIndex];
            int regen = Stats[regenStatIndex].Current;
            int postRegen = Math.Clamp(stat.Current + regen, 0, stat.Max);
            return new PlayerStat(stat.Max, stat.Min, postRegen);
        }

        public void IncrementGatheringCount(int recipeID, int amount)
        {
            if (!GatheringCount.ContainsKey(recipeID))
            {
                int maxLimit = (int) (RecipeMetadataStorage.GetRecipe(recipeID).NormalPropLimitCount * 1.4);
                GatheringCount[recipeID] = new PlayerStat(maxLimit, 0, 0);
            }
            if ((GatheringCount[recipeID].Current + amount) <= GatheringCount[recipeID].Max)
            {
                PlayerStat stat = GatheringCount[recipeID];
                stat.Current += amount;
                GatheringCount[recipeID] = stat;
            }
        }

        public bool IsVip()
        {
            return VIPExpiration > DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
