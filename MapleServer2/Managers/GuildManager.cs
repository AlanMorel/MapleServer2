using MapleServer2.Types;

namespace MapleServer2.Managers;

public class GuildManager
{
    private readonly Dictionary<long, Guild> GuildList;

    public GuildManager()
    {
        GuildList = new();
    }

    public void AddGuild(Guild guild)
    {
        GuildList.Add(guild.Id, guild);
    }

    public void RemoveGuild(Guild guild)
    {
        GuildList.Remove(guild.Id);
    }

    public List<Guild> GetGuildList()
    {
        return GuildList.Values.Where(x => x.Searchable).ToList();
    }

    public List<Guild> GetGuildListByName(string name)
    {
        List<Guild> allGuilds = GetGuildList();
        return allGuilds.Where(x => x.Name.Contains(name)).ToList();
    }

    public Guild GetGuildById(long id)
    {
        return GuildList.TryGetValue(id, out Guild foundGuild) ? foundGuild : null;
    }

    public Guild GetGuildByName(string name)
    {
        return GuildList.Values
            .Where(guild => guild.Name == name)
            .FirstOrDefault();
    }

    public Guild GetGuildByLeader(Player leader)
    {
        return GuildList.Values
            .Where(guild => guild.LeaderCharacterId == leader.CharacterId)
            .FirstOrDefault();
    }

    public List<Guild> GetAllGuilds()
    {
        return GuildList.Values.ToList();
    }
}
