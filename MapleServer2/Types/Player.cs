using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
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

        public Account Account;
        // Constant Values
        public long AccountId { get; set; }
        public long CharacterId { get; set; }
        public long CreationTime { get; set; }
        public bool IsDeleted;

        public string Name { get; set; }
        // Gender - 0 = male, 1 = female
        public byte Gender { get; set; }

        public bool Awakened { get; set; }

        // Job Group, according to jobgroupname.xml
        public Job Job { get; set; }
        public JobCode JobCode
        {
            get
            {
                if (Job == Job.GameMaster)
                {
                    return JobCode.GameMaster;
                }
                return (JobCode) ((int) Job * 10 + (Awakened ? 1 : 0));
            }
        }

        // Mutable Values
        public Levels Levels { get; set; }
        public int MapId { get; set; }
        public long InstanceId { get; set; }
        public int TitleId { get; set; }
        public short InsigniaId { get; set; }
        public List<int> Titles { get; set; }
        public List<int> PrestigeRewardsClaimed { get; set; }

        public byte Animation;
        public PlayerStats Stats;
        public IFieldObject<Mount> Mount;
        public IFieldObject<Pet> Pet;
        public IFieldObject<GuideObject> Guide;
        public IFieldObject<Instrument> Instrument;

        public int SuperChat;
        public int ShopId; // current shop player is interacting

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

        public long VisitingHomeId;
        public bool IsInDecorPlanner;

        public Mapleopoly Mapleopoly = new Mapleopoly();

        public int MaxSkillTabs { get; set; }
        public long ActiveSkillTabId { get; set; }

        public SkillCast SkillCast = new SkillCast();

        public List<SkillTab> SkillTabs;
        public StatDistribution StatPointDistribution;

        public GameOptions GameOptions { get; set; }

        public Inventory Inventory;
        public DismantleInventory DismantleInventory = new DismantleInventory();
        public LockInventory LockInventory = new LockInventory();
        public HairInventory HairInventory = new HairInventory();

        public Mailbox Mailbox = new();

        public List<Buddy> BuddyList;

        public Party Party;
        public long ClubId;
        // TODO make this as an array

        public int[] GroupChatId;

        public long GuildId;
        public long GuildMemberId;
        public Guild Guild;
        public GuildMember GuildMember;
        public List<GuildApplication> GuildApplications = new List<GuildApplication>();

        public Dictionary<int, Fishing> FishAlbum = new Dictionary<int, Fishing>();
        public Item FishingRod; // Possibly temp solution?

        public Wallet Wallet { get; set; }
        public List<QuestStatus> QuestList;

        public CancellationTokenSource CombatCTS;
        private Task HpRegenThread;
        private Task SpRegenThread;
        private Task StaRegenThread;
        private readonly TimeInfo Timestamps;
        public Dictionary<int, PlayerStat> GatheringCount = new Dictionary<int, PlayerStat>();

        public List<Status> StatusContainer = new List<Status>();
        public List<int> UnlockedTaxis;
        public List<int> UnlockedMaps;

        public List<string> GmFlags = new List<string>();
        public int DungeonSessionId = -1;

        public List<PlayerTrigger> Triggers = new List<PlayerTrigger>();

        private class TimeInfo
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
        public Player(Account account, string name, byte gender, Job job, SkinColor skinColor)
        {
            AccountId = account.Id;
            Account = account;
            Name = name;
            Gender = gender;
            Job = job;
            GameOptions = new GameOptions();
            Wallet = new Wallet(meso: 0, valorToken: 0, treva: 0, rue: 0, haviFruit: 0);
            Levels = new Levels(playerLevel: 1, exp: 0, restExp: 0, prestigeLevel: 1, prestigeExp: 0, new List<MasteryExp>()
            {
                new MasteryExp(MasteryType.Fishing),
                new MasteryExp(MasteryType.Performance),
                new MasteryExp(MasteryType.Mining),
                new MasteryExp(MasteryType.Foraging),
                new MasteryExp(MasteryType.Ranching),
                new MasteryExp(MasteryType.Farming),
                new MasteryExp(MasteryType.Smithing),
                new MasteryExp(MasteryType.Handicraft),
                new MasteryExp(MasteryType.Alchemy),
                new MasteryExp(MasteryType.Cooking),
                new MasteryExp(MasteryType.PetTaming)
            });
            Timestamps = new TimeInfo(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            MapId = JobMetadataStorage.GetStartMapId((int) job);
            Coord = MapEntityStorage.GetRandomPlayerSpawn(MapId).Coord.ToFloat();
            Stats = new PlayerStats(strBase: 10, dexBase: 10, intBase: 10, lukBase: 10, hpBase: 500, critRateBase: 10);
            Motto = "Motto";
            ProfileUrl = "";
            CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            TitleId = 0;
            InsigniaId = 0;
            Titles = new List<int>();
            PrestigeRewardsClaimed = new List<int>();
            ChatSticker = new List<ChatSticker>();
            FavoriteStickers = new List<int>();
            Emotes = new List<int>() { 90200011, 90200004, 90200024, 90200041, 90200042, 90200057, 90200043, 90200022, 90200031, 90200005, 90200006, 90200003, 90200092, 90200077, 90200073, 90200023, 90200001, 90200019, 90200020, 90200021 };
            StatPointDistribution = new StatDistribution(20);
            Inventory = new Inventory();
            Mailbox = new Mailbox();
            BuddyList = new List<Buddy>();
            QuestList = new List<QuestStatus>();
            TrophyCount = new int[3] { 0, 0, 0 };
            ReturnMapId = (int) Map.Tria;
            ReturnCoord = MapEntityStorage.GetRandomPlayerSpawn(ReturnMapId).Coord.ToFloat();
            GroupChatId = new int[3];
            SkinColor = skinColor;
            UnlockedTaxis = new List<int>();
            UnlockedMaps = new List<int>();
            ActiveSkillTabId = 1;
            CharacterId = DatabaseManager.Characters.Insert(this);
            SkillTabs = new List<SkillTab> { new SkillTab(CharacterId, job, 1, $"Build {(SkillTabs == null ? "1" : SkillTabs.Count + 1)}") };
        }

        public void Warp(int mapId, CoordF coord = default, CoordF rotation = default, long instanceId = 0)
        {
            if (MapEntityStorage.HasSafePortal(MapId))
            {
                ReturnCoord = Coord;
                ReturnMapId = MapId;
            }
            Coord = coord;
            Rotation = rotation;
            SafeBlock = coord;
            MapId = mapId;
            InstanceId = instanceId;

            if (coord == default || rotation == default)
            {
                MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);
                if (spawn == null)
                {
                    Session.SendNotice($"Could not find a spawn for map {mapId}");
                    return;
                }
                if (coord == default)
                {
                    Coord = spawn.Coord.ToFloat();
                    SafeBlock = spawn.Coord.ToFloat();
                }
                if (rotation == default)
                {
                    Rotation = spawn.Rotation.ToFloat();
                }
            }

            if (!UnlockedMaps.Contains(MapId))
            {
                UnlockedMaps.Add(MapId);
            }

            DatabaseManager.Characters.Update(this);
            Session.Send(FieldPacket.RequestEnter(this));
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

        public void Cast(SkillCast skillCast)
        {
            int spiritCost = skillCast.GetSpCost();
            int staminaCost = skillCast.GetStaCost();

            if (Stats[PlayerStatId.Spirit].Current >= spiritCost && Stats[PlayerStatId.Stamina].Current >= staminaCost)
            {
                ConsumeSp(spiritCost);
                ConsumeStamina(staminaCost);
                SkillCast = skillCast;
                Session.SendNotice(skillCast.SkillId.ToString());

                // TODO: Move this and all others combat cases like recover sp to its own class.
                // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
                if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
                {
                    Status status = new Status(skillCast, Session.FieldPlayer.ObjectId, Session.FieldPlayer.ObjectId, 1);
                    StatusHandler.Handle(Session, status);
                }

                // Refresh out-of-combat timer
                if (CombatCTS != null)
                {
                    CombatCTS.Cancel();
                }
                CombatCTS = new CancellationTokenSource();
                CombatCTS.Token.Register(() => CombatCTS.Dispose());
                StartCombatStance(CombatCTS);
            }
        }

        private Task StartCombatStance(CancellationTokenSource ct)
        {
            Session.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(Session.FieldPlayer, true));
            return Task.Run(async () =>
            {
                await Task.Delay(5000);

                if (!ct.Token.IsCancellationRequested)
                {
                    CombatCTS = null;
                    ct.Dispose();
                    Session?.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(Session.FieldPlayer, false));
                }
            }, ct.Token);
        }

        public Task TimeSyncLoop()
        {
            return Task.Run(async () =>
            {
                while (Session != null)
                {
                    Session.Send(TimeSyncPacket.Request());
                    await Task.Delay(1000);
                }
            });
        }

        public Task ClientTickSyncLoop()
        {
            return Task.Run(async () =>
            {
                while (Session != null)
                {
                    Session.Send(RequestPacket.TickSync());
                    await Task.Delay(300 * 1000); // every 5 minutes
                }
            });
        }

        public void RecoverHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Hp];
                if (stat.Current < stat.Max)
                {
                    Stats.Increase(PlayerStatId.Hp, Math.Min(amount, stat.Max - stat.Current));
                    Session.Send(StatPacket.UpdateStats(Session.FieldPlayer, PlayerStatId.Hp));
                }
            }
        }

        public void ConsumeHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Hp];
                Stats.Decrease(PlayerStatId.Hp, Math.Min(amount, stat.Current));
            }

            if (HpRegenThread == null || HpRegenThread.IsCompleted)
            {
                HpRegenThread = StartRegen(PlayerStatId.Hp, PlayerStatId.HpRegen, PlayerStatId.HpRegenTime);
            }
        }

        public void RecoverSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Spirit];
                if (stat.Current < stat.Max)
                {
                    Stats.Increase(PlayerStatId.Spirit, Math.Min(amount, stat.Max - stat.Current));
                    Session.Send(StatPacket.UpdateStats(Session.FieldPlayer, PlayerStatId.Spirit));
                }
            }
        }

        public void ConsumeSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Spirit];
                Stats.Decrease(PlayerStatId.Spirit, Math.Min(amount, stat.Current));
            }

            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(PlayerStatId.Spirit, PlayerStatId.SpRegen, PlayerStatId.SpRegenTime);
            }
        }

        public void RecoverStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Stamina];
                if (stat.Current < stat.Max)
                {
                    Stats.Increase(PlayerStatId.Stamina, Math.Min(amount, stat.Max - stat.Current));
                    Session.Send(StatPacket.UpdateStats(Session.FieldPlayer, PlayerStatId.Stamina));
                }
            }
        }

        public void ConsumeStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                PlayerStat stat = Stats[PlayerStatId.Stamina];
                Stats.Decrease(PlayerStatId.Stamina, Math.Min(amount, stat.Current));
            }

            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(PlayerStatId.Stamina, PlayerStatId.StaRegen, PlayerStatId.StaRegenTime);
            }
        }

        private Task StartRegen(PlayerStatId statId, PlayerStatId regenStatId, PlayerStatId timeStatId)
        {
            // TODO: merge regen updates with larger packets
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(Stats[timeStatId].Current);

                    lock (Stats)
                    {
                        if (Stats[statId].Current >= Stats[statId].Max)
                        {
                            return;
                        }

                        // TODO: Check if regen-enabled
                        Stats[statId] = AddStatRegen(statId, regenStatId);
                        Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(Session.FieldPlayer, statId));
                        if (Party != null)
                        {
                            Party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(this));
                        }
                    }
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

        public void TrophyUpdate(int trophyId, long addAmount, int sendUpdateInterval = 1)
        {
            if (!TrophyData.ContainsKey(trophyId))
            {
                TrophyData[trophyId] = new Trophy(CharacterId, AccountId, trophyId);
            }
            TrophyData[trophyId].AddCounter(Session, addAmount);
            if (TrophyData[trophyId].Counter % sendUpdateInterval == 0)
            {
                DatabaseManager.Trophies.Update(TrophyData[trophyId]);
                Session.Send(TrophyPacket.WriteUpdate(TrophyData[trophyId]));
            }
        }

        private Task OnlineTimer()
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

        public void AddStatPoint(int amount, OtherStatsIndex index)
        {
            StatPointDistribution.AddTotalStatPoints(amount, index);
            Session.Send(StatPointPacket.WriteTotalStatPoints(this));
        }
    }
}
