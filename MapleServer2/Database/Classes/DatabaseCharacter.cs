using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseCharacter : DatabaseTable
{
    public DatabaseCharacter() : base("characters") { }

    public long Insert(Player player)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            account_id = player.AccountId,
            creation_time = player.CreationTime,
            last_log_time = player.LastLogTime,
            birthday = player.Birthday,
            player.Name,
            gender = (byte) player.Gender,
            player.Awakened,
            channel_id = player.ChannelId,
            instance_id = player.InstanceId,
            is_migrating = player.IsMigrating,
            job = (int) player.JobCode,
            levels_id = player.Levels.Id,
            map_id = player.MapId,
            title_id = player.TitleId,
            insignia_id = player.InsigniaId,
            titles = JsonConvert.SerializeObject(player.Titles),
            gear_score = player.GearScore,
            max_skill_tabs = player.MaxSkillTabs,
            active_skill_tab_id = player.ActiveSkillTabId,
            game_options_id = player.GameOptions.Id,
            wallet_id = player.Wallet.Id,
            chat_sticker = JsonConvert.SerializeObject(player.ChatSticker),
            coord = JsonConvert.SerializeObject(player.SavedCoord),
            emotes = JsonConvert.SerializeObject(player.Emotes),
            favorite_stickers = JsonConvert.SerializeObject(player.FavoriteStickers),
            guild_applications = JsonConvert.SerializeObject(player.GuildApplications),
            guild_id = player.Guild?.Id,
            guild_member_id = player.GuildMember?.Id,
            inventory_id = player.Inventory.Id,
            is_deleted = player.IsDeleted,
            player.Motto,
            profile_url = player.ProfileUrl,
            return_coord = JsonConvert.SerializeObject(player.ReturnCoord),
            return_map_id = player.ReturnMapId,
            skin_color = JsonConvert.SerializeObject(player.SkinColor),
            statpoint_distribution = JsonConvert.SerializeObject(player.StatPointDistribution),
            stats = JsonConvert.SerializeObject(player.Stats),
            trophy_count = JsonConvert.SerializeObject(player.TrophyCount),
            unlocked_maps = JsonConvert.SerializeObject(player.UnlockedMaps),
            unlocked_taxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
            visiting_home_id = player.VisitingHomeId,
            gathering_count = JsonConvert.SerializeObject(player.GatheringCount),
            active_pet_item_uid = player.ActivePet?.Uid ?? 0
        });
    }

    /// <summary>
    /// Return the full player with the given id, with Hotbars, SkillTabs, Inventories, etc.
    /// </summary>
    /// <returns>Player</returns>
    public Player FindPlayerById(long characterId, GameSession session)
    {
        dynamic data = QueryFactory.Query(TableName).Where("character_id", characterId)
            .Join("levels", "levels.id", "characters.levels_id")
            .Join("accounts", "accounts.id", "characters.account_id")
            .Join("game_options", "game_options.id", "characters.game_options_id")
            .Join("wallets", "wallets.id", "characters.wallet_id")
            .Join("auth_data", "auth_data.account_id", "characters.account_id")
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select(
                "characters.{*}",
                "levels.{level, exp, rest_exp, mastery_exp}",
                "accounts.{username, password_hash, creation_time, last_log_time, character_slots, meret, game_meret, event_meret, meso_token, bank_inventory_id, mushking_royale_id, vip_expiration, " +
                "prestige_id, premium_rewards_claimed, meso_market_daily_listings, meso_market_monthly_purchases}",
                "game_options.{keybinds, active_hotbar_id}",
                "wallets.{meso, valor_token, treva, rue, havi_fruit}",
                "homes.id as home_id",
                "auth_data.{token_a, token_b, online_character_id}")
            .FirstOrDefault();

        List<Hotbar> hotbars = DatabaseManager.Hotbars.FindAllByGameOptionsId(data.game_options_id);
        List<Macro> macros = DatabaseManager.Macros.FindAllByCharacterId(data.character_id);
        List<SkillTab> skillTabs = DatabaseManager.SkillTabs.FindAllByCharacterId(data.character_id, data.job);
        IInventory inventory = DatabaseManager.Inventories.FindById(data.inventory_id);
        BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bank_inventory_id);
        MushkingRoyaleStats royaleStats = DatabaseManager.MushkingRoyaleStats.FindById(data.mushking_royale_id);
        List<Medal> medals = DatabaseManager.MushkingRoyaleMedals.FindAllByAccountId(data.account_id);
        Prestige prestige = DatabaseManager.Prestiges.FindById(data.prestige_id);
        Dictionary<int, Trophy> trophies = DatabaseManager.Trophies.FindAllByCharacterId(data.character_id);
        List<ClubMember> clubMemberships = DatabaseManager.ClubMembers.FindAllClubIdsByCharacterId(data.character_id);
        List<Wardrobe> wardrobes = DatabaseManager.Wardrobes.FindAllByCharacterId(data.character_id);
        foreach (KeyValuePair<int, Trophy> trophy in DatabaseManager.Trophies.FindAllByAccountId(data.account_id))
        {
            trophies.Add(trophy.Key, trophy.Value);
        }

        Dictionary<int, QuestStatus> questList = DatabaseManager.Quests.FindAllByCharacterId(data.character_id);
        AuthData authData = new(data.token_a, data.token_b, data.account_id, data.online_character_id ?? 0);

        Item pet = DatabaseManager.Items.FindByUid(data.active_pet_item_uid);
        pet?.SetMetadataValues();

        return new()
        {
            Session = session,
            CharacterId = data.character_id,
            AccountId = data.account_id,
            Account = new(data.account_id, data, bankInventory, royaleStats, prestige, medals, authData, session),
            CreationTime = data.creation_time,
            Birthday = data.birthday,
            Name = data.name,
            Gender = (Gender) data.gender,
            Awakened = data.awakened,
            ChannelId = data.channel_id,
            InstanceId = data.instance_id,
            IsMigrating = data.is_migrating,
            JobCode = (JobCode) data.job,
            Levels = new(data.level, data.exp, data.rest_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), session, data.levels_id),
            MapId = data.map_id,
            TitleId = data.title_id,
            InsigniaId = data.insignia_id,
            Titles = JsonConvert.DeserializeObject<List<int>>(data.titles),
            GearScore = data.gear_score,
            MaxSkillTabs = data.max_skill_tabs,
            ActiveSkillTabId = data.active_skill_tab_id,
            GameOptions = new GameOptions(JsonConvert.DeserializeObject<Dictionary<int, KeyBind>>(data.keybinds),
                hotbars, data.active_hotbar_id, data.game_options_id),
            Macros = macros,
            Wallet = new Wallet(data.meso, data.valor_token, data.treva, data.rue, data.havi_fruit, session, data.wallet_id),
            Inventory = inventory,
            Wardrobes = wardrobes,
            ChatSticker = JsonConvert.DeserializeObject<List<ChatSticker>>(data.chat_sticker),
            SavedCoord = JsonConvert.DeserializeObject<CoordF>(data.coord),
            Emotes = JsonConvert.DeserializeObject<List<int>>(data.emotes),
            FavoriteStickers = JsonConvert.DeserializeObject<List<int>>(data.favorite_stickers),
            GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guild_applications),
            GuildId = data.guild_id ?? 0,
            ClubMembers = clubMemberships,
            IsDeleted = data.is_deleted,
            Motto = data.motto,
            ProfileUrl = data.profile_url,
            ReturnCoord = JsonConvert.DeserializeObject<CoordF>(data.return_coord),
            ReturnMapId = data.return_map_id,
            SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skin_color),
            StatPointDistribution = JsonConvert.DeserializeObject<StatDistribution>(data.statpoint_distribution),
            Stats = JsonConvert.DeserializeObject<Stats>(data.stats),
            TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count),
            UnlockedMaps = JsonConvert.DeserializeObject<List<int>>(data.unlocked_maps),
            UnlockedTaxis = JsonConvert.DeserializeObject<List<int>>(data.unlocked_taxis),
            VisitingHomeId = data.visiting_home_id,
            SkillTabs = skillTabs,
            TrophyData = trophies,
            QuestData = questList,
            GatheringCount = JsonConvert.DeserializeObject<List<GatheringCount>>(data.gathering_count),
            ActivePet = pet
        };
    }

    /// <summary>
    /// Return the player with the given id with the minimal amount of data needed for Buddy list and Guild members.
    /// </summary>
    /// <returns>Player</returns>
    public Player FindPartialPlayerById(long characterId)
    {
        return ReadPartialPlayer(QueryFactory.Query(TableName).Where("character_id", characterId)
            .Join("levels", "levels.id", "characters.levels_id")
            .Join("accounts", "accounts.id", "characters.account_id")
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select(
                "characters.{*}",
                "levels.{level, exp, rest_exp, mastery_exp}",
                "accounts.{username, password_hash, creation_time, last_log_time, character_slots, meret, game_meret, event_meret}",
                "homes.{plot_map_id, plot_number, apartment_number, expiration, id as home_id}")
            .FirstOrDefault());
    }

    /// <summary>
    /// Return the player with the given name with the minimal amount of data needed for Buddy list and Guild members.
    /// </summary>
    /// <returns>Player</returns>
    public Player FindPartialPlayerByName(string name)
    {
        return ReadPartialPlayer(QueryFactory.Query(TableName).Where("characters.name", name)
            .Join("levels", "levels.id", "characters.levels_id")
            .Join("accounts", "accounts.id", "characters.account_id")
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select(
                "characters.{*}",
                "levels.{level, exp, rest_exp, mastery_exp}",
                "accounts.{username, password_hash, creation_time, last_log_time, character_slots, meret, game_meret, event_meret}",
                "homes.{plot_map_id, plot_number, apartment_number, expiration, id as home_id}")
            .FirstOrDefault());
    }

    /// <summary>
    /// Return the player with the given account id with the minimal amount of data needed for Buddy list and Guild members.
    /// </summary>
    /// <returns>Player</returns>
    public Player FindPartialPlayerByAccountId(long accountId)
    {
        return ReadPartialPlayer(QueryFactory.Query(TableName).Where("characters.account_id", accountId)
            .Join("levels", "levels.id", "characters.levels_id")
            .Join("accounts", "accounts.id", "characters.account_id")
            .LeftJoin("homes", "homes.account_id", "accounts.id")
            .Select(
                "characters.{*}",
                "levels.{level, exp, rest_exp, mastery_exp}",
                "accounts.{username, password_hash, creation_time, last_log_time, character_slots, meret, game_meret, event_meret}",
                "homes.{plotmap_id, plot_number, apartment_number, expiration, id as home_id}")
            .FirstOrDefault());
    }

    public List<Player> FindAllByAccountId(long accountId)
    {
        List<Player> characters = new();

        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where(new
        {
            account_id = accountId,
            is_deleted = false
        })
            .Join("levels", "levels.id", "characters.levels_id")
            .Join("prestiges", "prestiges.id", "characters.account_id")
            .Select(
                "characters.{*}",
                "prestiges.{id as prestige_id, level as prestige_level, exp as prestige_exp, rewards_claimed, missions}",
                "levels.{level, exp, rest_exp, mastery_exp}").Get();

        foreach (dynamic data in result)
        {
            Account account = new()
            {
                Prestige = new(data.prestige_id, data.prestige_level, data.prestige_exp, JsonConvert.DeserializeObject<List<int>>(data.rewards_claimed), 
                    JsonConvert.DeserializeObject<List<PrestigeMission>>(data.missions))
            };
            characters.Add(new()
            {
                AccountId = data.account_id,
                CharacterId = data.character_id,
                CreationTime = data.creation_time,
                Birthday = data.birthday,
                Name = data.name,
                Gender = (Gender) data.gender,
                Awakened = data.awakened,
                JobCode = (JobCode) data.job,
                Levels = new Levels(data.level, data.exp, data.rest_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), null, data.levels_id),
                MapId = data.map_id,
                Stats = JsonConvert.DeserializeObject<Stats>(data.stats),
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count),
                Motto = data.motto,
                ProfileUrl = data.profile_url,
                Inventory = DatabaseManager.Inventories.FindById(data.inventory_id),
                SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skin_color),
                Account = account
            });
        }

        return characters;
    }

    public void Update(Player player)
    {
        QueryFactory.Query(TableName).Where("character_id", player.CharacterId).Update(new
        {
            player.Name,
            last_log_time = player.LastLogTime,
            birthday = player.Birthday,
            gender = (byte) player.Gender,
            player.Awakened,
            channel_id = player.ChannelId,
            instance_id = player.InstanceId,
            is_migrating = player.IsMigrating,
            job = (int) player.JobCode,
            map_id = player.MapId,
            title_id = player.TitleId,
            insignia_id = player.InsigniaId,
            titles = JsonConvert.SerializeObject(player.Titles),
            gear_score = player.GearScore,
            max_skill_tabs = player.MaxSkillTabs,
            active_skill_tab_id = player.ActiveSkillTabId,
            chat_sticker = JsonConvert.SerializeObject(player.ChatSticker),
            coord = JsonConvert.SerializeObject(player.SavedCoord),
            emotes = JsonConvert.SerializeObject(player.Emotes),
            favorite_stickers = JsonConvert.SerializeObject(player.FavoriteStickers),
            guild_applications = JsonConvert.SerializeObject(player.GuildApplications),
            guild_id = player.Guild?.Id,
            guild_member_id = player.GuildMember?.Id,
            is_deleted = player.IsDeleted,
            player.Motto,
            profile_url = player.ProfileUrl,
            return_coord = JsonConvert.SerializeObject(player.ReturnCoord),
            return_map_id = player.ReturnMapId,
            skin_color = JsonConvert.SerializeObject(player.SkinColor),
            statpoint_distribution = JsonConvert.SerializeObject(player.StatPointDistribution),
            stats = JsonConvert.SerializeObject(player.Stats),
            trophy_count = JsonConvert.SerializeObject(player.TrophyCount),
            unlocked_maps = JsonConvert.SerializeObject(player.UnlockedMaps),
            unlocked_taxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
            visiting_home_id = player.VisitingHomeId,
            gathering_count = JsonConvert.SerializeObject(player.GatheringCount),
            active_pet_item_uid = player.ActivePet?.Uid ?? 0
        });
        DatabaseManager.Accounts.Update(player.Account);

        if (player.GuildMember is not null)
        {
            DatabaseManager.GuildMembers.Update(player.GuildMember);
        }

        DatabaseManager.Levels.Update(player.Levels);
        DatabaseManager.Wallets.Update(player.Wallet);
        DatabaseManager.GameOptions.Update(player.GameOptions);
        DatabaseManager.Inventories.Update(player.Inventory);

        foreach (KeyValuePair<int, Trophy> trophy in player.TrophyData)
        {
            DatabaseManager.Trophies.Update(trophy.Value);
        }
    }

    public void UpdateChannelId(long characterId, short channelId, long instanceId, bool isMigrating)
    {
        QueryFactory.Query(TableName).Where("character_id", characterId).Update(new
        {
            channel_id = channelId,
            instance_id = instanceId,
            is_migrating = isMigrating
        });
    }

    public void UpdateProfileUrl(long characterId, string profileUrl)
    {
        QueryFactory.Query(TableName).Where("character_id", characterId).Update(new
        {
            profile_url = profileUrl
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("character_id", id).Delete() == 1;
    }

    public bool SetCharacterDeleted(long characterId)
    {
        return QueryFactory.Query(TableName).Where("character_id", characterId).Update(new
        {
            is_deleted = true
        }) == 1;
    }

    public bool NameExists(string name)
    {
        return QueryFactory.Query(TableName).Where("name", name).AsCount().FirstOrDefault().count == 1;
    }

    private static Player? ReadPartialPlayer(dynamic? data)
    {
        if (data is null)
        {
            return null;
        }

        Home? home = null;
        if (data.homeid is not null)
        {
            home = new()
            {
                Id = data.home_id,
                AccountId = data.account_id,
                PlotMapId = data.plotmap_id,
                PlotNumber = data.plot_number,
                ApartmentNumber = data.apartment_number,
                Expiration = data.expiration
            };
        }

        return new()
        {
            CharacterId = data.character_id,
            AccountId = data.account_id,
            Account = new()
            {
                Home = home
            },
            CreationTime = data.creation_time,
            LastLogTime = data.last_log_time,
            Name = data.name,
            Gender = (Gender) data.gender,
            Awakened = data.awakened,
            JobCode = (JobCode) data.job,
            Levels = new Levels(data.level, data.exp, data.rest_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), null, data.levels_id),
            MapId = data.map_id,
            GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guild_applications),
            Motto = data.motto,
            ProfileUrl = data.profile_url,
            TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count)
        };
    }
}
