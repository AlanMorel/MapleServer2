using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuildMember
    {
        private readonly string TableName = "GuildMembers";

        public void Insert(GuildMember guildMember)
        {
            DatabaseManager.QueryFactory.Query(TableName).Insert(new
            {
                guildMember.Id,
                guildMember.Motto,
                guildMember.Rank,
                guildMember.DailyContribution,
                guildMember.ContributionTotal,
                guildMember.DailyDonationCount,
                guildMember.AttendanceTimestamp,
                guildMember.JoinTimestamp,
                guildMember.GuildId
            });
        }

        public GuildMember FindById(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Get<GuildMember>().FirstOrDefault();

        public List<GuildMember> FindAllByGuildId(long guildId)
        {
            List<GuildMember> members = DatabaseManager.QueryFactory.Query(TableName).Where("GuildId", guildId).Get<GuildMember>().ToList();
            foreach (GuildMember guildMember in members)
            {
                dynamic result = DatabaseManager.QueryFactory.Query("Characters").Where("CharacterId", guildMember.Id)
                    .Join("Levels", "Levels.Id", "Characters.LevelsId")
                    .Join("Accounts", "Accounts.Id", "Characters.AccountId")
                    .LeftJoin("Homes", "Homes.AccountId", "Accounts.Id")
                    .Select(
                        "Characters.{*}",
                        "Levels.{Level, Exp, RestExp, PrestigeLevel, PrestigeExp, MasteryExp}",
                        "Accounts.{Username, PasswordHash, CreationTime, LastLoginTime, CharacterSlots, Meret, GameMeret, EventMeret}",
                        "Homes.{PlotMapId, PlotNumber, ApartmentNumber, Expiration, Id as HomeId}")
                    .FirstOrDefault();
                Home home = null;
                if (result.HomeId != null)
                {
                    home = new Home()
                    {
                        Id = result.HomeId,
                        AccountId = result.AccountId,
                        PlotMapId = result.PlotMapId,
                        PlotNumber = result.PlotNumber,
                        ApartmentNumber = result.ApartmentNumber,
                        Expiration = result.Expiration
                    };
                }
                guildMember.Player = new Player()
                {
                    CharacterId = result.CharacterId,
                    AccountId = result.AccountId,
                    Account = new Account()
                    {
                        Home = home
                    },
                    CreationTime = result.CreationTime,
                    Name = result.Name,
                    Gender = result.Gender,
                    Awakened = result.Awakened,
                    Job = (Job) result.Job,
                    Levels = new Levels(result.Level, result.Exp, result.RestExp, result.PrestigeLevel, result.PrestigeExp, JsonConvert.DeserializeObject<List<MasteryExp>>(result.MasteryExp), result.LevelsId),
                    MapId = result.MapId,
                    GuildApplications = JsonConvert.DeserializeObject<List<GuildApplication>>(result.GuildApplications),
                    Motto = result.Motto,
                    ProfileUrl = result.ProfileUrl,
                    TrophyCount = JsonConvert.DeserializeObject<int[]>(result.TrophyCount),
                };
            }
            return members;
        }

        public void Update(GuildMember guildMember)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", guildMember.Id).Update(new
            {
                guildMember.Rank,
                guildMember.DailyContribution,
                guildMember.ContributionTotal,
                guildMember.DailyDonationCount,
                guildMember.AttendanceTimestamp,
                guildMember.JoinTimestamp,
                guildMember.Motto,
                guildMember.GuildId
            });
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
