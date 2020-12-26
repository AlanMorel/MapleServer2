using System;
using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly Dictionary<long, Player> idStorage;
        private readonly Dictionary<string, Player> nameStorage;

        public PlayerStorage()
        {
            idStorage = new Dictionary<long, Player>();

            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            nameStorage = new Dictionary<string, Player>(ignoreCase);
        }

        public void AddPlayer(Player player)
        {
            idStorage.Add(player.CharacterId, player);
            nameStorage.Add(player.Name, player);
        }

        public void RemovePlayer(Player player)
        {
            idStorage.Remove(player.CharacterId);
            nameStorage.Remove(player.Name);
        }

        public Player GetPlayerByName(string name)
        {
            return nameStorage.TryGetValue(name, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerById(long id)
        {
            return idStorage.TryGetValue(id, out Player foundPlayer) ? foundPlayer : null;
        }
    }
}