using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly ConcurrentDictionary<long, Player> CharacterId;
        private readonly ConcurrentDictionary<long, Player> AccountId;
        private readonly ConcurrentDictionary<string, Player> NameStorage;

        public PlayerStorage()
        {
            CharacterId = new ConcurrentDictionary<long, Player>();
            AccountId = new ConcurrentDictionary<long, Player>();

            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            NameStorage = new ConcurrentDictionary<string, Player>(ignoreCase);
        }

        public void AddPlayer(Player player)
        {
            CharacterId[player.CharacterId] = player;
            AccountId[player.AccountId] = player;
            NameStorage[player.Name] = player;
        }

        public void RemovePlayer(Player player)
        {
            CharacterId.Remove(player.CharacterId, out _);
            AccountId.Remove(player.AccountId, out _);
            NameStorage.Remove(player.Name, out _);
        }

        public Player GetPlayerByName(string name)
        {
            return NameStorage.TryGetValue(name, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerByCharacterId(long id)
        {
            return CharacterId.TryGetValue(id, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerByAccountId(long id)
        {
            return AccountId.TryGetValue(id, out Player foundPlayer) ? foundPlayer : null;
        }
    }
}
