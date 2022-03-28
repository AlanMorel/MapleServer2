using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class FieldWar
{
    public int Id;
    public int PlayerCount;
    public DateTimeOffset EntryClosureTime;
    public int MapId;
    public FieldWar(int id)
    {
        Id = id;
        MapId = FieldWarMetadataStorage.MapId(id);

        List<Player> onlinePlayers = GameServer.PlayerManager.GetAllPlayers().Where(x => !MapMetadataStorage.MapIsInstancedOnly(x.MapId)).ToList();

        DateTimeOffset now = DateTimeOffset.UtcNow;
        EntryClosureTime = now.AddSeconds(-now.Second).AddMinutes(5);

        foreach (Player player in onlinePlayers)
        {
            player.Session.Send(FieldWarPacket.LegionPopup(id, EntryClosureTime.ToUnixTimeSeconds()));
        }
    }
}

