using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuild
    {
        public static long CreateGuild(Guild guild)
        {
            return DatabaseManager.QueryFactory.Query("Guilds").InsertGetId<long>(new
            {
                guild.Name,
                guild.CreationTimestamp,
                guild.LeaderAccountId,
                guild.LeaderCharacterId,
                guild.LeaderName,
                guild.Capacity,
                guild.Funds,
                guild.Exp,
                guild.Searchable,
                Buffs = JsonConvert.SerializeObject(guild.Buffs),
                guild.Emblem,
                guild.FocusAttributes,
                guild.HouseRank,
                guild.HouseTheme,
                guild.Notice,
                Ranks = JsonConvert.SerializeObject(guild.Ranks),
                Services = JsonConvert.SerializeObject(guild.Services),
            });
        }

        public static Guild FindById(long id) => ReadGuild(DatabaseManager.QueryFactory.Query("guilds").Where("Id", id).FirstOrDefault());

        public static bool GuildNameExists(string name) => DatabaseManager.QueryFactory.Query("Guilds").Where("Name", name).AsCount().FirstOrDefault().count == 1;

        public static List<Guild> GetAllGuilds()
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query("guilds").Get();
            List<Guild> guilds = new List<Guild>();
            foreach (dynamic data in result)
            {
                guilds.Add(ReadGuild(data));
            }
            return guilds;
        }

        public static void Update(Guild guild)
        {
            DatabaseManager.QueryFactory.Query("Guilds").Where("Id", guild.Id).Update(new
            {
                guild.Name,
                guild.CreationTimestamp,
                guild.LeaderAccountId,
                guild.LeaderCharacterId,
                guild.LeaderName,
                guild.Capacity,
                guild.Funds,
                guild.Exp,
                guild.Searchable,
                Buffs = JsonConvert.SerializeObject(guild.Buffs),
                guild.Emblem,
                guild.FocusAttributes,
                guild.HouseRank,
                guild.HouseTheme,
                guild.Notice,
                Ranks = JsonConvert.SerializeObject(guild.Ranks),
                Services = JsonConvert.SerializeObject(guild.Services),
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("Guilds").Where("Id", id).Delete() == 1;

        private static Guild ReadGuild(dynamic data)
        {
            return new Guild()
            {
                Id = data.Id,
                Name = data.Name,
                CreationTimestamp = data.CreationTimestamp,
                LeaderAccountId = data.LeaderAccountId,
                LeaderCharacterId = data.LeaderCharacterId,
                LeaderName = data.LeaderName,
                Capacity = data.Capacity,
                Funds = data.Funds,
                Exp = data.Exp,
                Searchable = data.Searchable,
                Buffs = JsonConvert.DeserializeObject<List<GuildBuff>>(data.Buffs),
                Emblem = data.Emblem,
                FocusAttributes = data.FocusAttributes,
                HouseRank = data.HouseRank,
                HouseTheme = data.HouseTheme,
                Notice = data.Notice,
                Ranks = JsonConvert.DeserializeObject<GuildRank[]>(data.Ranks),
                Services = JsonConvert.DeserializeObject<List<GuildService>>(data.Services),
                Members = DatabaseGuildMember.GetMembersByGuildId(data.Id),
                Applications = DatabaseGuildApplication.FindAllByGuildId(data.Id),
            };
        }
    }
}
