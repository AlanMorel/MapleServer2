using System.Collections.Generic;
using MapleServer2.Data.Static;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public enum DungeonType
    {
        solo,
        group
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

        public void AddMember(Player player)
        {
        }

        public bool ContainsMap(int mapId)
        {
            if (DungeonMapIds.Contains(mapId) || DungeonLobbyId == mapId)
            {
                return true;
            }
            return false;
        }
    }
}
