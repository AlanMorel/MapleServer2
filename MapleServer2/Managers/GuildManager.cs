using MapleServer2.Types;

namespace MapleServer2.Managers;

public class GuildManager
{
    private readonly Dictionary<long, Guild> GuildList;

    public GuildManager() => GuildList = new();

    public void AddGuild(Guild guild) => GuildList.Add(guild.Id, guild);

    public void RemoveGuild(Guild guild) => GuildList.Remove(guild.Id);

    public List<Guild> GetGuildList() => GuildList.Values.Where(x => x.Searchable).ToList();

    public List<Guild> GetGuildListByName(string name)
    {
        List<Guild> allGuilds = GetGuildList();
        return allGuilds.Where(x => x.Name.Contains(name)).ToList();
    }

    public Guild GetGuildById(long id) => GuildList.TryGetValue(id, out Guild foundGuild) ? foundGuild : null;

    public Guild GetGuildByName(string name) => GuildList.Values.FirstOrDefault(guild => guild.Name == name);

    public Guild GetGuildByLeader(Player leader) => GuildList.Values.FirstOrDefault(guild => guild.LeaderCharacterId == leader.CharacterId);

    public List<Guild> GetAllGuilds() => GuildList.Values.ToList();
}
