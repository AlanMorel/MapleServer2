using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseGuild : DatabaseTable
    {
        public DatabaseGuild(string tableName) : base(tableName) { }

        public long Insert(Guild guild)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
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

        public Guild FindById(long id) => ReadGuild(DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).FirstOrDefault());

        public bool NameExists(string name) => DatabaseManager.QueryFactory.Query(TableName).Where("Name", name).AsCount().FirstOrDefault().count == 1;

        public List<Guild> FindAll()
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Get();
            List<Guild> guilds = new List<Guild>();
            foreach (dynamic data in result)
            {
                guilds.Add(ReadGuild(data));
            }
            return guilds;
        }

        public void Update(Guild guild)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", guild.Id).Update(new
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

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

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
                Members = DatabaseManager.GuildMembers.FindAllByGuildId(data.Id),
                Applications = DatabaseManager.GuildApplications.FindAllByGuildId(data.Id),
            };
        }
    }
}
