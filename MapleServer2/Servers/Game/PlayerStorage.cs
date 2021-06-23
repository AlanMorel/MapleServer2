using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly ConcurrentDictionary<long, Player> IdStorage;
        private readonly ConcurrentDictionary<string, Player> NameStorage;

        public PlayerStorage()
        {
            IdStorage = new ConcurrentDictionary<long, Player>();

            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            NameStorage = new ConcurrentDictionary<string, Player>(ignoreCase);
        }

        public void AddPlayer(Player player)
        {
            IdStorage[player.CharacterId] = player;
            NameStorage[player.Name] = player;
        }

        public void RemovePlayer(Player player)
        {
            IdStorage.Remove(player.CharacterId, out _);
            NameStorage.Remove(player.Name, out _);
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
