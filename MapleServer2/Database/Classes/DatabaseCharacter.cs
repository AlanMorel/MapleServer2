using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCharacter : DatabaseTable
    {
        public DatabaseCharacter() : base("Characters") { }

        public long Insert(Player player)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                player.AccountId,
                player.CreationTime,
                player.Name,
                player.Gender,
                Job = (int) player.Job,
                LevelsId = player.Levels.Id,
                player.MapId,
                player.TitleId,
                player.InsigniaId,
                Titles = JsonConvert.SerializeObject(player.Titles),
                PrestigeRewardsClaimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                player.MaxSkillTabs,
                player.ActiveSkillTabId,
                GameOptionsId = player.GameOptions.Id,
                WalletId = player.Wallet.Id,
                ChatSticker = JsonConvert.SerializeObject(player.ChatSticker),
                player.ClubId,
                Coord = JsonConvert.SerializeObject(player.Coord),
                Emotes = JsonConvert.SerializeObject(player.Emotes),
                FavoriteStickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                GroupChatId = JsonConvert.SerializeObject(player.GroupChatId),
                GuildApplications = JsonConvert.SerializeObject(player.GuildApplications),
                GuildId = player.Guild?.Id,
                GuildMemberId = player.GuildMember?.Id,
                InventoryId = player.Inventory.Id,
                player.IsDeleted,
                Mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
                player.Motto,
                player.ProfileUrl,
                ReturnCoord = JsonConvert.SerializeObject(player.ReturnCoord),
                player.ReturnMapId,
                SkinColor = JsonConvert.SerializeObject(player.SkinColor),
                StatPointDistribution = JsonConvert.SerializeObject(player.StatPointDistribution),
                Stats = JsonConvert.SerializeObject(player.Stats),
                TrophyCount = JsonConvert.SerializeObject(player.TrophyCount),
                UnlockedMaps = JsonConvert.SerializeObject(player.UnlockedMaps),
                UnlockedTaxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
                player.VisitingHomeId
            });
        }

        /// <summary>
        /// Return the full player with the given id, with Hotbars, SkillTabs, Inventories, etc.
        /// </summary>
        /// <returns>Player</returns>
        public Player FindPlayerById(long characterId)
        {
            dynamic data = QueryFactory.Query(TableName).Where("CharacterId", characterId)
                .Join("Levels", "Levels.Id", "Characters.LevelsId")
                .Join("Accounts", "Accounts.Id", "Characters.AccountId")
                .Join("GameOptions", "GameOptions.Id", "Characters.GameOptionsId")
                .Join("Wallets", "Wallets.Id", "Characters.WalletId")
                .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
                .Select(
                    "Characters.{*}",
                    "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}",
                    "Accounts.{Username, PasswordHash, CreationTime, LastLoginTime, CharacterSlots, Meret, GameMeret, EventMeret, MesoToken, BankInventoryId, VIPExpiration}",
                    "GameOptions.{KeyBinds, ActiveHotbarId}",
                    "Wallets.{Meso, ValorToken, Treva, Rue, HaviFruit}",
                    "Homes.Id as HomeId")
                .FirstOrDefault();

            List<Hotbar> hotbars = DatabaseManager.Hotbars.FindAllByGameOptionsId(data.GameOptionsId);
            List<SkillTab> skillTabs = DatabaseManager.SkillTabs.FindAllByCharacterId(data.CharacterId, data.Job);
            Inventory inventory = DatabaseManager.Inventories.FindById(data.InventoryId);
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.BankInventoryId);
            Dictionary<int, Trophy> trophies = DatabaseManager.Trophies.FindAllByCharacterId(data.CharacterId);
            foreach (KeyValuePair<int, Trophy> trophy in DatabaseManager.Trophies.FindAllByAccountId(data.AccountId))
            {
                trophies.Add(trophy.Key, trophy.Value);
            }
            List<QuestStatus> questList = DatabaseManager.Quests.FindAllByCharacterId(data.CharacterId);

            return new Player()
            {
                CharacterId = data.CharacterId,
                AccountId = data.AccountId,
                Account = new Account(data.AccountId, data.Username, data.PasswordHash, data.CreationTime, data.LastLoginTime, data.CharacterSlots,
                    data.Meret, data.GameMeret, data.EventMeret, data.MesoToken, data.HomeId ?? 0, data.VIPExpiration, bankInventory),
                CreationTime = data.CreationTime,
                Name = data.Name,
                Gender = data.Gender,
                Job = (Job) data.Job,
                Levels = new Levels(data.Level, data.Exp, data.RestExp, data.PrestigeLevel, data.PrestigeExp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.MasteryExp), data.LevelsId),
                MapId = data.MapId,
                TitleId = data.TitleId,
                InsigniaId = data.InsigniaId,
                Titles = JsonConvert.DeserializeObject<List<int>>(data.Titles),
                PrestigeRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.PrestigeRewardsClaimed),
                MaxSkillTabs = data.MaxSkillTabs,
                ActiveSkillTabId = data.ActiveSkillTabId,
                GameOptions = new GameOptions(JsonConvert.DeserializeObject<Dictionary<int, KeyBind>>(data.KeyBinds), hotbars, data.ActiveHotbarId, data.GameOptionsId),
                Wallet = new Wallet(data.Meso, data.ValorToken, data.Treva, data.Rue, data.HaviFruit, data.WalletId),
                Inventory = inventory,
                ChatSticker = JsonConvert.DeserializeObject<List<ChatSticker>>(data.ChatSticker),
                ClubId = data.ClubId,
                Coord = JsonConvert.DeserializeObject<CoordF>(data.Coord),
                Emotes = JsonConvert.DeserializeObject<List<int>>(data.Emotes),
                FavoriteStickers = JsonConvert.DeserializeObject<List<int>>(data.FavoriteStickers),
                GroupChatId = JsonConvert.DeserializeObject<int[]>(data.GroupChatId),
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.GuildApplications),
                GuildId = data.GuildId ?? 0,
                IsDeleted = data.IsDeleted,
                Mapleopoly = JsonConvert.DeserializeObject<Mapleopoly>(data.Mapleopoly),
                Motto = data.Motto,
                ProfileUrl = data.ProfileUrl,
                ReturnCoord = JsonConvert.DeserializeObject<CoordF>(data.ReturnCoord),
                ReturnMapId = data.ReturnMapId,
                SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.SkinColor),
                StatPointDistribution = JsonConvert.DeserializeObject<StatDistribution>(data.StatPointDistribution),
                Stats = JsonConvert.DeserializeObject<PlayerStats>(data.Stats),
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.TrophyCount),
                UnlockedMaps = JsonConvert.DeserializeObject<List<int>>(data.UnlockedMaps),
                UnlockedTaxis = JsonConvert.DeserializeObject<List<int>>(data.UnlockedTaxis),
                VisitingHomeId = data.VisitingHomeId,
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
            return ReadPartialPlayer(QueryFactory.Query(TableName).Where("CharacterId", characterId)
                .Join("Levels", "Levels.Id", "Characters.LevelsId")
                .Join("Accounts", "Accounts.Id", "Characters.AccountId")
                .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
                .Select(
                    "Characters.{*}",
                    "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}",
                    "Accounts.{Username, PasswordHash, CreationTime, LastLoginTime, CharacterSlots, Meret, GameMeret, EventMeret}",
                    "Homes.{PlotMapId, PlotNumber, ApartmentNumber, Expiration, Id as HomeId}")
                .FirstOrDefault());
        }

        /// <summary>
        /// Return the player with the given name with the minimal amount of data needed for Buddy list and Guild members.
        /// </summary>
        /// <returns>Player</returns>
        public Player FindPartialPlayerByName(string name)
        {
            return ReadPartialPlayer(QueryFactory.Query(TableName).Where("Characters.Name", name)
                            .Join("Levels", "Levels.Id", "Characters.LevelsId")
                            .Join("Accounts", "Accounts.Id", "Characters.AccountId")
                            .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
                            .Select(
                                "Characters.{*}",
                                "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}",
                                "Accounts.{Username, PasswordHash, CreationTime, LastLoginTime, CharacterSlots, Meret, GameMeret, EventMeret}",
                                "Homes.{PlotMapId, PlotNumber, ApartmentNumber, Expiration, Id as HomeId}")
                            .FirstOrDefault());
        }

        public List<Player> FindAllByAccountId(long accountId)
        {
            List<Player> characters = new List<Player>();

            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where(new
            {
                AccountId = accountId,
                IsDeleted = false
            })
            .Join("Levels", "Levels.Id", "Characters.LevelsId")
            .Select(
                "Characters.{*}",
                "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}").Get();

            foreach (dynamic data in result)
            {
                characters.Add(new Player()
                {
                    AccountId = data.AccountId,
                    CharacterId = data.CharacterId,
                    CreationTime = data.CreationTime,
                    Name = data.Name,
                    Gender = data.Gender,
                    Job = (Job) data.Job,
                    Levels = new Levels(data.Level, data.Exp, data.RestExp, data.PrestigeLevel, data.PrestigeExp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.MasteryExp), data.LevelsId),
                    MapId = data.MapId,
                    Stats = JsonConvert.DeserializeObject<PlayerStats>(data.Stats),
                    TrophyCount = JsonConvert.DeserializeObject<int[]>(data.TrophyCount),
                    Motto = data.Motto,
                    ProfileUrl = data.ProfileUrl,
                    Inventory = DatabaseManager.Inventories.FindById(data.InventoryId),
                    SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.SkinColor),
                });
            }
            return characters;
        }

        public void Update(Player player)
        {
            QueryFactory.Query(TableName).Where("CharacterId", player.CharacterId).Update(new
            {
                player.Name,
                player.Gender,
                Job = (int) player.Job,
                player.MapId,
                player.TitleId,
                player.InsigniaId,
                Titles = JsonConvert.SerializeObject(player.Titles),
                PrestigeRewardsClaimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                player.MaxSkillTabs,
                player.ActiveSkillTabId,
                ChatSticker = JsonConvert.SerializeObject(player.ChatSticker),
                player.ClubId,
                Coord = JsonConvert.SerializeObject(player.Coord),
                Emotes = JsonConvert.SerializeObject(player.Emotes),
                FavoriteStickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                GroupChatId = JsonConvert.SerializeObject(player.GroupChatId),
                GuildApplications = JsonConvert.SerializeObject(player.GuildApplications),
                GuildId = player.Guild?.Id,
                GuildMemberId = player.GuildMember?.Id,
                player.IsDeleted,
                Mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
                player.Motto,
                player.ProfileUrl,
                ReturnCoord = JsonConvert.SerializeObject(player.ReturnCoord),
                player.ReturnMapId,
                SkinColor = JsonConvert.SerializeObject(player.SkinColor),
                StatPointDistribution = JsonConvert.SerializeObject(player.StatPointDistribution),
                Stats = JsonConvert.SerializeObject(player.Stats),
                TrophyCount = JsonConvert.SerializeObject(player.TrophyCount),
                UnlockedMaps = JsonConvert.SerializeObject(player.UnlockedMaps),
                UnlockedTaxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
                player.VisitingHomeId
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

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("CharacterId", id).Delete() == 1;

        public bool SetCharacterDeleted(long characterId)
        {
            return QueryFactory.Query(TableName).Where("CharacterId", characterId).Update(new
            {
                IsDeleted = true
            }) == 1;
        }

        public bool NameExists(string name) => QueryFactory.Query(TableName).Where("Name", name).AsCount().FirstOrDefault().count == 1;

        private static Player ReadPartialPlayer(dynamic data)
        {
            Home home = null;
            if (data.HomeId != null)
            {
                home = new Home()
                {
                    Id = data.HomeId,
                    AccountId = data.AccountId,
                    PlotMapId = data.PlotMapId,
                    PlotNumber = data.PlotNumber,
                    ApartmentNumber = data.ApartmentNumber,
                    Expiration = data.Expiration
                };
            }
            return new Player()
            {
                CharacterId = data.CharacterId,
                AccountId = data.AccountId,
                Account = new Account()
                {
                    Home = home
                },
                CreationTime = data.CreationTime,
                Name = data.Name,
                Gender = data.Gender,
                Job = (Job) data.Job,
                Levels = new Levels(data.Level, data.Exp, data.RestExp, data.PrestigeLevel, data.PrestigeExp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.MasteryExp), data.LevelsId),
                MapId = data.MapId,
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.GuildApplications),
                Motto = data.Motto,
                ProfileUrl = data.ProfileUrl,
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.TrophyCount),
            };
        }
    }
}
