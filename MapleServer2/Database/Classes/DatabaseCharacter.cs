using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCharacter : DatabaseTable
    {
        public DatabaseCharacter() : base("characters") { }

        public long Insert(Player player)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                account_id = player.AccountId,
                creation_time = player.CreationTime,
                player.Name,
                player.Gender,
                player.Awakened,
                job = (int) player.Job,
                levels_id = player.Levels.Id,
                map_id = player.MapId,
                title_id = player.TitleId,
                insignia_id = player.InsigniaId,
                titles = JsonConvert.SerializeObject(player.Titles),
                prestige_rewards_claimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                max_skill_tabs = player.MaxSkillTabs,
                active_skill_tab_id = player.ActiveSkillTabId,
                game_options_id = player.GameOptions.Id,
                wallet_id = player.Wallet.Id,
                chat_sticker = JsonConvert.SerializeObject(player.ChatSticker),
                club_id = player.ClubId,
                coord = JsonConvert.SerializeObject(player.Coord),
                emotes = JsonConvert.SerializeObject(player.Emotes),
                favorite_stickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                group_chat_id = JsonConvert.SerializeObject(player.GroupChatId),
                guild_applications = JsonConvert.SerializeObject(player.GuildApplications),
                guild_id = player.Guild?.Id,
                guild_member_id = player.GuildMember?.Id,
                inventory_id = player.Inventory.Id,
                is_deleted = player.IsDeleted,
                mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
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
                visiting_home_id = player.VisitingHomeId
            });
        }

        /// <summary>
        /// Return the full player with the given id, with Hotbars, SkillTabs, Inventories, etc.
        /// </summary>
        /// <returns>Player</returns>
        public Player FindPlayerById(long characterId)
        {
            dynamic data = QueryFactory.Query(TableName).Where("character_id", characterId)
                .Join("levels", "levels.id", "characters.levels_id")
                .Join("accounts", "accounts.id", "characters.account_id")
                .Join("game_options", "game_options.id", "characters.game_options_id")
                .Join("wallets", "wallets.id", "characters.wallet_id")
                .LeftJoin("homes", "homes.account_id", "accounts.id")
                .Select(
                    "characters.{*}",
                    "levels.{level, exp, rest_exp, prestige_level, prestige_exp, mastery_exp}",
                    "accounts.{username, password_hash, creation_time, last_login_time, character_slots, meret, game_meret, event_meret, meso_token, bank_inventory_id, vip_expiration}",
                    "game_options.{keybinds, active_hotbar_id}",
                    "wallets.{meso, valor_token, treva, rue, havi_fruit}",
                    "homes.id as home_id")
                .FirstOrDefault();

            List<Hotbar> hotbars = DatabaseManager.Hotbars.FindAllByGameOptionsId(data.game_options_id);
            List<SkillTab> skillTabs = DatabaseManager.SkillTabs.FindAllByCharacterId(data.character_id, data.job);
            Inventory inventory = DatabaseManager.Inventories.FindById(data.inventory_id);
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bank_inventory_id);
            Dictionary<int, Trophy> trophies = DatabaseManager.Trophies.FindAllByCharacterId(data.character_id);
            foreach (KeyValuePair<int, Trophy> trophy in DatabaseManager.Trophies.FindAllByAccountId(data.account_id))
            {
                trophies.Add(trophy.Key, trophy.Value);
            }
            List<QuestStatus> questList = DatabaseManager.Quests.FindAllByCharacterId(data.character_id);

            return new Player()
            {
                CharacterId = data.character_id,
                AccountId = data.account_id,
                Account = new Account(data.account_id, data.username, data.password_hash, data.creation_time, data.last_login_time, data.character_slots,
                    data.meret, data.game_meret, data.event_meret, data.meso_token, data.home_id ?? 0, data.vip_expiration, bankInventory),
                CreationTime = data.creation_time,
                Name = data.name,
                Gender = data.gender,
                Awakened = data.awakened,
                Job = (Job) data.job,
                Levels = new Levels(data.level, data.exp, data.rest_exp, data.prestige_level, data.prestige_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), data.levels_id),
                MapId = data.map_id,
                TitleId = data.title_id,
                InsigniaId = data.insignia_id,
                Titles = JsonConvert.DeserializeObject<List<int>>(data.titles),
                PrestigeRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.prestige_rewards_claimed),
                MaxSkillTabs = data.max_skill_tabs,
                ActiveSkillTabId = data.active_skill_tab_id,
                GameOptions = new GameOptions(JsonConvert.DeserializeObject<Dictionary<int, KeyBind>>(data.keybinds), hotbars, data.active_hotbar_id, data.game_options_id),
                Wallet = new Wallet(data.meso, data.valor_token, data.treva, data.rue, data.havi_fruit, data.wallet_id),
                Inventory = inventory,
                ChatSticker = JsonConvert.DeserializeObject<List<ChatSticker>>(data.chat_sticker),
                ClubId = data.club_id,
                Coord = JsonConvert.DeserializeObject<CoordF>(data.coord),
                Emotes = JsonConvert.DeserializeObject<List<int>>(data.emotes),
                FavoriteStickers = JsonConvert.DeserializeObject<List<int>>(data.favorite_stickers),
                GroupChatId = JsonConvert.DeserializeObject<int[]>(data.group_chat_id),
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guild_applications),
                GuildId = data.guild_id ?? 0,
                IsDeleted = data.is_deleted,
                Mapleopoly = JsonConvert.DeserializeObject<Mapleopoly>(data.mapleopoly),
                Motto = data.motto,
                ProfileUrl = data.profile_url,
                ReturnCoord = JsonConvert.DeserializeObject<CoordF>(data.return_coord),
                ReturnMapId = data.return_map_id,
                SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skin_color),
                StatPointDistribution = JsonConvert.DeserializeObject<StatDistribution>(data.statpoint_distribution),
                Stats = JsonConvert.DeserializeObject<PlayerStats>(data.stats),
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count),
                UnlockedMaps = JsonConvert.DeserializeObject<List<int>>(data.unlocked_maps),
                UnlockedTaxis = JsonConvert.DeserializeObject<List<int>>(data.unlocked_taxis),
                VisitingHomeId = data.visiting_home_id,
                SkillTabs = skillTabs,
                TrophyData = trophies,
                QuestList = questList
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
                    "levels.{level, exp, rest_exp, prestige_level, prestige_exp, mastery_exp}",
                    "accounts.{username, password_hash, creation_time, last_login_time, character_slots, meret, game_meret, event_meret}",
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
                                "levels.{level, exp, rest_exp, prestige_level, prestige_exp, mastery_exp}",
                                "accounts.{username, password_hash, creation_time, last_login_time, character_slots, meret, game_meret, event_meret}",
                                "homes.{plotmap_id, plot_number, apartment_number, expiration, id as home_id}")
                            .FirstOrDefault());
        }

        public List<Player> FindAllByAccountId(long accountId)
        {
            List<Player> characters = new List<Player>();

            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where(new
            {
                account_id = accountId,
                is_deleted = false
            })
            .Join("levels", "levels.id", "characters.levels_id")
            .Select(
                "characters.{*}",
                "levels.{level, exp, rest_exp, prestige_level, prestige_exp, mastery_exp}").Get();

            foreach (dynamic data in result)
            {
                characters.Add(new Player()
                {
                    AccountId = data.account_id,
                    CharacterId = data.character_id,
                    CreationTime = data.creation_time,
                    Name = data.name,
                    Gender = data.gender,
                    Awakened = data.awakened,
                    Job = (Job) data.job,
                    Levels = new Levels(data.level, data.exp, data.rest_exp, data.prestige_level, data.prestige_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), data.levels_id),
                    MapId = data.map_id,
                    Stats = JsonConvert.DeserializeObject<PlayerStats>(data.stats),
                    TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count),
                    Motto = data.motto,
                    ProfileUrl = data.profile_url,
                    Inventory = DatabaseManager.Inventories.FindById(data.inventory_id),
                    SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skin_color),
                });
            }
            return characters;
        }

        public void Update(Player player)
        {
            QueryFactory.Query(TableName).Where("character_id", player.CharacterId).Update(new
            {
                player.Name,
                player.Gender,
                player.Awakened,
                job = (int) player.Job,
                map_id = player.MapId,
                title_id = player.TitleId,
                insignia_id = player.InsigniaId,
                titles = JsonConvert.SerializeObject(player.Titles),
                prestige_rewards_claimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                max_skill_tabs = player.MaxSkillTabs,
                active_skill_tab_id = player.ActiveSkillTabId,
                chat_sticker = JsonConvert.SerializeObject(player.ChatSticker),
                club_id = player.ClubId,
                coord = JsonConvert.SerializeObject(player.Coord),
                emotes = JsonConvert.SerializeObject(player.Emotes),
                favorite_stickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                group_chat_id = JsonConvert.SerializeObject(player.GroupChatId),
                guild_applications = JsonConvert.SerializeObject(player.GuildApplications),
                guild_id = player.Guild?.Id,
                guild_member_id = player.GuildMember?.Id,
                is_deleted = player.IsDeleted,
                mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
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
                visiting_home_id = player.VisitingHomeId
            });
            DatabaseManager.Accounts.Update(player.Account);

            DatabaseManager.Levels.Update(player.Levels);
            DatabaseManager.Wallets.Update(player.Wallet);
            DatabaseManager.GameOptions.Update(player.GameOptions);
            DatabaseManager.Inventories.Update(player.Inventory);

            foreach (KeyValuePair<int, Trophy> trophy in player.TrophyData)
            {
                DatabaseManager.Trophies.Update(trophy.Value);
            }
        }

        public void UpdateProfileUrl(long characterId, string profileUrl) => QueryFactory.Query(TableName).Where("character_id", characterId).Update(new { profile_url = profileUrl });

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("character_id", id).Delete() == 1;

        public bool SetCharacterDeleted(long characterId)
        {
            return QueryFactory.Query(TableName).Where("character_id", characterId).Update(new
            {
                is_deleted = true
            }) == 1;
        }

        public bool NameExists(string name) => QueryFactory.Query(TableName).Where("name", name).AsCount().FirstOrDefault().count == 1;

        private static Player ReadPartialPlayer(dynamic data)
        {
            Home home = null;
            if (data.homeid != null)
            {
                home = new Home()
                {
                    Id = data.home_id,
                    AccountId = data.account_id,
                    PlotMapId = data.plotmap_id,
                    PlotNumber = data.plot_number,
                    ApartmentNumber = data.apartment_number,
                    Expiration = data.expiration
                };
            }
            return new Player()
            {
                CharacterId = data.character_id,
                AccountId = data.account_id,
                Account = new Account()
                {
                    Home = home
                },
                CreationTime = data.creation_time,
                Name = data.name,
                Gender = data.gender,
                Awakened = data.awakened,
                Job = (Job) data.job,
                Levels = new Levels(data.level, data.exp, data.rest_exp, data.prestige_level, data.prestige_exp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.mastery_exp), data.levels_id),
                MapId = data.map_id,
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guild_applications),
                Motto = data.motto,
                ProfileUrl = data.profile_url,
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophy_count),
            };
        }
    }
}
