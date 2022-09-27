using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseGuild : DatabaseTable
{
    public DatabaseGuild() : base("guilds") { }

    public long Insert(Guild guild)
    {
        List<long> ugcUids = guild.Banners.Select(x => x.Uid).ToList();
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
            banners = JsonConvert.SerializeObject(ugcUids),
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

    public void UpdateEmblem(long guildId, string emblemUrl)
    {
        QueryFactory.Query(TableName).Where("id", guildId).Update(new
        {
            emblem = emblemUrl
        });
    }

    public void UpdateBanners(long guildId, List<UGC> guildBanners)
    {
        List<long> ugcUids = guildBanners.Select(x => x.Uid).ToList();
        QueryFactory.Query(TableName).Where("id", guildId).Update(new
        {
            banners = JsonConvert.SerializeObject(ugcUids)
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Guild ReadGuild(dynamic data)
    {
        List<UGC> banners = new();
        foreach (long ugcUid in JsonConvert.DeserializeObject<List<long>>(data.banners))
        {
            UGC? findByUid = DatabaseManager.UGC.FindByUid(ugcUid);
            if (findByUid != null)
            {
                banners.Add(findByUid);
            }
        }

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
            Banners = banners,
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
