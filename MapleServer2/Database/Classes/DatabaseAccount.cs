using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public static class DatabaseAccount
    {
        public static long CreateAccount(Account account)
        {
            return DatabaseManager.QueryFactory.Query("Accounts").InsertGetId<long>(new
            {
                account.Username,
                account.PasswordHash,
                account.CreationTime,
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
            });
        }

        public static Account FindById(long id)
        {
            return ReadAccount(DatabaseManager.QueryFactory.Query("Accounts").Where("Accounts.Id", id)
            .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
            .Select("Accounts.{*}", "Homes.Id as HomeId")
            .FirstOrDefault());
        }

        public static Account FindByUsername(string username) => ReadAccount(DatabaseManager.QueryFactory.Query("Accounts").Where("Username", username).FirstOrDefault());

        public static bool Authenticate(string username, string password, out Account account)
        {
            account = null;
            Account dbAccount = FindByUsername(username);

            if (BCrypt.Net.BCrypt.Verify(password, dbAccount.PasswordHash))
            {
                account = dbAccount;
                return true;
            }
            return false;
        }

        internal static List<Player> FindAllByAccountId(long accountId)
        {
            List<Player> characters = new List<Player>();

            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query("Characters").Where("AccountId", accountId)
            .Join("Levels", "Levels.Id", "Characters.LevelsId")
            .Select(
                "Characters.{*}",
                "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}").Get();

            foreach (dynamic data in result)
            {
                characters.Add(new Player()
                {
                    CharacterId = data.CharacterId,
                    AccountId = data.AccountId,
                    CreationTime = data.CreationTime,
                    Name = data.Name,
                    Gender = data.Gender,
                    Awakened = data.Awakened,
                    Job = (Job) data.Job,
                    Levels = new Levels(data.Level, data.Exp, data.RestExp, data.PrestigeLevel, data.PrestigeExp, JsonConvert.DeserializeObject<List<MasteryExp>>(data.MasteryExp), data.LevelsId),
                    MapId = data.MapId,
                    TitleId = data.TitleId,
                    InsigniaId = data.InsigniaId,
                    Titles = JsonConvert.DeserializeObject<List<int>>(data.Titles),
                    PrestigeRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.PrestigeRewardsClaimed),
                    VIPExpiration = data.VIPExpiration,
                    MaxSkillTabs = data.MaxSkillTabs,
                    ActiveSkillTabId = data.ActiveSkillTabId,
                    BankInventory = DatabaseBankInventory.FindById(data.BankInventoryId),
                    ChatSticker = JsonConvert.DeserializeObject<List<ChatSticker>>(data.ChatSticker),
                    ClubId = data.ClubId,
                    Coord = JsonConvert.DeserializeObject<CoordF>(data.Coord),
                    Emotes = JsonConvert.DeserializeObject<List<int>>(data.Emotes),
                    FavoriteStickers = JsonConvert.DeserializeObject<List<int>>(data.FavoriteStickers),
                    GroupChatId = JsonConvert.DeserializeObject<int[]>(data.GroupChatId),
                    GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(data.GuildApplications),
                    GuildId = data.GuildId ?? 0,
                    Inventory = DatabaseInventory.FindById(data.InventoryId),
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
                    VisitingHomeId = data.VisitingHomeId
                });
            }
            return characters;
        }

        public static bool AccountExists(string username) => DatabaseManager.QueryFactory.Query("Accounts").Where("Username", username).AsCount().FirstOrDefault().count > 0;

        public static void Update(Account account)
        {
            DatabaseManager.QueryFactory.Query("Accounts").Where("Id", account.Id).Update(new
            {
                account.LastLoginTime,
                account.CharacterSlots,
                Meret = account.Meret.Amount,
                GameMeret = account.GameMeret.Amount,
                EventMeret = account.EventMeret.Amount,
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("Accounts").Where("Id", id).Delete() == 1;

        public static Account ReadAccount(dynamic data) => new Account(data.Username, data.PasswordHash, data.CreationTime, data.LastLoginTime, data.CharacterSlots, data.Meret, data.GameMeret, data.EventMeret, data.Id, data.HomeId ?? 0);
    }
}
