using System.Collections.Concurrent;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class PlayerStorage
    {
        private readonly ConcurrentDictionary<long, Player> CharacterId;
        private readonly ConcurrentDictionary<string, Player> NameStorage;

        public PlayerStorage()
        {
            CharacterId = new ConcurrentDictionary<long, Player>();

            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            NameStorage = new ConcurrentDictionary<string, Player>(ignoreCase);
        }

        public void AddPlayer(Player player)
        {
            CharacterId[player.CharacterId] = player;
            NameStorage[player.Name] = player;
        }

        public void RemovePlayer(Player player)
        {
            CharacterId.Remove(player.CharacterId, out _);
            NameStorage.Remove(player.Name, out _);
        }

        public Player GetPlayerByName(string name)
        {
            return NameStorage.TryGetValue(name, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerById(long id)
        {
            return CharacterId.TryGetValue(id, out Player foundPlayer) ? foundPlayer : null;
        }

        public Player GetPlayerByAccountId(long accountId)
        {
            Player player = CharacterId.Values.FirstOrDefault(p => p.AccountId == accountId);
            return player == default ? null : player;
        }
    }
}
