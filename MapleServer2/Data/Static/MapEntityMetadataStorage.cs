using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Data.Static;

public static class MapEntityMetadataStorage
{
    private static readonly Dictionary<int, List<MapNpc>> Npcs = new();
    private static readonly Dictionary<int, List<MapPortal>> Portals = new();
    private static readonly Dictionary<int, List<MapPlayerSpawn>> PlayerSpawns = new();
    private static readonly Dictionary<int, List<MapMobSpawn>> MobSpawns = new();
    private static readonly Dictionary<int, List<MapInteractObject>> InteractObject = new();
    private static readonly Dictionary<int, CoordS[]> BoundingBox = new();
    private static readonly Dictionary<int, List<CoordS>> HealthSpot = new();
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
    private static readonly Dictionary<int, List<MapChestMetadata>> MapChests = new();
    private static readonly Dictionary<int, List<int>> AdBannerIds = new();

    public static void AddToStorage(int mapId, MapEntityMetadata entity)
    {
        Npcs.Add(mapId, entity.Npcs);
        Portals.Add(mapId, entity.Portals);
        PlayerSpawns.Add(mapId, entity.PlayerSpawns);
        MobSpawns.Add(mapId, entity.MobSpawns);
        BoundingBox.Add(mapId, new[]
        {
            entity.BoundingBox0, entity.BoundingBox1
        });
        HealthSpot.Add(mapId, entity.HealingSpot);
        PatrolDatas.Add(mapId, entity.PatrolDatas);
        WayPoints.Add(mapId, entity.WayPoints);
        EventNpcSpawnPoints.Add(mapId, entity.EventNpcSpawnPoints);
        TriggerMeshes.Add(mapId, entity.TriggerMeshes);
        TriggerEffects.Add(mapId, entity.TriggerEffects);
        TriggerCameras.Add(mapId, entity.TriggerCameras);
        TriggerBoxes.Add(mapId, entity.TriggerBoxes);
        TriggerActors.Add(mapId, entity.TriggerActors);
        TriggerCubes.Add(mapId, entity.TriggerCubes);
        TriggerLadders.Add(mapId, entity.TriggerLadders);
        TriggerRopes.Add(mapId, entity.TriggerRopes);
        TriggerSounds.Add(mapId, entity.TriggerSounds);
        BreakableActors.Add(mapId, entity.BreakableActors);
        BreakableNifs.Add(mapId, entity.BreakableNifs);
        VibrateObjects.Add(mapId, entity.VibrateObjects);
        TriggerSkills.Add(mapId, entity.TriggerSkills);
        InteractObjects.Add(mapId, entity.InteractObjects);
        WeaponObjects.Add(mapId, entity.WeaponObjects);
        LiftableObjects.Add(mapId, entity.LiftableObjects);
        LiftableTargets.Add(mapId, entity.LiftableTargets);
        MapChests.Add(mapId, entity.MapChests);
        AdBannerIds.Add(mapId, entity.AdBannerIds);
    }

    public static IEnumerable<MapNpc> GetNpcs(int mapId) => Npcs.GetValueOrDefault(mapId);

    public static IEnumerable<MapPortal> GetPortals(int mapId) => Portals.GetValueOrDefault(mapId);

    public static IEnumerable<MapPlayerSpawn> GetPlayerSpawns(int mapId) => PlayerSpawns.GetValueOrDefault(mapId);

    public static IEnumerable<MapMobSpawn> GetMobSpawns(int mapId) => MobSpawns.GetValueOrDefault(mapId);

    public static IEnumerable<MapInteractObject> GetInteractObject(int mapId) => InteractObject.GetValueOrDefault(mapId);

    public static MapPlayerSpawn GetRandomPlayerSpawn(int mapId)
    {
        List<MapPlayerSpawn> list = PlayerSpawns.GetValueOrDefault(mapId);
        return list?.Count > 0 ? list[Random.Shared.Next(list.Count)] : null;
    }

    public static bool HasPortals(int mapId)
    {
        List<MapPortal> items = Portals.GetValueOrDefault(mapId);
        return items?.Count > 0;
    }

    public static MapPortal GetFirstPortal(int mapId)
    {
        List<MapPortal> items = Portals.GetValueOrDefault(mapId);
        return items?.Count > 0 ? items[0] : null;
    }

    public static CoordS[] GetBoundingBox(int mapId) => BoundingBox.GetValueOrDefault(mapId);

    public static bool HasSafePortal(int mapId)
    {
        List<MapPortal> items = Portals.GetValueOrDefault(mapId).Where(x => x.TargetPortalId != 0).ToList();
        return items.Count != 0;
    }

    public static List<CoordS> GetHealingSpot(int mapId) => HealthSpot.GetValueOrDefault(mapId);

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

    public static WayPoint GetWayPoint(int mapId, string id) => WayPoints.GetValueOrDefault(mapId)?.FirstOrDefault(x => x.Id == id);

    public static MapEventNpcSpawnPoint GetMapEventNpcSpawnPoint(int mapId, int spawnPointId) =>
        EventNpcSpawnPoints.GetValueOrDefault(mapId).FirstOrDefault(x => x.Id == spawnPointId);

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

    public static List<MapTriggerSkill> GetTriggerSkills(int mapId) => TriggerSkills.GetValueOrDefault(mapId);

    public static List<MapBreakableActorObject> GetBreakableActors(int mapId) => BreakableActors.GetValueOrDefault(mapId);

    public static List<MapBreakableNifObject> GetBreakableNifs(int mapId) => BreakableNifs.GetValueOrDefault(mapId);

    public static List<MapVibrateObject> GetVibrateObjects(int mapId) => VibrateObjects.GetValueOrDefault(mapId);

    public static bool IsVibrateObject(int mapId, string entityId) =>
        VibrateObjects.GetValueOrDefault(mapId).FirstOrDefault(x => x.EntityId == entityId) != default;

    public static List<MapInteractObject> GetInteractObjects(int mapId) => InteractObjects.GetValueOrDefault(mapId);

    public static int GetWeaponObjectItemId(int mapId, CoordB coord)
    {
        MapWeaponObject weaponObject = WeaponObjects.GetValueOrDefault(mapId).FirstOrDefault(x => x.Coord == coord);
        if (weaponObject == null)
        {
            return 0;
        }

        Random random = Random.Shared;
        int index = random.Next(weaponObject.WeaponItemIds.Count);
        return weaponObject.WeaponItemIds[index];
    }

    public static IEnumerable<MapLiftableObject> GetLiftablesObjects(int mapId) => LiftableObjects.GetValueOrDefault(mapId);

    public static IEnumerable<MapLiftableTarget> GetLiftablesTargets(int mapId) => LiftableTargets.GetValueOrDefault(mapId);

    public static IEnumerable<MapChestMetadata> GetMapChests(int mapId) => MapChests.GetValueOrDefault(mapId);

    public static Dictionary<int, List<int>> GetAdBannerIds() => AdBannerIds.Where(x => x.Value.Count > 0).ToDictionary(x => x.Key, x => x.Value);
}
