using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuild : DatabaseTable
    {
        public DatabaseGuild() : base("guilds") { }

        public long Insert(Guild guild)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
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
                buffs = JsonConvert.SerializeObject(guild.Buffs),
                guild.Emblem,
                guild.FocusAttributes,
                guild.HouseRank,
                guild.HouseTheme,
                guild.Notice,
                ranks = JsonConvert.SerializeObject(guild.Ranks),
                services = JsonConvert.SerializeObject(guild.Services),
            });
        }

        public Guild FindById(long id) => ReadGuild(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());

        public bool NameExists(string name) => QueryFactory.Query(TableName).Where("name", name).AsCount().FirstOrDefault().count == 1;

        public List<Guild> FindAll()
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
            List<Guild> guilds = new List<Guild>();
            foreach (dynamic data in result)
            {
                guilds.Add(ReadGuild(data));
            }
            return guilds;
        }

        public void Update(Guild guild)
        {
            QueryFactory.Query(TableName).Where("id", guild.Id).Update(new
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
                buffs = JsonConvert.SerializeObject(guild.Buffs),
                guild.Emblem,
                guild.FocusAttributes,
                guild.HouseRank,
                guild.HouseTheme,
                guild.Notice,
                ranks = JsonConvert.SerializeObject(guild.Ranks),
                services = JsonConvert.SerializeObject(guild.Services),
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static Guild ReadGuild(dynamic data)
        {
            return new Guild()
            {
                Id = data.id,
                Name = data.name,
                CreationTimestamp = data.creationtimestamp,
                LeaderAccountId = data.leaderaccountid,
                LeaderCharacterId = data.leadercharacterid,
                LeaderName = data.leadername,
                Capacity = data.capacity,
                Funds = data.funds,
                Exp = data.exp,
                Searchable = data.searchable,
                Buffs = JsonConvert.DeserializeObject<List<GuildBuff>>(data.buffs),
                Emblem = data.emblem,
                FocusAttributes = data.focusattributes,
                HouseRank = data.houserank,
                HouseTheme = data.housetheme,
                Notice = data.notice,
                Ranks = JsonConvert.DeserializeObject<GuildRank[]>(data.ranks),
                Services = JsonConvert.DeserializeObject<List<GuildService>>(data.services),
                Members = DatabaseManager.GuildMembers.FindAllByGuildId(data.id),
                Applications = DatabaseManager.GuildApplications.FindAllByGuildId(data.id),
            };
        }
    }
}
