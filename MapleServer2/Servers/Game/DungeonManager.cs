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

        public DungeonSession GetDungeonSession(int dungeonSessionId)
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

        public bool DungeonUsesMap(DungeonSession dungeonSession, FieldManager fieldManager, Player player)
        {
            player.PartyId
            Party Game
            //fieldmanager is the map the player is coming from/that is to be released
            if (dungeonSession != null) //is not null after entering dungeon via directory
            {
                //dungeon has no instance on map that is to be released
                if (dungeonSession.ContainsMap(fieldManager.MapId))  //map is a dungeon map
                //fieldmanager.mapId is the to be released map
                {
                    if (dungeonSession.ContainsMap(player.MapId)) //check map the player will travel: lobby to dungeon and dungeon to lobby
                    {
                        //also check for instance if player in group dungeon instance enters solo instance
                        return true;
                    }
                    else //travel destination is not a dungeon map
                    {
                        RemoveDungeonSession(player.DungeonSessionId); //leaving lobby or dungeonmap means dungeon session is finished
                        player.DungeonSessionId = -1;
                        return false;
                    }
                }
                else //map is not dungeon map //tria
                {
                    return false;
                }
            }
            return false; //if no dungeonsession, the instance is not in use.
        }

        public bool KeepInstanceAlive(DungeonSession dungeonSession, FieldManager fieldManager, Player player)
        {
            // left map is a dungeon map
            if (dungeonSession.ContainsMap(fieldManager.MapId))  //solo: 
            {

                return true;
            }
            return false;
        }
    }
}
