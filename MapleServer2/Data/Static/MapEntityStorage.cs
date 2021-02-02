using System;
using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MapEntityStorage
    {
        private static readonly Dictionary<int, List<NpcMetadata>> npcs = new Dictionary<int, List<NpcMetadata>>();
        private static readonly Dictionary<int, List<MapPortal>> portals = new Dictionary<int, List<MapPortal>>();
        private static readonly Dictionary<int, List<MapPlayerSpawn>> playerSpawns = new Dictionary<int, List<MapPlayerSpawn>>();
        private static readonly Dictionary<int, List<MapObject>> objects = new Dictionary<int, List<MapObject>>();
        private static readonly Dictionary<int, CoordS[]> boundingBox = new Dictionary<int, CoordS[]>();

        static MapEntityStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-map-entity-metadata");
            List<MapEntityMetadata> entities = Serializer.Deserialize<List<MapEntityMetadata>>(stream);
            foreach (MapEntityMetadata entity in entities)
            {
                List<NpcMetadata> npcList = new List<NpcMetadata>();
                foreach (NpcMetadata npc in entity.Npcs)
                {
                    NpcMetadata newNpc = NpcMetadataStorage.GetNpc(npc.Id);
                    newNpc.Rotation = npc.Rotation;
                    newNpc.Coord = npc.Coord;
                    npcList.Add(newNpc);
                }
                npcs.Add(entity.MapId, npcList);
                portals.Add(entity.MapId, entity.Portals);
                playerSpawns.Add(entity.MapId, entity.PlayerSpawns);
                objects.Add(entity.MapId, entity.Objects);
                boundingBox.Add(entity.MapId, new CoordS[] { entity.BoundingBox0, entity.BoundingBox1 });
            }
        }

        public static IEnumerable<NpcMetadata> GetNpcs(int mapId)
        {
            return npcs.GetValueOrDefault(mapId);
        }

        public static IEnumerable<MapPortal> GetPortals(int mapId)
        {
            return portals.GetValueOrDefault(mapId);
        }

        public static IEnumerable<MapPlayerSpawn> GetPlayerSpawns(int mapId)
        {
            return playerSpawns.GetValueOrDefault(mapId);
        }

        public static IEnumerable<MapObject> GetObjects(int mapId)
        {
            return objects.GetValueOrDefault(mapId);
        }

        public static MapPlayerSpawn GetRandomPlayerSpawn(int mapId)
        {
            List<MapPlayerSpawn> list = playerSpawns.GetValueOrDefault(mapId);
            return list?.Count > 0 ? list[new Random().Next(list.Count)] : null;
        }

        public static bool HasPortals(int mapId)
        {
            List<MapPortal> items = portals.GetValueOrDefault(mapId);
            return items?.Count > 0;
        }

        public static MapPortal GetFirstPortal(int mapId)
        {
            List<MapPortal> items = portals.GetValueOrDefault(mapId);
            return items?.Count > 0 ? items[0] : null;
        }

        public static CoordS[] GetBoundingBox(int mapId)
        {
            return boundingBox.GetValueOrDefault(mapId);
        }
    }
}
