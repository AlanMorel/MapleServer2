using System;
using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly Dictionary<long, Player> IdStorage;
        private readonly Dictionary<string, Player> NameStorage;

        public PlayerStorage()
        {
            IdStorage = new Dictionary<long, Player>();

            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            NameStorage = new Dictionary<string, Player>(ignoreCase);
        }

        public void AddPlayer(Player player)
        {
            IdStorage.Add(player.CharacterId, player);
            NameStorage.Add(player.Name, player);
        }

        public void RemovePlayer(Player player)
        {
            IdStorage.Remove(player.CharacterId);
            NameStorage.Remove(player.Name);
        }

        public Player GetPlayerByName(string name)
        {
            return NameStorage.TryGetValue(name, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerById(long id)
        {
            return IdStorage.TryGetValue(id, out Player foundPlayer) ? foundPlayer : null;
        }
    }
}
