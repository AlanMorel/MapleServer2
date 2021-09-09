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
        private static readonly Dictionary<int, List<PatrolData>> PatrolDatas = new Dictionary<int, List<PatrolData>>();
        private static readonly Dictionary<int, List<WayPoint>> WayPoints = new Dictionary<int, List<WayPoint>>();
        private static readonly Dictionary<int, List<MapEventNpcSpawnPoint>> EventNpcSpawnPoints = new Dictionary<int, List<MapEventNpcSpawnPoint>>();
        private static readonly Dictionary<int, List<MapTriggerMesh>> TriggerMeshes = new Dictionary<int, List<MapTriggerMesh>>();
        private static readonly Dictionary<int, List<MapTriggerEffect>> TriggerEffects = new Dictionary<int, List<MapTriggerEffect>>();
        private static readonly Dictionary<int, List<MapTriggerCamera>> TriggerCameras = new Dictionary<int, List<MapTriggerCamera>>();
        private static readonly Dictionary<int, List<MapTriggerBox>> TriggerBoxes = new Dictionary<int, List<MapTriggerBox>>();
        private static readonly Dictionary<int, List<MapTriggerActor>> TriggerActors = new Dictionary<int, List<MapTriggerActor>>();
        private static readonly Dictionary<int, List<MapTriggerCube>> TriggerCubes = new Dictionary<int, List<MapTriggerCube>>();
        private static readonly Dictionary<int, List<MapTriggerLadder>> TriggerLadders = new Dictionary<int, List<MapTriggerLadder>>();
        private static readonly Dictionary<int, List<MapTriggerRope>> TriggerRopes = new Dictionary<int, List<MapTriggerRope>>();
        private static readonly Dictionary<int, List<MapTriggerSound>> TriggerSounds = new Dictionary<int, List<MapTriggerSound>>();

        public static void Init()
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
                PatrolDatas.Add(entity.MapId, entity.PatrolDatas);
                WayPoints.Add(entity.MapId, entity.WayPoints);
                EventNpcSpawnPoints.Add(entity.MapId, entity.EventNpcSpawnPoints);
                TriggerMeshes.Add(entity.MapId, entity.TriggerMeshes);
                TriggerEffects.Add(entity.MapId, entity.TriggerEffects);
                TriggerCameras.Add(entity.MapId, entity.TriggerCameras);
                TriggerBoxes.Add(entity.MapId, entity.TriggerBoxes);
                TriggerActors.Add(entity.MapId, entity.TriggerActors);
                TriggerCubes.Add(entity.MapId, entity.TriggerCubes);
                TriggerLadders.Add(entity.MapId, entity.TriggerLadders);
                TriggerRopes.Add(entity.MapId, entity.TriggerRopes);
                TriggerSounds.Add(entity.MapId, entity.TriggerSounds);
            }
        }

        public static IEnumerable<MapNpc> GetNpcs(int mapId) => npcs.GetValueOrDefault(mapId);

        public static IEnumerable<MapPortal> GetPortals(int mapId) => portals.GetValueOrDefault(mapId);

        public static IEnumerable<MapPlayerSpawn> GetPlayerSpawns(int mapId) => playerSpawns.GetValueOrDefault(mapId);

        public static IEnumerable<MapMobSpawn> GetMobSpawns(int mapId) => mobSpawns.GetValueOrDefault(mapId);

        public static IEnumerable<MapObject> GetObjects(int mapId) => objects.GetValueOrDefault(mapId);

        public static IEnumerable<MapInteractObject> GetInteractObject(int mapId) => interactObject.GetValueOrDefault(mapId);

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

        public static CoordS[] GetBoundingBox(int mapId) => boundingBox.GetValueOrDefault(mapId);

        public static bool HasSafePortal(int mapId)
        {
            List<MapPortal> items = portals.GetValueOrDefault(mapId).Where(x => x.TargetPortalId != 0).ToList();
            return items.Count != 0;
        }

        public static bool HasHealingSpot(int mapId) => healthSpot.GetValueOrDefault(mapId).Count != 0;

        public static List<CoordS> GetHealingSpot(int mapId) => healthSpot.GetValueOrDefault(mapId);

        public static PatrolData GetPatrolData(int mapId, string patrolDataName) => PatrolDatas.GetValueOrDefault(mapId).FirstOrDefault(x => x.Name == patrolDataName);

        public static WayPoint GetWayPoint(int mapId, string id) => WayPoints.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == id);

        public static MapEventNpcSpawnPoint GetMapEventNpcSpawnPoint(int mapId, int spawnPointId) => EventNpcSpawnPoints.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == spawnPointId);

        public static List<MapTriggerMesh> GetTriggerMeshes(int mapId) => TriggerMeshes.GetValueOrDefault(mapId);

        public static List<MapTriggerEffect> GetTriggerEffects(int mapId) => TriggerEffects.GetValueOrDefault(mapId);

        public static List<MapTriggerCamera> GetTriggerCameras(int mapId) => TriggerCameras.GetValueOrDefault(mapId);

        public static MapTriggerBox GetTriggerBox(int mapId, int boxId) => TriggerBoxes.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == boxId);

        public static List<MapTriggerBox> GetTriggerBoxes(int mapId) => TriggerBoxes.GetValueOrDefault(mapId);

        public static List<MapTriggerActor> GetTriggerActors(int mapId) => TriggerActors.GetValueOrDefault(mapId);

        public static List<MapTriggerCube> GetTriggerCubes(int mapId) => TriggerCubes.GetValueOrDefault(mapId);

        public static List<MapTriggerLadder> GetTriggerLadders(int mapId) => TriggerLadders.GetValueOrDefault(mapId);

        public static List<MapTriggerRope> GetTriggerRopes(int mapId) => TriggerRopes.GetValueOrDefault(mapId);

        public static List<MapTriggerSound> GetTriggerSounds(int mapId) => TriggerSounds.GetValueOrDefault(mapId);
    }
}
