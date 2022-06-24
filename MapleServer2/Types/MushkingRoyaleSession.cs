using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class MushkingRoyaleSession
{
    public int SessionId;
    public int MapId;
    public int InstanceId;
    public CoordF SpawnPosition;
    public CoordF SpawnRotation;

    public MushkingRoyaleSession(int mapId)
    {
        MapPlayerSpawn spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(mapId);
        MapId = mapId;
        SpawnPosition = spawn.Coord;
        SpawnRotation = spawn.Rotation;
        SessionId = GameServer.MushkingRoyaleSessionManager.GetSessionId();
        InstanceId = SessionId;
        GameServer.MushkingRoyaleSessionManager.AddSession(this);
    }
}
