﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class Player
    {
        // Bypass Key is constant PER ACCOUNT, unsure how it is validated
        // Seems like as long as it's valid, it doesn't matter though
        public readonly long UnknownId = 0x01EF80C2; //0x01CC3721;
        public GameSession Session;

        public readonly Account Account;
        // Constant Values
        public long AccountId { get; private set; }
        public long CharacterId { get; set; }
        public long CreationTime { get; private set; }

        public string Name { get; private set; }
        // Gender - 0 = male, 1 = female
        public byte Gender { get; private set; }

        // Job Group, according to jobgroupname.xml
        public bool Awakened { get; private set; }
        public Job Job { get; private set; }
        public JobCode JobCode => (JobCode) ((int) Job * 10 + (Awakened ? 1 : 0));

        // Mutable Values
        public Levels Levels { get; set; }
        public int MapId { get; set; }
        public int TitleId { get; set; }
        public short InsigniaId { get; set; }
        public List<int> Titles { get; set; }

        public byte Animation;
        public PlayerStats Stats;
        public IFieldObject<Mount> Mount;
        public IFieldObject<Pet> Pet;
        public IFieldObject<GuideObject> Guide;

        public long VIPExpiration { get; set; }
        public int SuperChat;

        // Combat, Adventure, Lifestyle
        public int[] TrophyCount;

        public Dictionary<int, Trophy> TrophyData = new Dictionary<int, Trophy>();

        public List<ChatSticker> ChatSticker;
        public List<int> FavoriteStickers;
        public List<int> Emotes;

        public NpcTalk NpcTalk;

        public CoordF Coord;
        public CoordF Rotation;
        public int ReturnMapId;
        public CoordF ReturnCoord;
        public CoordF SafeBlock = CoordF.From(0, 0, 0);
        public bool OnAirMount = false;

        // Appearance
        public SkinColor SkinColor;

        public string ProfileUrl; // profile/e2/5a/2755104031905685000/637207943431921205.png
        public string Motto;

        // TODO: Rework to use class Home
        public int HomeMapId = 62000000;
        public int PlotMapId;
        public int HomePlotNumber;
        public int ApartmentNumber;
        public long HomeExpiration; // if player does not have a purchased plot, home expiration needs to be set to a far away date
        public string HomeName;

        public int MaxSkillTabs { get; set; }
        public long ActiveSkillTabId { get; set; }

        public SkillCast SkillCast = new SkillCast();

        public List<SkillTab> SkillTabs;
        public StatDistribution StatPointDistribution;

        public GameOptions GameOptions { get; private set; }

        public Inventory Inventory;
        public BankInventory BankInventory;
        public DismantleInventory DismantleInventory = new DismantleInventory();
        public LockInventory LockInventory = new LockInventory();
        public HairInventory HairInventory = new HairInventory();

        public Mailbox Mailbox;

        public List<Buddy> BuddyList;

        public long PartyId;
        public long ClubId;
        // TODO make this as an array

        public int[] GroupChatId;

        // TODO: Rework to use Class Guild
        public long GuildId;
        public string GuildName;
        public int GuildContribution;

        public Dictionary<int, Fishing> FishAlbum = new Dictionary<int, Fishing>();
        public Item FishingRod; // Possibly temp solution?

        public Wallet Wallet { get; set; }
        public List<QuestStatus> QuestList;

        private Task HpRegenThread;
        private Task SpRegenThread;
        private Task StaRegenThread;
        private Task OnlineDurationThread;
        private TimeInfo Timestamps;
        public Dictionary<int, PlayerStat> GatheringCount = new Dictionary<int, PlayerStat>();

        class TimeInfo
        {
            public long CharCreation;
            public long OnlineDuration;
            public long LastOnline;

            public TimeInfo(long charCreation = -1, long onlineDuration = 0, long lastOnline = -1)
            {
                CharCreation = charCreation;
                OnlineDuration = onlineDuration;
                LastOnline = lastOnline;
            }
        }

        public Player() { }

        // Initializes all values to be saved into the database
        public Player(long accountId, long characterId, string name, byte gender, Job job)
        {
            AccountId = accountId;
            CharacterId = characterId;
            Name = name;
            Gender = gender;
            Job = job;
            GameOptions = new GameOptions();
            Wallet = new Wallet(this, meso: 0, meret: 0, gameMeret: 0, eventMeret: 0, valorToken: 0, treva: 0, rue: 0,
                                haviFruit: 0, mesoToken: 0, bank: 0);
            Levels = new Levels(this, playerLevel: 1, exp: 0, restExp: 0, prestigeLevel: 1, prestigeExp: 0, new List<MasteryExp>());
            Timestamps = new TimeInfo(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            MapId = 52000065;
            Coord = CoordF.From(-675, 525, 600); // Intro map (52000065)
            Stats = new PlayerStats(strBase: 10, dexBase: 10, intBase: 10, lukBase: 10, hpBase: 500, critRateBase: 10);
            Motto = "Motto";
            ProfileUrl = "";
            HomeName = "HomeName";
            CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount;
            TitleId = 0;
            InsigniaId = 0;
            Titles = new List<int>();
            ChatSticker = new List<ChatSticker>();
            FavoriteStickers = new List<int>();
            Emotes = new List<int>();
            SkillTabs = new List<SkillTab> { new SkillTab(job) };
            StatPointDistribution = new StatDistribution(20);
            Inventory = new Inventory();
            BankInventory = new BankInventory();
            Mailbox = new Mailbox();
            BuddyList = new List<Buddy>();
            QuestList = new List<QuestStatus>();
            GuildName = "";
            TrophyCount = new int[3] { 0, 0, 0 };
            ReturnMapId = (int) Map.Tria;
            ReturnCoord = CoordF.From(-900, -900, 3000);
            GroupChatId = new int[3];
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
                    return Inventory.Equips;
                case InventoryTab.Outfit:
                    return Inventory.Cosmetics;
                default:
                    break;
            }
            return null;
        }

        public Item GetEquippedItem(long itemUid)
        {
            Item gearItem = Inventory.Equips.FirstOrDefault(x => x.Value.Uid == itemUid).Value;
            if (gearItem == null)
            {
                Item cosmeticItem = Inventory.Cosmetics.FirstOrDefault(x => x.Value.Uid == itemUid).Value;
                return cosmeticItem;
            }
            return gearItem;
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

        public void TrophyUpdate(int trophyId, long addAmount, int sendUpdateInterval = 1)
        {
            if (!TrophyData.ContainsKey(trophyId))
            {
                TrophyData[trophyId] = new Trophy(trophyId);
            }
            TrophyData[trophyId].AddCounter(Session, addAmount);
            if (TrophyData[trophyId].Counter % sendUpdateInterval == 0)
            {
                Session.Send(TrophyPacket.WriteUpdate(TrophyData[trophyId]));
            }
        }

        private Task OnlineTimer(PlayerStatId statId)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(60000);
                lock (Timestamps)
                {
                    Timestamps.OnlineDuration += 1;
                    Timestamps.LastOnline = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    TrophyUpdate(23100001, 1);
                }
            });
        }
    }
}
