using System.Collections.Concurrent;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game;

public class PlayerManager
{
    private readonly ConcurrentDictionary<long, Player> CharacterId;
    private readonly ConcurrentDictionary<string, Player> NameStorage;

    public PlayerManager()
    {
        CharacterId = new();

        StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
        NameStorage = new(ignoreCase);
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

    public List<Player> GetAllPlayers()
    {
        return CharacterId.Values.ToList();
    }
}
