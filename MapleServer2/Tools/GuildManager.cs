using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class GuildManager
    {
        private readonly Dictionary<long, Guild> GuildList;

        public GuildManager()
        {
            GuildList = new Dictionary<long, Guild>();
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
            return GuildList.Cast<Guild>().Where(guild => guild.Searchable = true).ToList();
        }

        public Guild GetGuildById(long id)
        {
            return GuildList.TryGetValue(id, out Guild foundGuild) ? foundGuild : null;
        }

        public Guild GetGuildByName(string name)
        {
            return (from entry in GuildList
                    where entry.Value.Name == name
                    select entry.Value).FirstOrDefault();
        }

        public Guild GetGuildByLeader(Player leader)
        {
            return (from entry in GuildList
                    where entry.Value.Leader.CharacterId == leader.CharacterId
                    select entry.Value).FirstOrDefault();
        }
    }
}
