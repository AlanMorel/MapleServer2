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
                player.AccountId,
                player.CreationTime,
                player.Name,
                player.Gender,
                player.Awakened,
                job = (int) player.Job,
                levelsid = player.Levels.Id,
                player.MapId,
                player.TitleId,
                player.InsigniaId,
                titles = JsonConvert.SerializeObject(player.Titles),
                prestigerewardsclaimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                player.MaxSkillTabs,
                player.ActiveSkillTabId,
                gameoptionsid = player.GameOptions.Id,
                walletid = player.Wallet.Id,
                chatsticker = JsonConvert.SerializeObject(player.ChatSticker),
                player.ClubId,
                coord = JsonConvert.SerializeObject(player.Coord),
                emotes = JsonConvert.SerializeObject(player.Emotes),
                favoritestickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                groupchatid = JsonConvert.SerializeObject(player.GroupChatId),
                guildapplications = JsonConvert.SerializeObject(player.GuildApplications),
                guildid = player.Guild?.Id,
                guildmemberid = player.GuildMember?.Id,
                inventoryid = player.Inventory.Id,
                player.IsDeleted,
                mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
                player.Motto,
                player.ProfileUrl,
                returncoord = JsonConvert.SerializeObject(player.ReturnCoord),
                player.ReturnMapId,
                skincolor = JsonConvert.SerializeObject(player.SkinColor),
                statpointdistribution = JsonConvert.SerializeObject(player.StatPointDistribution),
                stats = JsonConvert.SerializeObject(player.Stats),
                trophycount = JsonConvert.SerializeObject(player.TrophyCount),
                unlockedmaps = JsonConvert.SerializeObject(player.UnlockedMaps),
                unlockedtaxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
                player.VisitingHomeId
            });
        }

        /// <summary>
        /// Return the full player with the given id, with Hotbars, SkillTabs, Inventories, etc.
        /// </summary>
        /// <returns>Player</returns>
        public Player FindPlayerById(long characterId)
        {
            dynamic data = QueryFactory.Query(TableName).Where("characterid", characterId)
                .Join("levels", "levels.id", "characters.levelsid")
                .Join("accounts", "accounts.id", "characters.accountid")
                .Join("gameoptions", "gameoptions.id", "characters.gameoptionsid")
                .Join("wallets", "wallets.id", "characters.walletid")
                .LeftJoin("homes", "homes.accountid", "accounts.id")
                .Select(
                    "characters.{*}",
                    "levels.{level, exp, restexp, prestigelevel, prestigeexp, masteryexp}",
                    "accounts.{username, passwordhash, creationtime, lastlogintime, characterslots, meret, gamemeret, eventmeret, mesotoken, bankinventoryid, vipexpiration}",
                    "gameoptions.{keybinds, activehotbarid}",
                    "wallets.{meso, valortoken, treva, rue, havifruit}",
                    "homes.id as homeid")
                .FirstOrDefault();

            List<Hotbar> hotbars = DatabaseManager.Hotbars.FindAllByGameOptionsId(data.gameoptionsid);
            List<SkillTab> skillTabs = DatabaseManager.SkillTabs.FindAllByCharacterId(data.characterid, data.job);
            Inventory inventory = DatabaseManager.Inventories.FindById(data.inventoryid);
            BankInventory bankInventory = DatabaseManager.BankInventories.FindById(data.bankinventoryid);
            Dictionary<int, Trophy> trophies = DatabaseManager.Trophies.FindAllByCharacterId(data.characterid);
            foreach (KeyValuePair<int, Trophy> trophy in DatabaseManager.Trophies.FindAllByAccountId(data.accountid))
            {
                trophies.Add(trophy.Key, trophy.Value);
            }
            List<QuestStatus> questList = DatabaseManager.Quests.FindAllByCharacterId(data.characterid);

            return new Player()
            {
                CharacterId = data.characterid,
                AccountId = data.accountid,
                Account = new Account(data.accountid, data.username, data.passwordhash, data.creationtime, data.lastlogintime, data.characterslots,
                    data.meret, data.gamemeret, data.eventmeret, data.mesotoken, data.homeid ?? 0, data.vipexpiration, bankInventory),
                CreationTime = data.creationtime,
                Name = data.name,
                Gender = data.gender,
                Awakened = data.awakened,
                Job = (Job) data.job,
                Levels = new Levels(data.level, data.exp, data.restexp, data.prestigelevel, data.prestigeexp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.masteryexp), data.levelsid),
                MapId = data.mapid,
                TitleId = data.titleid,
                InsigniaId = data.insigniaid,
                Titles = JsonConvert.DeserializeObject<List<int>>(data.titles),
                PrestigeRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.prestigerewardsclaimed),
                MaxSkillTabs = data.maxskilltabs,
                ActiveSkillTabId = data.activeskilltabid,
                GameOptions = new GameOptions(JsonConvert.DeserializeObject<Dictionary<int, KeyBind>>(data.keybinds), hotbars, data.activehotbarid, data.gameoptionsid),
                Wallet = new Wallet(data.meso, data.valortoken, data.treva, data.rue, data.havifruit, data.walletid),
                Inventory = inventory,
                ChatSticker = JsonConvert.DeserializeObject<List<ChatSticker>>(data.chatsticker),
                ClubId = data.clubid,
                Coord = JsonConvert.DeserializeObject<CoordF>(data.coord),
                Emotes = JsonConvert.DeserializeObject<List<int>>(data.emotes),
                FavoriteStickers = JsonConvert.DeserializeObject<List<int>>(data.favoritestickers),
                GroupChatId = JsonConvert.DeserializeObject<int[]>(data.groupchatid),
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guildapplications),
                GuildId = data.guildid ?? 0,
                IsDeleted = data.isdeleted,
                Mapleopoly = JsonConvert.DeserializeObject<Mapleopoly>(data.mapleopoly),
                Motto = data.motto,
                ProfileUrl = data.profileurl,
                ReturnCoord = JsonConvert.DeserializeObject<CoordF>(data.returncoord),
                ReturnMapId = data.returnmapid,
                SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skincolor),
                StatPointDistribution = JsonConvert.DeserializeObject<StatDistribution>(data.statpointdistribution),
                Stats = JsonConvert.DeserializeObject<PlayerStats>(data.stats),
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophycount),
                UnlockedMaps = JsonConvert.DeserializeObject<List<int>>(data.unlockedmaps),
                UnlockedTaxis = JsonConvert.DeserializeObject<List<int>>(data.unlockedtaxis),
                VisitingHomeId = data.visitinghomeid,
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
            return ReadPartialPlayer(QueryFactory.Query(TableName).Where("characterid", characterId)
                .Join("levels", "levels.id", "characters.levelsid")
                .Join("accounts", "accounts.id", "characters.accountid")
                .LeftJoin("homes", "homes.accountid", "accounts.id")
                .Select(
                    "characters.{*}",
                    "levels.{level, exp, restexp, prestigelevel, prestigeexp, masteryexp}",
                    "accounts.{username, passwordhash, creationtime, lastlogintime, characterslots, meret, gamemeret, eventmeret}",
                    "homes.{plotmapid, plotnumber, apartmentnumber, expiration, id as homeid}")
                .FirstOrDefault());
        }

        /// <summary>
        /// Return the player with the given name with the minimal amount of data needed for Buddy list and Guild members.
        /// </summary>
        /// <returns>Player</returns>
        public Player FindPartialPlayerByName(string name)
        {
            return ReadPartialPlayer(QueryFactory.Query(TableName).Where("characters.name", name)
                            .Join("levels", "levels.id", "characters.levelsid")
                            .Join("accounts", "accounts.id", "characters.accountid")
                            .LeftJoin("homes", "homes.accountid", "accounts.id")
                            .Select(
                                "characters.{*}",
                                "levels.{level, exp, restexp, prestigelevel, prestigeexp, masteryexp}",
                                "accounts.{username, passwordhash, creationtime, lastlogintime, characterslots, meret, gamemeret, eventmeret}",
                                "homes.{plotmapid, plotnumber, apartmentnumber, expiration, id as homeid}")
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
            .Join("levels", "levels.id", "characters.levelsid")
            .Select(
                "characters.{*}",
                "levels.{level, exp, restexp, prestigelevel, prestigeexp, masteryexp}").Get();

            foreach (dynamic data in result)
            {
                characters.Add(new Player()
                {
                    AccountId = data.accountid,
                    CharacterId = data.characterid,
                    CreationTime = data.creationtime,
                    Name = data.name,
                    Gender = data.gender,
                    Awakened = data.awakened,
                    Job = (Job) data.job,
                    Levels = new Levels(data.level, data.exp, data.restexp, data.prestigelevel, data.prestigeexp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.masteryexp), data.levelsid),
                    MapId = data.mapid,
                    Stats = JsonConvert.DeserializeObject<PlayerStats>(data.stats),
                    TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophycount),
                    Motto = data.motto,
                    ProfileUrl = data.profileurl,
                    Inventory = DatabaseManager.Inventories.FindById(data.inventoryid),
                    SkinColor = JsonConvert.DeserializeObject<SkinColor>(data.skincolor),
                });
            }
            return characters;
        }

        public void Update(Player player)
        {
            QueryFactory.Query(TableName).Where("characterid", player.CharacterId).Update(new
            {
                player.Name,
                player.Gender,
                player.Awakened,
                job = (int) player.Job,
                player.MapId,
                player.TitleId,
                player.InsigniaId,
                titles = JsonConvert.SerializeObject(player.Titles),
                prestigerewardsclaimed = JsonConvert.SerializeObject(player.PrestigeRewardsClaimed),
                player.MaxSkillTabs,
                player.ActiveSkillTabId,
                chatsticker = JsonConvert.SerializeObject(player.ChatSticker),
                player.ClubId,
                coord = JsonConvert.SerializeObject(player.Coord),
                emotes = JsonConvert.SerializeObject(player.Emotes),
                favoritestickers = JsonConvert.SerializeObject(player.FavoriteStickers),
                groupchatid = JsonConvert.SerializeObject(player.GroupChatId),
                guildapplications = JsonConvert.SerializeObject(player.GuildApplications),
                guildid = player.Guild?.Id,
                guildmemberid = player.GuildMember?.Id,
                player.IsDeleted,
                mapleopoly = JsonConvert.SerializeObject(player.Mapleopoly),
                player.Motto,
                player.ProfileUrl,
                returncoord = JsonConvert.SerializeObject(player.ReturnCoord),
                player.ReturnMapId,
                skincolor = JsonConvert.SerializeObject(player.SkinColor),
                statpointdistribution = JsonConvert.SerializeObject(player.StatPointDistribution),
                stats = JsonConvert.SerializeObject(player.Stats),
                trophycount = JsonConvert.SerializeObject(player.TrophyCount),
                unlockedmaps = JsonConvert.SerializeObject(player.UnlockedMaps),
                unlockedtaxis = JsonConvert.SerializeObject(player.UnlockedTaxis),
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

        public void UpdateProfileUrl(long characterId, string profileUrl) => QueryFactory.Query(TableName).Where("characterid", characterId).Update(new { ProfileUrl = profileUrl });

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("characterid", id).Delete() == 1;

        public bool SetCharacterDeleted(long characterId)
        {
            return QueryFactory.Query(TableName).Where("characterid", characterId).Update(new
            {
                IsDeleted = true
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
                    Id = data.homeid,
                    AccountId = data.accountid,
                    PlotMapId = data.plotmapid,
                    PlotNumber = data.plotnumber,
                    ApartmentNumber = data.apartmentnumber,
                    Expiration = data.expiration
                };
            }
            return new Player()
            {
                CharacterId = data.characterid,
                AccountId = data.accountid,
                Account = new Account()
                {
                    Home = home
                },
                CreationTime = data.creationtime,
                Name = data.name,
                Gender = data.gender,
                Awakened = data.awakened,
                Job = (Job) data.job,
                Levels = new Levels(data.level, data.exp, data.restexp, data.prestigelevel, data.prestigeexp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.masteryexp), data.levelsid),
                MapId = data.mapid,
                GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.guildapplications),
                Motto = data.motto,
                ProfileUrl = data.profileurl,
                TrophyCount = JsonConvert.DeserializeObject<int[]>(data.trophycount),
            };
        }
    }
}
