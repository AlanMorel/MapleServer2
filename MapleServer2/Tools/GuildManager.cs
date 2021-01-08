using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class GuildManager
    {
        private readonly Dictionary<long, Guild> guildList;

        public GuildManager()
        {
            guildList = new Dictionary<long, Guild>();
        }

        public void AddGuild(Guild guild)
        {
            guildList.Add(guild.Id, guild);
        }

        public void RemoveGuild(Guild guild)
        {
            guildList.Remove(guild.Id);
        }

        public Guild GetGuildById(long id)
        {
            return guildList.TryGetValue(id, out Guild foundGuild) ? foundGuild : null;
        }

        public Guild GetGuildByName(string name)
        {
            return (from entry in guildList
                    where entry.Value.Name == name
                    select entry.Value).FirstOrDefault();
        }

        public Guild GetGuildByLeader(Player leader)
        {
            return (from entry in guildList
                    where entry.Value.Leader.CharacterId == leader.CharacterId
                    select entry.Value).FirstOrDefault();
        }
    }
}
