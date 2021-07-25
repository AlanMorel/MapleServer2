using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Tools;
using Maple2Storage.Types;
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
        private static readonly Dictionary<int, List<MapMobSpawn>> mobSpawns = new Dictionary<int, List<MapMobSpawn>>();
        private static readonly Dictionary<int, List<MapObject>> objects = new Dictionary<int, List<MapObject>>();
        private static readonly Dictionary<int, List<MapInteractObject>> interactObject = new Dictionary<int, List<MapInteractObject>>();
        private static readonly Dictionary<int, CoordS[]> boundingBox = new Dictionary<int, CoordS[]>();
        private static readonly Dictionary<int, List<CoordS>> healthSpot = new Dictionary<int, List<CoordS>>();
        private static readonly Dictionary<int, List<TriggerObject>> TriggerObjects = new Dictionary<int, List<TriggerObject>>();

        static MapEntityStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-map-entity-metadata");
            List<MapEntityMetadata> entities = Serializer.Deserialize<List<MapEntityMetadata>>(stream);
            foreach (MapEntityMetadata entity in entities)
            {
                npcs.Add(entity.MapId, entity.Npcs);
                portals.Add(entity.MapId, entity.Portals);
                playerSpawns.Add(entity.MapId, entity.PlayerSpawns);
                mobSpawns.Add(entity.MapId, entity.MobSpawns);
                interactObject.Add(entity.MapId, entity.InteractObjects);
                objects.Add(entity.MapId, entity.Objects);
                boundingBox.Add(entity.MapId, new CoordS[] { entity.BoundingBox0, entity.BoundingBox1 });
                healthSpot.Add(entity.MapId, entity.HealingSpot);
                TriggerObjects.Add(entity.MapId, entity.TriggerObjects);
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

        public static IEnumerable<MapMobSpawn> GetMobSpawns(int mapId)
        {
            return mobSpawns.GetValueOrDefault(mapId);
        }

        public static IEnumerable<MapObject> GetObjects(int mapId)
        {
            return objects.GetValueOrDefault(mapId);
        }

        public static IEnumerable<MapInteractObject> GetInteractObject(int mapId)
        {
            return interactObject.GetValueOrDefault(mapId);
        }

        public static MapPlayerSpawn GetRandomPlayerSpawn(int mapId)
        {
            List<MapPlayerSpawn> list = playerSpawns.GetValueOrDefault(mapId);
            return list?.Count > 0 ? list[RandomProvider.Get().Next(list.Count)] : null;
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

        public static bool HasSafePortal(int mapId)
        {
            List<MapPortal> items = portals.GetValueOrDefault(mapId).Where(x => x.TargetPortalId != 0).ToList();
            return items.Count != 0;
        }

        public static bool HasHealingSpot(int mapId)
        {
            return healthSpot.GetValueOrDefault(mapId).Count != 0;
        }

        public static List<CoordS> GetHealingSpot(int mapId)
        {
            return healthSpot.GetValueOrDefault(mapId);
        }

        public static List<TriggerObject> GetTriggerObjects(int mapId)
        {
            return TriggerObjects.GetValueOrDefault(mapId);
        }
    }
}
