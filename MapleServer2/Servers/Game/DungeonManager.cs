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

        public DungeonSession GetDungeonSessionByInstance(int instanceId)
        {
            //TODO: if no dungeonsession is found, return null
            return DungeonSessionList.FirstOrDefault(session => session.Value.DungeonInstanceId == instanceId).Value;
        }

        public bool DungeonUsesMap(DungeonSession dungeonSession, FieldManager fieldManager, Player player)
        {
            //fieldmanager is the map the player is coming from/that is to be released
            if (dungeonSession != null) //is not null after entering dungeon via directory
            {
                //check map that is left: fieldmanager.mapId is the to be released map
                if (dungeonSession.ContainsMap(fieldManager.MapId))  //map is a dungeon map
                {
                    //check map the player will travel to: lobby to dungeon and dungeon to lobby
                    if (dungeonSession.ContainsMap(player.MapId) && player.InstanceId == dungeonSession.DungeonInstanceId)
                    {
                        return true;
                    }
                    else //travel destination is not a dungeon map
                    {
                        RemoveDungeonSession(dungeonSession.SessionId); //leaving lobby or dungeonmap means dungeon session is finished, deletes session.
                        if (dungeonSession.DungeonType == DungeonType.group && player.PartyId != 0)
                        {
                            Party party = GameServer.PartyManager.GetPartyById(player.PartyId);
                            party.DungeonSessionId = -1;
                        }
                        else
                        {
                            player.DungeonSessionId = -1;
                        }
                        return false;
                    }
                }
                else //left map is not dungeon map e.g. tria
                {
                    return false;
                }
            }
            return false; //no dungeonsession = the map is unused by dungeon.
        }
    }
}
