using System.Net;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
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
    public long LastLoginTime { get; set; }
    private long OnlineTime { get; set; }
    public bool IsDeleted;

    public string Name { get; set; }
    public Gender Gender { get; set; }

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
    public CoordF SavedCoord { get; set; }
    public CoordF SavedRotation { get; set; }
    public int MapId { get; set; }
    public long InstanceId { get; set; }
    public int TitleId { get; set; }
    public short InsigniaId { get; set; }
    public List<int> Titles { get; set; }
    public List<int> PrestigeRewardsClaimed { get; set; }

    public Stats Stats;
    public IFieldObject<Mount> Mount;
    public IFieldObject<Pet> Pet;
    public IFieldObject<GuideObject> Guide;
    public IFieldObject<Instrument> Instrument;

    public int SuperChat;
    public int ShopId; // current shop player is interacting

    public short ChannelId;
    public bool IsMigrating;

    // Combat, Adventure, Lifestyle
    public int[] TrophyCount;

    public Dictionary<int, Trophy> TrophyData = new();

    public List<ChatSticker> ChatSticker;
    public List<int> FavoriteStickers;
    public List<int> Emotes;

    public NpcTalk NpcTalk;

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
    public Guild Guild;
    public GuildMember GuildMember;
    public List<GuildApplication> GuildApplications = new();

    public Dictionary<int, Fishing> FishAlbum = new();
    public Item FishingRod; // Possibly temp solution?

    public Wallet Wallet { get; set; }
    public Dictionary<int, QuestStatus> QuestData;

    public CancellationTokenSource OnlineCTS;
    public Task OnlineTimeThread;

    public List<GatheringCount> GatheringCount;

    public List<Status> StatusContainer = new();
    public List<int> UnlockedTaxis;
    public List<int> UnlockedMaps;

    public List<string> GmFlags = new();
    public int DungeonSessionId = -1;

    public List<PlayerTrigger> Triggers = new();

    public IFieldActor<Player> FieldPlayer;

    public Player() { }

    // Initializes all values to be saved into the database
    public Player(Account account, string name, Gender gender, Job job, SkinColor skinColor)
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
        MapId = JobMetadataStorage.GetStartMapId((int) job);
        SavedCoord = MapEntityStorage.GetRandomPlayerSpawn(MapId).Coord.ToFloat();
        Stats = new(10, 10, 10, 10, 500, 10);
        Motto = "Motto";
        ProfileUrl = "";
        CreationTime = TimeInfo.Now();
        LastLoginTime = TimeInfo.Now();
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
        QuestData = new();
        GatheringCount = new();
        TrophyCount = new[]
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
            new(CharacterId, job, id: 1, name: "Build 1")
        };

        // Add initial quests
        foreach (QuestMetadata questMetadata in QuestMetadataStorage.GetAvailableQuests(Levels.Level, job))
        {
            QuestData.Add(questMetadata.Basic.Id, new(this, questMetadata));
        }

        // Get account trophies
        foreach ((int key, Trophy value) in DatabaseManager.Trophies.FindAllByAccountId(account.Id))
        {
            TrophyData.Add(key, value);
        }

        // Add initial trophy for level
        TrophyUpdate("level", 1);
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
        Session.Send(RequestFieldEnterPacket.RequestEnter(FieldPlayer));
    }

    public void WarpGameToGame(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        UpdateCoords(mapId, instanceId, coord, rotation);
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("GAME_PORT"));
        IPEndPoint endpoint = new(IPAddress.Parse(ipAddress), port);

        IsMigrating = true;

        Session.SendFinal(MigrationPacket.GameToGame(endpoint, this), logoutNotice: false);
    }

    private void SetCoords(int mapId, CoordF? coord, CoordF? rotation)
    {
        if (coord is not null && rotation is not null)
        {
            return;
        }

        MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);
        if (spawn is null)
        {
            Session.SendNotice($"Could not find a spawn for map {mapId}");
            return;
        }

        if (coord is null)
        {
            SavedCoord = spawn.Coord.ToFloat();
            SafeBlock = spawn.Coord.ToFloat();
        }

        if (rotation is null)
        {
            SavedRotation = spawn.Rotation.ToFloat();
        }
    }

    private void UpdateCoords(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        if (MapEntityStorage.HasSafePortal(MapId))
        {
            ReturnCoord = FieldPlayer.Coord;
            ReturnMapId = MapId;
        }

        if (coord is not null)
        {
            SavedCoord = (CoordF) coord;
            SafeBlock = (CoordF) coord;
        }

        if (rotation is not null)
        {
            SavedRotation = (CoordF) rotation;
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
        return tab switch
        {
            InventoryTab.Gear => Inventory.Equips,
            InventoryTab.Outfit => Inventory.Cosmetics,
            _ => null
        };
    }

    public Item GetEquippedItem(long itemUid)
    {
        Item gearItem = Inventory.Equips.FirstOrDefault(x => x.Value.Uid == itemUid).Value;
        if (gearItem is not null)
        {
            return gearItem;
        }

        return Inventory.Cosmetics.FirstOrDefault(x => x.Value.Uid == itemUid).Value;
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

    public void IncrementGatheringCount(int recipeId, int amount)
    {
        GatheringCount gatheringCount = GatheringCount.FirstOrDefault(x => x.RecipeId == recipeId);
        if (gatheringCount is null)
        {
            int maxLimit = (int) (RecipeMetadataStorage.GetRecipe(recipeId).NormalPropLimitCount * 1.4);
            gatheringCount = new(recipeId, 0, maxLimit);
            GatheringCount.Add(gatheringCount);
        }

        if (gatheringCount.CurrentCount + amount <= gatheringCount.MaxCount)
        {
            gatheringCount.CurrentCount += amount;
        }
    }

    public void TrophyUpdate(string type, int addAmount, string code = "", string target = "", int sendUpdateInterval = 1)
    {
        IEnumerable<TrophyMetadata> trophies = TrophyMetadataStorage.GetTrophiesByCondition(type, code, target);
        foreach (TrophyMetadata metadata in trophies)
        {
            if (TrophyData.ContainsKey(metadata.Id))
            {
                continue;
            }

            TrophyData[metadata.Id] = new(CharacterId, AccountId, metadata.Id);
        }

        IEnumerable<Trophy> enumerable = TrophyData.Values.Where(x =>
            x.GradeCondition.ConditionType == type &&
            (x.GradeCondition.ConditionCodes.Length == 0 || x.GradeCondition.ConditionCodes.Contains(code)) &&
            (x.GradeCondition.ConditionTargets.Length == 0 || x.GradeCondition.ConditionTargets.Contains(target)));
        foreach (Trophy trophy in enumerable)
        {
            trophy.AddCounter(Session, addAmount);
            if (trophy.Counter % sendUpdateInterval != 0)
            {
                continue;
            }

            DatabaseManager.Trophies.Update(trophy);
            Session?.Send(TrophyPacket.WriteUpdate(trophy));
        }
    }

    public Task OnlineTimer()
    {
        OnlineCTS = new();
        return Task.Run(async () =>
        {
            // First wait one minute before updating the online time
            await Task.Delay(60000);

            // Then update the online time every minute if CTS is not requested
            while (!OnlineCTS.IsCancellationRequested)
            {
                OnlineTime += 1;
                LastLoginTime = TimeInfo.Now();
                TrophyUpdate("playtime", 1);
                await Task.Delay(60000);
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
        int unreadCount = Mailbox.Count(x => x.ReadTimestamp == 0);
        Session.Send(MailPacket.Notify(unreadCount, true));
    }

    public void FallDamage()
    {
        long currentHp = Stats[StatId.Hp].TotalLong;
        int fallDamage = (int) (currentHp * Math.Clamp(currentHp * 4 / 100 - 1, 0, 25) / 100); // TODO: Create accurate damage model
        FieldPlayer.ConsumeHp(fallDamage);
        Session.Send(StatPacket.UpdateStats(FieldPlayer, StatId.Hp));
        Session.Send(FallDamagePacket.FallDamage(FieldPlayer.ObjectId, fallDamage));
    }
}
