using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class DungeonManager
    {
        public readonly Dictionary<int, DungeonSession> DungeonSessionList;
        private int SessionId = 0;
        private readonly List<int> RecyclableSessionIds;
        private readonly List<int> RecyclableMapInstanceIds;
        private int MapInstanceId = 0;

        public DungeonManager()
        {
            DungeonSessionList = new Dictionary<int, DungeonSession>(); //dungeonsessionid, dungeonsession
            RecyclableSessionIds = new List<int>();
            RecyclableMapInstanceIds = new List<int>(); //mapid
        }

        public int GetMapInstanceId()
        {
            if (RecyclableMapInstanceIds.Count > 0)
            {
                int mapInstanceId = RecyclableMapInstanceIds.First();
                RecyclableMapInstanceIds.Remove(mapInstanceId);
                return mapInstanceId;
            }
            return MapInstanceId++;
        }

        public DungeonSession CreateDungeonSession(int dungeonId, DungeonType dungeonType)
        {
            int dungeonSessionId = GetUniqueSessionId();
            int dungeonInstanceId = GetMapInstanceId();
            DungeonSession dungeonSession = new DungeonSession(dungeonSessionId, dungeonId, dungeonInstanceId, dungeonType);
            DungeonSessionList.Add(dungeonSessionId, dungeonSession);
            return dungeonSession;
        }

        public void RemoveDungeonSession(int dungeonSessionId)
        {
            if (!DungeonSessionList.ContainsKey(dungeonSessionId))
            {
                return;
            }
            int currentDungeonSessionId = DungeonSessionList[dungeonSessionId].SessionId;
            int currentDungeonInstanceId = DungeonSessionList[dungeonSessionId].DungeonInstanceId;
            RecyclableSessionIds.Add(currentDungeonSessionId);
            RecyclableMapInstanceIds.Add(currentDungeonInstanceId);
            DungeonSessionList.Remove(dungeonSessionId);
        }

        public int GetUniqueSessionId()
        {
            if (RecyclableSessionIds.Count > 0)
            {
                int sessionId = RecyclableSessionIds.First();
                RecyclableSessionIds.Remove(sessionId);
                return sessionId;
            }
            return SessionId++;
        }

        public DungeonSession GetDungeonSessionBySessionId(int dungeonSessionId)
        {
            return !DungeonSessionList.ContainsKey(dungeonSessionId) ? null : DungeonSessionList[dungeonSessionId];
        }

        public DungeonSession GetDungeonSessionByInstanceId(long instanceId)
        {
            return DungeonSessionList.FirstOrDefault(session => session.Value.DungeonInstanceId == instanceId).Value;
        }

        public bool IsDungeonUsingFieldInstance(FieldManager fieldManager, Player player) //alternatively this could be: IsFieldInstanceUsed in FieldManagerFactory
        {
            //fieldManager.MapId: left map that is to be destroyed
            //player.MapId: travel destination of the player
            DungeonSession currentDungeonSession = GetDungeonSessionByInstanceId(fieldManager.InstanceId);
            if (currentDungeonSession == null) //is not null after entering dungeon via directory
            {
                return false; //no dungeonsession -> the map is unused by dungeon
            }
            //check map that is left: 
            if (!currentDungeonSession.IsDungeonSessionMap(fieldManager.MapId)) //left map is not dungeon map e.g. tria
            {
                return false;
            }
            //travel destination is a dungeon map: lobby to dungeon or dungeon to lobby
            if (!currentDungeonSession.IsDungeonSessionMap(player.MapId))
            {
                return false;
            }
            //if travel destination is a dungeon it has to be the same instance or it does not pertain to the same DungeonSession.
            if (player.InstanceId != currentDungeonSession.DungeonInstanceId)
            {
                return false;
            }
            return true;
        }

        public void ResetDungeonSession(Player player, DungeonSession dungeonSession)
        {
            if (dungeonSession == null)
            {
                return;
            }
            RemoveDungeonSession(dungeonSession.SessionId);
            //if last player leaves lobby or dungeonmap -> destroy dungeonSession.
            if (dungeonSession.DungeonType == DungeonType.Group && player.PartyId != 0)
            {
                Party party = GameServer.PartyManager.GetPartyById(player.PartyId);
                party.DungeonSessionId = -1;
            }
            else
            {
                player.DungeonSessionId = -1;
            }
        }
    }
}
