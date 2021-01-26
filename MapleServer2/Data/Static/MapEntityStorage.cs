using System;
using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MapEntityStorage
    {
        private static readonly Dictionary<int, List<MapNpc>> npcs = new Dictionary<int, List<MapNpc>>();
        private static readonly Dictionary<int, List<MapPortal>> portals = new Dictionary<int, List<MapPortal>>();
        private static readonly Dictionary<int, List<MapPlayerSpawn>> playerSpawns = new Dictionary<int, List<MapPlayerSpawn>>();
        private static readonly Dictionary<int, List<MapObject>> objects = new Dictionary<int, List<MapObject>>();

        static MapEntityStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-map-entity-metadata");
            List<MapEntityMetadata> entities = Serializer.Deserialize<List<MapEntityMetadata>>(stream);
            foreach (MapEntityMetadata entity in entities)
            {
                npcs.Add(entity.MapId, entity.Npcs);
                portals.Add(entity.MapId, entity.Portals);
                playerSpawns.Add(entity.MapId, entity.PlayerSpawns);
                objects.Add(entity.MapId, entity.Objects);
            }
        }

        public static IEnumerable<MapNpc> GetNpcs(int mapId)
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
    }
}
