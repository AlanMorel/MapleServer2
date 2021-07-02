using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaplePacketLib2.Tools;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Constants;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;
using Maple2Storage.Types;
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
            //possibly add players to DungeonSession
            //player.DungeonSessionId = SessionId;
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
