using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseGuild : DatabaseTable
{
    public DatabaseGuild() : base("guilds") { }

    public long Insert(Guild guild)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            guild.Name,
            creation_timestamp = guild.CreationTimestamp,
            leader_account_id = guild.LeaderAccountId,
            leader_character_id = guild.LeaderCharacterId,
            leader_name = guild.LeaderName,
            guild.Capacity,
            guild.Funds,
            guild.Exp,
            guild.Searchable,
            buffs = JsonConvert.SerializeObject(guild.Buffs),
            guild.Emblem,
            focus_attributes = guild.FocusAttributes,
            house_rank = guild.HouseRank,
            house_theme = guild.HouseTheme,
            guild.Notice,
            ranks = JsonConvert.SerializeObject(guild.Ranks),
            services = JsonConvert.SerializeObject(guild.Services)
        });
    }

    public Guild FindById(long id)
    {
        return ReadGuild(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public bool NameExists(string name)
    {
        return QueryFactory.Query(TableName).Where("name", name).AsCount().FirstOrDefault().count == 1;
    }

    public List<Guild> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<Guild> guilds = new();
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
            creation_timestamp = guild.CreationTimestamp,
            leader_account_id = guild.LeaderAccountId,
            leader_character_id = guild.LeaderCharacterId,
            leader_name = guild.LeaderName,
            guild.Capacity,
            guild.Funds,
            guild.Exp,
            guild.Searchable,
            buffs = JsonConvert.SerializeObject(guild.Buffs),
            guild.Emblem,
            focus_attributes = guild.FocusAttributes,
            house_rank = guild.HouseRank,
            house_theme = guild.HouseTheme,
            guild.Notice,
            ranks = JsonConvert.SerializeObject(guild.Ranks),
            services = JsonConvert.SerializeObject(guild.Services)
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Guild ReadGuild(dynamic data)
    {
        return new()
        {
            Id = data.id,
            Name = data.name,
            CreationTimestamp = data.creation_timestamp,
            LeaderAccountId = data.leader_account_id,
            LeaderCharacterId = data.leader_character_id,
            LeaderName = data.leader_name,
            Capacity = data.capacity,
            Funds = data.funds,
            Exp = data.exp,
            Searchable = data.searchable,
            Buffs = JsonConvert.DeserializeObject<List<GuildBuff>>(data.buffs),
            Emblem = data.emblem,
            FocusAttributes = data.focus_attributes,
            HouseRank = data.house_rank,
            HouseTheme = data.house_theme,
            Notice = data.notice,
            Ranks = JsonConvert.DeserializeObject<GuildRank[]>(data.ranks),
            Services = JsonConvert.DeserializeObject<List<GuildService>>(data.services),
            Members = DatabaseManager.GuildMembers.FindAllByGuildId(data.id),
            Applications = DatabaseManager.GuildApplications.FindAllByGuildId(data.id)
        };
    }
}
