using System.Net;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Player
{
    public long SessionId;
    public GameSession? Session;

    public Account Account;

    // Constant Values
    public long AccountId { get; set; }
    public long CharacterId { get; set; }
    public long CreationTime { get; set; }
    public long LastLogTime { get; set; }
    public long DeletionTime { get; set; }
    public long Birthday { get; set; } // Currently just uses the creation time from account
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
    public int GearScore;
    public List<int> Titles { get; set; }
    public List<int> PrestigeRewardsClaimed { get; set; }
    public List<PrestigeMission> PrestigeMissions = new();

    public Stats Stats;

    public IFieldObject<Mount>? Mount;
    public IFieldObject<GuideObject>? Guide;
    public IFieldObject<Instrument>? Instrument;

    public Item? ActivePet;

    public long HouseStorageAccessTime;
    public long HouseDoctorAccessTime;
    public int DungeonHelperAccessTime; // tick

    public int SuperChatId;
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

    public List<GameEventUserValue> EventUserValues = new();
    public long RPSOpponentId;
    public RpsChoice RPSSelection;
    public int RouletteId;

    public int MaxSkillTabs { get; set; }
    public long ActiveSkillTabId { get; set; }

    public List<SkillTab> SkillTabs;
    public StatDistribution StatPointDistribution;

    public GameOptions GameOptions { get; set; }
    public List<Macro> Macros { get; set; }

    public IInventory Inventory;
    public DismantleInventory DismantleInventory = new();
    public LockInventory LockInventory = new();
    public HairInventory HairInventory = new();
    public TradeInventory TradeInventory;
    public ItemEnchant? ItemEnchant; // Current item player is enchanting
    public List<Wardrobe> Wardrobes = new();

    public List<Mail> Mailbox = new();

    public List<Buddy> BuddyList;

    public Party Party;

    public List<ClubMember> ClubMembers = new();
    public List<Club> Clubs = new();

    public List<GroupChat> GroupChats = new();

    public long GuildId;
    public Guild? Guild;
    public GuildMember? GuildMember;
    public List<GuildApplication> GuildApplications = new();

    public Dictionary<int, Fishing> FishAlbum = new();
    public Item FishingRod; // Possibly temp solution?

    public Wallet Wallet { get; set; }
    public Dictionary<int, QuestStatus> QuestData;

    public CancellationTokenSource OnlineCTS;
    public Task OnlineTimeThread;
    public Task TimeSyncTask;

    public List<GatheringCount> GatheringCount;

    public AdditionalEffects AdditionalEffects = new();
    public List<Status> StatusContainer = new();
    public List<int> UnlockedTaxis;
    public List<int> UnlockedMaps;

    public List<string> GmFlags = new();
    public int DungeonSessionId = -1;
    public int MushkingRoyaleSession = -1;

    public List<PlayerTrigger> Triggers = new();

    public Character? FieldPlayer;

    private Dictionary<int, short> PassiveSkillEffects = new();
    private Dictionary<int, SkillLevel> EnabledPassiveSkillEffects = new();
    public Player() { }

    // Initializes all values to be saved into the database
    public Player(Account account, string name, Gender gender, Job job, SkinColor skinColor)
    {
        AccountId = account.Id;
        Account = account;
        Name = name;
        Gender = gender;
        Job = job;
        GameOptions = new(job);
        Macros = new();
        Wallet = new(meso: 0, valorToken: 0, treva: 0, rue: 0, haviFruit: 0, gameSession: null);
        Levels = new(playerLevel: 1, exp: 0, restExp: 0, prestigeLevel: 1, prestigeExp: 0, masteryExp: new()
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
        }, gameSession: null);
        MapId = JobMetadataStorage.GetStartMapId(job) ?? 0;
        SavedCoord = MapEntityMetadataStorage.GetRandomPlayerSpawn(MapId)?.Coord.ToFloat() ?? default;
        Stats = new(job);
        Motto = "Motto";
        ProfileUrl = "";
        CreationTime = TimeInfo.Now();
        LastLogTime = TimeInfo.Now();
        Birthday = account.CreationTime;
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
        StatPointDistribution = new();
        Inventory = new Inventory(true);
        Mailbox = new();
        BuddyList = new();
        QuestData = new();
        GatheringCount = new();
        TrophyCount = new int[3];
        ReturnMapId = (int) Map.Tria;
        ReturnCoord = MapEntityMetadataStorage.GetRandomPlayerSpawn(ReturnMapId)?.Coord.ToFloat() ?? default;
        PrestigeMissions = PrestigeLevelMissionMetadataStorage.GetPrestigeMissions;
        SkinColor = skinColor;
        UnlockedTaxis = new();
        UnlockedMaps = new();
        ActiveSkillTabId = 1;
        CharacterId = DatabaseManager.Characters.Insert(this);
        SkillTabs = new()
        {
            new(CharacterId, job, JobCode, id: 1, name: "Build 1")
        };

        // Add initial quests
        foreach (QuestMetadata questMetadata in QuestMetadataStorage.GetAvailableQuests(Levels.Level, job))
        {
            QuestData.Add(questMetadata.Basic.Id, new(CharacterId, questMetadata));
        }

        // Get account trophies, only used for the OnLevelUp event
        foreach ((int key, Trophy value) in DatabaseManager.Trophies.FindAllByAccountId(account.Id))
        {
            TrophyData.Add(key, value);
        }

        // Add initial trophy for level
        TrophyManager.OnLevelUp(this);
    }

    public void UpdateBuddies()
    {
        BuddyList.ForEach(buddy =>
        {
            if (!buddy.Friend?.Session?.Connected() ?? true)
            {
                return;
            }

            Buddy myBuddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddy.SharedId);
            buddy.Friend.Session?.Send(BuddyPacket.LoginLogoutNotification(myBuddy));
            buddy.Friend.Session?.Send(BuddyPacket.UpdateBuddy(myBuddy));
        });
    }

    public void UpdateSocials()
    {
        Party?.BroadcastPacketParty(PartyPacket.UpdatePlayer(this));
        Guild?.BroadcastPacketGuild(GuildPacket.UpdatePlayer(this));
        foreach (Club club in Clubs)
        {
            ClubMember membership = ClubMembers.First(x => x.ClubId == club.Id);
            club?.BroadcastPacketClub(ClubPacket.UpdatePlayer(membership, club));
        }
    }

    public void Warp(int mapId, CoordF? coord = null, CoordF? rotation = null, long instanceId = -1)
    {
        if (MapMetadataStorage.GetMetadata(mapId)?.Property.IsTutorialMap ?? true)
        {
            WarpGameToGame(mapId, instanceId, coord, rotation);
            return;
        }

        if (mapId == MapId)
        {
            if (coord is null || rotation is null)
            {
                MapPlayerSpawn? spawn = GetSpawnCoords(mapId);
                if (spawn is null)
                {
                    Move(SavedCoord, SavedRotation);
                    return;
                }

                Move(spawn.Coord.ToFloat(), spawn.Rotation.ToFloat());
                return;
            }

            Move((CoordF) coord, (CoordF) rotation);
            return;
        }

        UpdateCoords(mapId, instanceId, coord, rotation);

        Session?.FieldManager.RemovePlayer(this);
        DatabaseManager.Characters.Update(this);
        Session?.Send(FieldEnterPacket.RequestEnter(FieldPlayer));
        Party?.BroadcastPacketParty(PartyPacket.UpdateMemberLocation(this));
        Guild?.BroadcastPacketGuild(GuildPacket.UpdateMemberLocation(Name, MapId));
        foreach (Club club in Clubs)
        {
            club?.BroadcastPacketClub(ClubPacket.UpdateMemberLocation(club.Id, Name, MapId));
        }
    }

    public void Warp(Map mapId, CoordF? coord = null, CoordF? rotation = null, long instanceId = 1)
    {
        Warp((int) mapId, coord, rotation, instanceId);
    }

    public void WarpGameToGame(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        UpdateCoords(mapId, instanceId, coord, rotation);

        string ipAddress = (Session?.IsLocalHost() ?? true) ? Constant.LocalHost : Environment.GetEnvironmentVariable("IP")!;
        int port = int.Parse(Environment.GetEnvironmentVariable("GAME_PORT")!);
        IPEndPoint endpoint = new(IPAddress.Parse(ipAddress), port);

        IsMigrating = true;

        Session?.SendFinal(MigrationPacket.GameToGame(endpoint, this), logoutNotice: false);
    }

    public void Move(CoordF coord, CoordF rotation, bool isTrigger = false)
    {
        FieldPlayer.Coord = coord;
        FieldPlayer.Rotation = rotation;

        Session?.Send(UserMoveByPortalPacket.Move(FieldPlayer, coord, rotation, isTrigger));
    }

    public Dictionary<ItemSlot, Item>? GetEquippedInventory(InventoryTab tab)
    {
        return tab switch
        {
            InventoryTab.Gear => Inventory.Equips,
            InventoryTab.Outfit => Inventory.Cosmetics,
            _ => null
        };
    }

    public Task TimeSyncLoop()
    {
        return Task.Run(async () =>
        {
            while (Session is not null)
            {
                Session?.Send(TimeSyncPacket.Request());
                await Task.Delay(TimeSpan.FromSeconds(1), OnlineCTS.Token);
            }
        }, OnlineCTS.Token);
    }

    public Task ClientTickSyncLoop()
    {
        return Task.Run(async () =>
        {
            while (Session != null)
            {
                Session?.Send(RequestPacket.TickSync());
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        });
    }

    public void IncrementGatheringCount(int recipeId, int amount)
    {
        GatheringCount? gatheringCount = GatheringCount.FirstOrDefault(x => x.RecipeId == recipeId);
        if (gatheringCount is null)
        {
            RecipeMetadata? recipeMetadata = RecipeMetadataStorage.GetRecipe(recipeId);
            if (recipeMetadata is null)
            {
                return;
            }

            int maxLimit = (int) (recipeMetadata.NormalPropLimitCount * 1.4);
            gatheringCount = new(recipeId, 0, maxLimit);
            GatheringCount.Add(gatheringCount);
        }

        if (gatheringCount.CurrentCount + amount <= gatheringCount.MaxCount)
        {
            gatheringCount.CurrentCount += amount;
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
                TrophyManager.OnPlayTimeTick(this);
                await Task.Delay(60000);
            }
        });
    }

    public void AddStatPoint(int amount, OtherStatsIndex index)
    {
        StatPointDistribution.AddTotalStatPoints(amount, index);
        Session?.Send(StatPointPacket.WriteTotalStatPoints(this));
    }

    public void AddSkillPoint(int amount, int rank, SkillPointSource index)
    {
        StatPointDistribution.AddTotalSkillPoints(amount, rank, index);
        Session?.Send(SkillPointPacket.ExtraSkillPoints(this));
    }

    public void GetUnreadMailCount(bool sendExpiryNotification = false)
    {
        int unreadCount = Mailbox.Count(x => x.ReadTimestamp == 0);
        if (sendExpiryNotification)
        {
            Session?.Send(MailPacket.ExpireNotification());
        }

        Session?.Send(MailPacket.Notify(unreadCount, true));
    }

    public void FallDamage()
    {
        MapUi? mapUi = MapMetadataStorage.GetMapUi(MapId);
        if (mapUi is { EnableFallDamage: false })
        {
            return;
        }

        long currentHp = Stats[StatAttribute.Hp].TotalLong;
        int fallDamage = (int) (currentHp * Math.Clamp(currentHp * 4 / 100 - 1, 0, 25) / 100); // TODO: Create accurate damage model
        FieldPlayer.ConsumeHp(fallDamage);
        Session?.Send(StatPacket.UpdateStats(FieldPlayer, StatAttribute.Hp));
        Session?.Send(FallDamagePacket.FallDamage(FieldPlayer.ObjectId, fallDamage));
    }

    public void GetMeretMarketPersonalListings()
    {
        List<UgcMarketItem> items = GameServer.UgcMarketManager.GetItemsByCharacterId(CharacterId);

        // TODO: Possibly a better way to implement updating item status?
        foreach (UgcMarketItem item in items)
        {
            if (item.ListingExpirationTimestamp < TimeInfo.Now() && item.Status == UgcMarketListingStatus.Active)
            {
                item.Status = UgcMarketListingStatus.Expired;
                DatabaseManager.UgcMarketItems.Update(item);
            }
        }

        Session?.Send(MeretMarketPacket.LoadPersonalListings(items));
    }

    public void GetMeretMarketSales()
    {
        List<UgcMarketSale> sales = GameServer.UgcMarketManager.GetSalesByCharacterId(CharacterId);
        Session?.Send(MeretMarketPacket.LoadSales(sales));
    }

    /// <summary>
    /// Remove all skills with level 0 from hotbar
    /// </summary>
    public void RemoveSkillsFromHotbar()
    {
        SkillTab skillTab = SkillTabs.First(x => x.TabId == ActiveSkillTabId);
        Hotbar hotbar = GameOptions.Hotbars[GameOptions.ActiveHotbarId];
        foreach (QuickSlot quickSlot in hotbar.Slots)
        {
            if (quickSlot.SkillId == 0)
            {
                continue;
            }

            if (skillTab.SkillLevels.Any(x => x.Key == quickSlot.SkillId && x.Value == 0))
            {
                hotbar.RemoveQuickSlot(quickSlot);
            }
        }
    }

    public void AddNewSkillsToHotbar(HashSet<int> newSkillIds)
    {
        foreach (int skillId in newSkillIds)
        {
            if (SkillMetadataStorage.IsPassive(skillId))
            {
                continue;
            }

            GameOptions.Hotbars[GameOptions.ActiveHotbarId].AddToFirstSlot(QuickSlot.From(skillId));
        }
    }

    public bool TryGetWardrobe(int index, out Wardrobe? wardrobe)
    {
        if (Wardrobes.ElementAtOrDefault(index) is null)
        {
            wardrobe = null;
            return false;
        }

        wardrobe = Wardrobes[index];
        return true;
    }

    private MapPlayerSpawn? GetSpawnCoords(int mapId)
    {
        MapPlayerSpawn? spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(mapId);
        if (spawn is null)
        {
            Session?.SendNotice($"Could not find a spawn for map {mapId}");
            return null;
        }

        return spawn;
    }

    private void UpdateCoords(int mapId, long instanceId, CoordF? coord = null, CoordF? rotation = null)
    {
        if (MapEntityMetadataStorage.HasSafePortal(MapId))
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

        if (coord is null && rotation is null)
        {
            MapPlayerSpawn? spawn = GetSpawnCoords(mapId);
            if (spawn is not null)
            {
                SavedCoord = spawn.Coord;
                SavedRotation = spawn.Rotation;
                SafeBlock = SavedCoord;
            }
        }

        MapId = mapId;

        if (instanceId != -1)
        {
            InstanceId = instanceId;
        }

        if (!UnlockedMaps.Contains(MapId))
        {
            UnlockedMaps.Add(MapId);
        }
    }

    public void UpdateGearScore(Item item, int value)
    {
        ItemType itemType = item.GetItemType();
        switch (itemType)
        {
            case ItemType.ThrowingStar:
            case ItemType.Dagger:
                value /= 2;
                break;
            case ItemType.Shield:
            case ItemType.Spellbook:
                return;
        }

        GearScore += value;
    }

    public void ComputeStatContribution(ItemStats stats)
    {
        foreach (ItemStat stat in stats.Constants.Values)
        {
            Stats.AddStat(stat.ItemAttribute, stat.AttributeType, stat.Flat, stat.Rate);
        }

        foreach (ItemStat stat in stats.Statics.Values)
        {
            Stats.AddStat(stat.ItemAttribute, stat.AttributeType, stat.Flat, stat.Rate);
        }

        foreach (ItemStat stat in stats.Randoms.Values)
        {
            Stats.AddStat(stat.ItemAttribute, stat.AttributeType, stat.Flat, stat.Rate);
        }

        foreach (ItemStat stat in stats.Enchants.Values)
        {
            int constantValue = stats.Constants.TryGetValue(stat.ItemAttribute, out ItemStat? itemStat) ? itemStat.Flat : 0;
            int staticValue = stats.Statics.TryGetValue(stat.ItemAttribute, out ItemStat? itemStat2) ? itemStat2.Flat : 0;
            int totalStat = constantValue + staticValue;
            Stats[stat.ItemAttribute].Add((int) (totalStat * stat.Rate));
        }

        foreach (ItemStat stat in stats.LimitBreakEnchants.Values)
        {
            if (stat.ItemAttribute is StatAttribute.MinWeaponAtk or StatAttribute.MaxWeaponAtk)
            {
                int constantValue = stats.Constants.TryGetValue(stat.ItemAttribute, out ItemStat? itemStat) ? itemStat.Flat : 0;
                int staticValue = stats.Statics.TryGetValue(stat.ItemAttribute, out ItemStat? itemStat2) ? itemStat2.Flat : 0;
                int totalStat = constantValue + staticValue;
                Stats[stat.ItemAttribute].Add((int) (totalStat * stat.Rate));

                continue;
            }

            Stats.AddStat(stat.ItemAttribute, stat.AttributeType, stat.Flat, stat.Rate);
        }
    }

    public void AddStats()
    {
        Stats = new(Job);
        Stats.AddBaseStats(this, Levels.Level);
        Stats.RecomputeAllocations(StatPointDistribution);

        foreach ((ItemSlot _, Item item) in Inventory.Equips)
        {
            ComputeStatContribution(item.Stats);

            foreach (GemSocket? socket in item.GemSockets.Sockets)
            {
                if (socket.Gemstone != null)
                {
                    ComputeStatContribution(socket.Gemstone.Stats);
                }
            }
        }

        if (ActivePet != null)
        {
            if (ActivePet.PetInfo != null)
            {
                Stats[StatAttribute.PetBonusAtk].Add(0, (float) ActivePet.PetInfo.Level - 1);
            }

            ComputeStatContribution(ActivePet.Stats);
        }

        foreach (AdditionalEffect effect in AdditionalEffects.Effects)
        {
            FieldPlayer.IncreaseStats(effect);
        }

        Stats.ComputeStatBonuses();

        if (Job == Job.Runeblade)
        {
            Stats.Data[StatAttribute.Int].AddBonus((long) (0.7f * Stats.Data[StatAttribute.Str].TotalLong));
        }

        Stats.AddAttackBonus(this);
    }

    public void EffectAdded(AdditionalEffect effect) { }

    public void EffectRemoved(AdditionalEffect effect) { }

    public void InitializeEffects()
    {
        AdditionalEffects.Parent = FieldPlayer;

        foreach (Item? item in Inventory.LapenshardStorage)
        {
            if (item != null)
            {
                LapenshardHandler.AddEffects(this, item);
            }
        }

        UpdatePassiveSkills();
    }

    public void AddEffects(ItemAdditionalEffectMetadata? effects)
    {
        if (effects?.Level == null || effects.Id == null)
        {
            return;
        }

        for (int i = 0; i < effects.Level.Length; ++i)
        {
            AdditionalEffects.AddEffect(new(effects.Id[i], effects.Level[i]));
        }
    }

    public void RemoveEffects(ItemAdditionalEffectMetadata? effects)
    {
        if (effects?.Level == null || effects?.Id == null)
        {
            return;
        }

        for (int i = 0; i < effects.Level.Length; ++i)
        {
            AdditionalEffects.RemoveEffect(effects.Id[i], effects.Level[i]);
        }
    }

    public void UpdatePassiveSkills()
    {
        foreach ((int id, short _) in PassiveSkillEffects)
        {
            PassiveSkillEffects[id] = -1;
        }

        foreach ((int id, short level) in SkillTabs[(int) ActiveSkillTabId - 1].GetSkillsByType(SkillType.Passive))
        {
            PassiveSkillEffects[id] = level;
        }

        EffectTriggers triggers = new()
        {
            Caster = FieldPlayer,
            Owner = FieldPlayer
        };

        foreach ((int id, short level) in PassiveSkillEffects)
        {
            SkillLevel? skillLevel;

            if (level < 1)
            {
                if (!EnabledPassiveSkillEffects.TryGetValue(id, out skillLevel))
                {
                    continue;
                }

                if (skillLevel.ConditionSkills == null)
                {
                    continue;
                }

                foreach (SkillCondition trigger in skillLevel.ConditionSkills)
                {
                    foreach (int skillId in trigger.SkillId)
                    {
                        AdditionalEffects.RemoveEffect(skillId, level);
                    }
                }

                EnabledPassiveSkillEffects.Remove(id);

                continue;
            }

            SkillMetadata? skill = SkillMetadataStorage.GetSkill(id);
            skillLevel = skill?.SkillLevels.FirstOrDefault(x => x.Level == level, skill.SkillLevels[0]);

            if (skillLevel is null)
            {
                continue;
            }

            EnabledPassiveSkillEffects[id] = skillLevel;

            //AdditionalEffects.AddEffect(new(id, level));
            FieldPlayer.SkillTriggerHandler.FireTriggers(skillLevel.ConditionSkills, triggers);
        }
    }
}
