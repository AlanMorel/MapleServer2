using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MapEntityStorage
{
    private static readonly Dictionary<int, List<MapNpc>> npcs = new();
    private static readonly Dictionary<int, List<MapPortal>> portals = new();
    private static readonly Dictionary<int, List<MapPlayerSpawn>> playerSpawns = new();
    private static readonly Dictionary<int, List<MapMobSpawn>> mobSpawns = new();
    private static readonly Dictionary<int, List<MapInteractObject>> interactObject = new();
    private static readonly Dictionary<int, CoordS[]> boundingBox = new();
    private static readonly Dictionary<int, List<CoordS>> healthSpot = new();
    private static readonly Dictionary<int, List<PatrolData>> PatrolDatas = new();
    private static readonly Dictionary<int, List<WayPoint>> WayPoints = new();
    private static readonly Dictionary<int, List<MapEventNpcSpawnPoint>> EventNpcSpawnPoints = new();
    private static readonly Dictionary<int, List<MapTriggerMesh>> TriggerMeshes = new();
    private static readonly Dictionary<int, List<MapTriggerEffect>> TriggerEffects = new();
    private static readonly Dictionary<int, List<MapTriggerCamera>> TriggerCameras = new();
    private static readonly Dictionary<int, List<MapTriggerBox>> TriggerBoxes = new();
    private static readonly Dictionary<int, List<MapTriggerActor>> TriggerActors = new();
    private static readonly Dictionary<int, List<MapTriggerCube>> TriggerCubes = new();
    private static readonly Dictionary<int, List<MapTriggerLadder>> TriggerLadders = new();
    private static readonly Dictionary<int, List<MapTriggerRope>> TriggerRopes = new();
    private static readonly Dictionary<int, List<MapTriggerSound>> TriggerSounds = new();
    private static readonly Dictionary<int, List<MapBreakableActorObject>> BreakableActors = new();
    private static readonly Dictionary<int, List<MapBreakableNifObject>> BreakableNifs = new();
    private static readonly Dictionary<int, List<MapVibrateObject>> VibrateObjects = new();
    private static readonly Dictionary<int, List<MapTriggerSkill>> TriggerSkills = new();
    private static readonly Dictionary<int, List<MapInteractObject>> InteractObjects = new();
    private static readonly Dictionary<int, List<MapWeaponObject>> WeaponObjects = new();
    private static readonly Dictionary<int, List<MapLiftableObject>> LiftableObjects = new();
    private static readonly Dictionary<int, List<MapLiftableTarget>> LiftableTargets = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-map-entity-metadata");
        List<MapEntityMetadata> entities = Serializer.Deserialize<List<MapEntityMetadata>>(stream);
        foreach (MapEntityMetadata entity in entities)
        {
            npcs.Add(entity.MapId, entity.Npcs);
            portals.Add(entity.MapId, entity.Portals);
            playerSpawns.Add(entity.MapId, entity.PlayerSpawns);
            mobSpawns.Add(entity.MapId, entity.MobSpawns);
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
            BreakableActors.Add(entity.MapId, entity.BreakableActors);
            BreakableNifs.Add(entity.MapId, entity.BreakableNifs);
            VibrateObjects.Add(entity.MapId, entity.VibrateObjects);
            TriggerSkills.Add(entity.MapId, entity.TriggerSkills);
            InteractObjects.Add(entity.MapId, entity.InteractObjects);
            WeaponObjects.Add(entity.MapId, entity.WeaponObjects);
            LiftableObjects.Add(entity.MapId, entity.LiftableObjects);
            LiftableTargets.Add(entity.MapId, entity.LiftableTargets);
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

    public static List<CoordS> GetHealingSpot(int mapId)
    {
        return healthSpot.GetValueOrDefault(mapId);
    }

    public static (PatrolData, List<WayPoint>) GetPatrolData(int mapId, string patrolDataName)
    {
        PatrolData patrolData = PatrolDatas.GetValueOrDefault(mapId)?.FirstOrDefault(x => x.Name == patrolDataName);
        if (patrolData is null)
        {
            return (null, null);
        }

        List<WayPoint> wayPoints = patrolData.WayPointIds.Select(wayPointId => GetWayPoint(mapId, wayPointId)).ToList();

        return (patrolData, wayPoints);
    }

    public static WayPoint GetWayPoint(int mapId, string id)
    {
        return WayPoints.GetValueOrDefault(mapId)?.FirstOrDefault(x => x.Id == id);
    }

    public static MapEventNpcSpawnPoint GetMapEventNpcSpawnPoint(int mapId, int spawnPointId)
    {
        return EventNpcSpawnPoints.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == spawnPointId);
    }

    public static List<MapTriggerMesh> GetTriggerMeshes(int mapId)
    {
        return TriggerMeshes.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerEffect> GetTriggerEffects(int mapId)
    {
        return TriggerEffects.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerCamera> GetTriggerCameras(int mapId)
    {
        return TriggerCameras.GetValueOrDefault(mapId);
    }

    public static MapTriggerBox GetTriggerBox(int mapId, int boxId)
    {
        return TriggerBoxes.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == boxId);
    }

    public static List<MapTriggerBox> GetTriggerBoxes(int mapId)
    {
        return TriggerBoxes.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerActor> GetTriggerActors(int mapId)
    {
        return TriggerActors.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerCube> GetTriggerCubes(int mapId)
    {
        return TriggerCubes.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerLadder> GetTriggerLadders(int mapId)
    {
        return TriggerLadders.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerRope> GetTriggerRopes(int mapId)
    {
        return TriggerRopes.GetValueOrDefault(mapId);
    }

    public static List<MapTriggerSound> GetTriggerSounds(int mapId)
    {
        return TriggerSounds.GetValueOrDefault(mapId);
    }

    public static List<MapBreakableActorObject> GetBreakableActors(int mapId)
    {
        return BreakableActors.GetValueOrDefault(mapId);
    }

    public static List<MapBreakableNifObject> GetBreakableNifs(int mapId)
    {
        return BreakableNifs.GetValueOrDefault(mapId);
    }

    public static bool IsVibrateObject(int mapId, string entityId)
    {
        return VibrateObjects.GetValueOrDefault(mapId).FirstOrDefault(x => x.EntityId == entityId) != default;
    }

    public static List<MapTriggerSkill> GetTriggerSkills(int mapId)
    {
        return TriggerSkills.GetValueOrDefault(mapId);
    }

    public static List<MapInteractObject> GetInteractObjects(int mapId)
    {
        return InteractObjects.GetValueOrDefault(mapId);
    }

    public static int GetWeaponObjectItemId(int mapId, CoordB coord)
    {
        MapWeaponObject weaponObject = WeaponObjects.GetValueOrDefault(mapId).FirstOrDefault(x => x.Coord == coord);
        if (weaponObject == null)
        {
            return 0;
        }

        Random random = RandomProvider.Get();
        int index = random.Next(weaponObject.WeaponItemIds.Count);
        return weaponObject.WeaponItemIds[index];
    }

    public static IEnumerable<MapLiftableObject> GetLiftablesObjects(int mapId) => LiftableObjects[mapId];

    public static IEnumerable<MapLiftableTarget> GetLiftablesTargets(int mapId) => LiftableTargets[mapId];
}
