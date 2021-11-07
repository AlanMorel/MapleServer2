using System.Net;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

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

    public short ChannelId;
    public bool IsChangingChannel;

    // Combat, Adventure, Lifestyle
    public int[] TrophyCount;

    public Dictionary<int, Trophy> TrophyData = new();

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

    public string ProfileUrl;
    public string Motto;

    public long VisitingHomeId;
    public bool IsInDecorPlanner;

    public Mapleopoly Mapleopoly = new();

    public int MaxSkillTabs { get; set; }
    public long ActiveSkillTabId { get; set; }

    public SkillCast SkillCast = new();

    public List<SkillTab> SkillTabs;
    public StatDistribution StatPointDistribution;

    public GameOptions GameOptions { get; set; }

    public Inventory Inventory;
    public DismantleInventory DismantleInventory = new();
    public LockInventory LockInventory = new();
    public HairInventory HairInventory = new();

    public List<Mail> Mailbox = new();

    public List<Buddy> BuddyList;

    public Party Party;
    public long ClubId;
    // TODO make this as an array

    public int[] GroupChatId;

    public long GuildId;
    public long GuildMemberId;
    public Guild Guild;
    public GuildMember GuildMember;
    public List<GuildApplication> GuildApplications = new();

    public Dictionary<int, Fishing> FishAlbum = new();
    public Item FishingRod; // Possibly temp solution?

    public Wallet Wallet { get; set; }
    public List<QuestStatus> QuestList;

    public CancellationTokenSource CombatCTS;
    private Task HpRegenThread;
    private Task SpRegenThread;
    private Task StaRegenThread;
    private readonly TimeInfo Timestamps;

    public List<GatheringCount> GatheringCount;

    public List<Status> StatusContainer = new();
    public List<int> UnlockedTaxis;
    public List<int> UnlockedMaps;

    public List<string> GmFlags = new();
    public int DungeonSessionId = -1;

    public List<PlayerTrigger> Triggers = new();

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
        GameOptions = new();
        Wallet = new(0, 0, 0, 0, 0);
        Levels = new(1, 0, 0, 1, 0, new()
        {
            new(MasteryType.Fishing),
            new(MasteryType.Performance),
            new(MasteryType.Mining),
            new(MasteryType.Foraging),
            new(MasteryType.Ranching),
            new(MasteryType.Farming),
            new(MasteryType.Smithing),
            new(MasteryType.Handicraft),
            new(MasteryType.Alchemy),
            new(MasteryType.Cooking),
            new(MasteryType.PetTaming)
        });
        Timestamps = new(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        MapId = JobMetadataStorage.GetStartMapId((int) job);
        Coord = MapEntityStorage.GetRandomPlayerSpawn(MapId).Coord.ToFloat();
        Stats = new(10, 10, 10, 10, 500, 10);
        Motto = "Motto";
        ProfileUrl = "";
        CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        TitleId = 0;
        InsigniaId = 0;
        Titles = new();
        PrestigeRewardsClaimed = new();
        ChatSticker = new();
        FavoriteStickers = new();
        Emotes = new()
        {
            90200011,
            90200004,
            90200024,
            90200041,
            90200042,
            90200057,
            90200043,
            90200022,
            90200031,
            90200005,
            90200006,
            90200003,
            90200092,
            90200077,
            90200073,
            90200023,
            90200001,
            90200019,
            90200020,
            90200021
        };
        StatPointDistribution = new(20);
        Inventory = new(true);
        Mailbox = new();
        BuddyList = new();
        QuestList = new();
        GatheringCount = new();
        TrophyCount = new int[3]
        {
            0, 0, 0
        };
        ReturnMapId = (int) Map.Tria;
        ReturnCoord = MapEntityStorage.GetRandomPlayerSpawn(ReturnMapId).Coord.ToFloat();
        GroupChatId = new int[3];
        SkinColor = skinColor;
        UnlockedTaxis = new();
        UnlockedMaps = new();
        ActiveSkillTabId = 1;
        CharacterId = DatabaseManager.Characters.Insert(this);
        SkillTabs = new()
        {
            new(CharacterId, job, 1, $"Build {(SkillTabs == null ? "1" : SkillTabs.Count + 1)}")
        };
    }

    public void UpdateBuddies()
    {
        BuddyList.ForEach(buddy =>
        {
            if (buddy.Friend?.Session?.Connected() ?? false)
            {
                Buddy myBuddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddy.SharedId);
                buddy.Friend.Session.Send(BuddyPacket.LoginLogoutNotification(myBuddy));
                buddy.Friend.Session.Send(BuddyPacket.UpdateBuddy(myBuddy));
            }
        });
    }

    public void Warp(int mapId, CoordF? coord = null, CoordF? rotation = null, long instanceId = 1)
    {
        UpdateCoords(mapId, instanceId, coord, rotation);

        SetCoords(mapId, coord, rotation);

        DatabaseManager.Characters.Update(this);
        Session.Send(FieldPacket.RequestEnter(this));
    }

    public void WarpGameToGame(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        UpdateCoords(mapId, instanceId, coord, rotation);
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("GAME_PORT"));
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

        AuthData authTokens = AuthStorage.GetData(AccountId);
        authTokens.Player.IsChangingChannel = true;

        DatabaseManager.Characters.Update(this);
        Session.Send(MigrationPacket.GameToGame(endpoint, authTokens, this));
    }

    public void SetCoords(int mapId, CoordF? coord, CoordF? rotation)
    {
        if (coord is not null && rotation is not null)
        {
            return;
        }

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

    private void UpdateCoords(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        if (MapEntityStorage.HasSafePortal(MapId))
        {
            ReturnCoord = Coord;
            ReturnMapId = MapId;
        }
        if (coord is not null && rotation is not null)
        {
            Coord = (CoordF) coord;
            Rotation = (CoordF) rotation;
            SafeBlock = (CoordF) coord;
        }
        MapId = mapId;

        if (instanceId != 0)
        {
            InstanceId = instanceId;
        }

        if (!UnlockedMaps.Contains(MapId))
        {
            UnlockedMaps.Add(MapId);
        }
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
                Status status = new(skillCast, Session.FieldPlayer.ObjectId, Session.FieldPlayer.ObjectId, 1);
                StatusHandler.Handle(Session, status);
            }

            // Refresh out-of-combat timer
            if (CombatCTS != null)
            {
                CombatCTS.Cancel();
            }
            CombatCTS = new();
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
        return new(stat.Max, stat.Min, postRegen);
    }

    public void IncrementGatheringCount(int recipeID, int amount)
    {
        GatheringCount gatheringCount = GatheringCount.FirstOrDefault(x => x.RecipeId == recipeID);
        if (gatheringCount is null)
        {
            int maxLimit = (int) (RecipeMetadataStorage.GetRecipe(recipeID).NormalPropLimitCount * 1.4);
            gatheringCount = new(recipeID, 0, maxLimit);
            GatheringCount.Add(gatheringCount);
        }

        if (gatheringCount.CurrentCount + amount <= gatheringCount.MaxCount)
        {
            gatheringCount.CurrentCount += amount;
        }
    }

    public void TrophyUpdate(int trophyId, long addAmount, int sendUpdateInterval = 1)
    {
        if (!TrophyData.ContainsKey(trophyId))
        {
            TrophyData[trophyId] = new(CharacterId, AccountId, trophyId);
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

    public void GetUnreadMailCount()
    {
        int unreadCount = Mailbox.Where(x => x.ReadTimestamp == 0).Count();
        Session.Send(MailPacket.Notify(unreadCount, true));
    }

    public void FallDamage()
    {
        int currentHp = Stats[PlayerStatId.Hp].Current;
        int fallDamage = currentHp * Math.Clamp(currentHp * 4 / 100 - 1, 0, 25) / 100; // TODO: Create accurate damage model
        ConsumeHp(fallDamage);
        Session.Send(StatPacket.UpdateStats(Session.FieldPlayer, PlayerStatId.Hp));
        Session.Send(FallDamagePacket.FallDamage(Session.FieldPlayer.ObjectId, fallDamage));
    }
}
