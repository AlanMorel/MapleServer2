using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class DungeonManager
    {
        public readonly Dictionary<int, DungeonSession> DungeonSessionList;
        private int SessionId = 0;
        private List<int> RecyclableSessionIds;
        private List<int> RecyclableMapInstanceIds;
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

        //int mapInstanceId = RecyclableMapInstanceIds[mapId].First();
        //RecyclableMapInstanceIds[mapId].Remove(mapInstanceId);
        //        if (RecyclableMapInstanceIds[mapId].Count == 0)
        //        {
        //            RecyclableMapInstanceIds.Remove(mapId);
        //        }
        //        return mapInstanceId;

        public DungeonSession CreateDungeonSession(int dungeonId)
        {
            int dungeonSessionId = GetUniqueSessionId();
            DungeonMetadata dungeon = DungeonStorage.GetDungeonByDungeonId(dungeonId);
            int dungeonInstanceId = GetMapInstanceId();
            DungeonSession dungeonSession = new DungeonSession(dungeonSessionId, dungeonId, dungeonInstanceId);
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

        public DungeonSession GetDungeonSession(Player player, int dungeonSessionId)
        {
            //set dungeon of session id to x on swich dungeon
            //if dungeonsession exists but id != dungeonid call dungeonid change
            //update dungeonid or handle it somewhere else
            if (!DungeonSessionList.ContainsKey(dungeonSessionId))
            {
                return null;
            }
            return DungeonSessionList[dungeonSessionId];
        }
    }
}
