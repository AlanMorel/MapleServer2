using System.Collections.Generic;
using MapleServer2.Data.Static;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public enum DungeonType
    {
        Solo,
        Group
    }

    public class DungeonSession
    {
        public int SessionId { get; }
        public int DungeonId { get; }
        public int DungeonInstanceId { get; }
        public List<int> DungeonMapIds { get; }
        public int DungeonLobbyId { get; }

        public DungeonType DungeonType { get; }

        public DungeonSession(int sessionId, int dungeonId, int dungeonInstanceId, DungeonType dungeonType)
        {
            DungeonType = dungeonType;
            SessionId = sessionId;
            DungeonId = dungeonId;
            DungeonInstanceId = dungeonInstanceId;
            DungeonMetadata dungeon = DungeonStorage.GetDungeonByDungeonId(dungeonId);
            DungeonMapIds = dungeon.FieldIds;
            DungeonLobbyId = dungeon.LobbyFieldId;
        }

        public bool ContainsMap(int mapId)
        {
            return DungeonMapIds.Contains(mapId) || DungeonLobbyId == mapId;
        }
    }
}
