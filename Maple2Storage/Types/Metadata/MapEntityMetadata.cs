using System.Xml.Serialization;
using Maple2Storage.Enums;
using ProtoBuf;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MapEntityMetadata
{
    [XmlElement(Order = 1)]
    public readonly int MapId;
    [XmlElement(Order = 2)]
    public readonly List<MapNpc> Npcs;
    [XmlElement(Order = 3)]
    public readonly List<MapPortal> Portals;
    [XmlElement(Order = 4)]
    public readonly List<MapPlayerSpawn> PlayerSpawns;
    [XmlElement(Order = 5)]
    public readonly List<MapMobSpawn> MobSpawns;
    [XmlElement(Order = 6)]
    public readonly List<MapWeaponObject> WeaponObjects;
    [XmlElement(Order = 7)]
    public CoordS BoundingBox0;
    [XmlElement(Order = 8)]
    public CoordS BoundingBox1;
    [XmlElement(Order = 9)]
    public List<CoordS> HealingSpot;
    [XmlElement(Order = 10)]
    public List<PatrolData> PatrolDatas;
    [XmlElement(Order = 11)]
    public List<WayPoint> WayPoints;
    [XmlElement(Order = 12)]
    public readonly List<MapTriggerMesh> TriggerMeshes;
    [XmlElement(Order = 13)]
    public readonly List<MapTriggerEffect> TriggerEffects;
    [XmlElement(Order = 14)]
    public readonly List<MapTriggerCamera> TriggerCameras;
    [XmlElement(Order = 15)]
    public readonly List<MapTriggerBox> TriggerBoxes;
    [XmlElement(Order = 16)]
    public readonly List<MapTriggerLadder> TriggerLadders;
    [XmlElement(Order = 17)]
    public readonly List<MapEventNpcSpawnPoint> EventNpcSpawnPoints;
    [XmlElement(Order = 18)]
    public readonly List<MapTriggerActor> TriggerActors;
    [XmlElement(Order = 19)]
    public readonly List<MapTriggerCube> TriggerCubes;
    [XmlElement(Order = 20)]
    public readonly List<MapTriggerSound> TriggerSounds;
    [XmlElement(Order = 21)]
    public readonly List<MapTriggerRope> TriggerRopes;
    [XmlElement(Order = 22)]
    public readonly List<MapBreakableActorObject> BreakableActors;
    [XmlElement(Order = 23)]
    public readonly List<MapBreakableNifObject> BreakableNifs;
    [XmlElement(Order = 24)]
    public readonly List<MapVibrateObject> VibrateObjects;
    [XmlElement(Order = 25)]
    public readonly List<MapTriggerSkill> TriggerSkills;
    [XmlElement(Order = 26)]
    public readonly List<MapInteractObject> InteractObjects;
    [XmlElement(Order = 27)]
    public readonly List<MapLiftableObject> LiftableObjects;

    // Required for deserialization
    public MapEntityMetadata()
    {
        PlayerSpawns = new();
        MobSpawns = new();
        Npcs = new();
        Portals = new();
        WeaponObjects = new();
        HealingSpot = new();
        PatrolDatas = new();
        WayPoints = new();
        TriggerMeshes = new();
        TriggerEffects = new();
        TriggerCameras = new();
        TriggerBoxes = new();
        TriggerLadders = new();
        EventNpcSpawnPoints = new();
        TriggerActors = new();
        TriggerCubes = new();
        TriggerSounds = new();
        TriggerRopes = new();
        BreakableActors = new();
        BreakableNifs = new();
        VibrateObjects = new();
        TriggerSkills = new();
        InteractObjects = new();
        LiftableObjects = new();
    }

    public MapEntityMetadata(int mapId)
    {
        MapId = mapId;
        PlayerSpawns = new();
        MobSpawns = new();
        Npcs = new();
        Portals = new();
        WeaponObjects = new();
        HealingSpot = new();
        PatrolDatas = new();
        WayPoints = new();
        TriggerMeshes = new();
        TriggerEffects = new();
        TriggerCameras = new();
        TriggerBoxes = new();
        TriggerLadders = new();
        EventNpcSpawnPoints = new();
        TriggerActors = new();
        TriggerCubes = new();
        TriggerSounds = new();
        TriggerRopes = new();
        BreakableActors = new();
        BreakableNifs = new();
        VibrateObjects = new();
        TriggerSkills = new();
        InteractObjects = new();
        LiftableObjects = new();
    }

    public override string ToString()
    {
        return $"MapEntityMetadata(Id:{MapId},PlayerSpawns:{string.Join(",", PlayerSpawns)},MobSpawns:{string.Join(",", MobSpawns)},Npcs:{string.Join(",", Npcs)},Portals:{string.Join(",", Portals)},Objects:{string.Join(",", WeaponObjects)})";
    }
}
[XmlType]
public class MapWeaponObject
{
    [XmlElement(Order = 1)]
    public readonly CoordB Coord;
    [XmlElement(Order = 2)]
    public readonly List<int> WeaponItemIds;

    // Required for deserialization
    public MapWeaponObject()
    {
        WeaponItemIds = new();
    }

    public MapWeaponObject(CoordB coord, List<int> weaponIds)
    {
        Coord = coord;
        WeaponItemIds = weaponIds;
    }

    public override string ToString()
    {
        return $"MapObject(Coord:{Coord},WeaponId:{WeaponItemIds})";
    }
}
[XmlType]
public class MapNpc
{
    [XmlElement(Order = 1)]
    public readonly int Id;
    [XmlElement(Order = 2)]
    public string ModelName;
    [XmlElement(Order = 3)]
    public string InstanceName;
    [XmlElement(Order = 4)]
    public readonly CoordS Coord;
    [XmlElement(Order = 5)]
    public readonly CoordS Rotation;
    [XmlElement(Order = 6)]
    public string PatrolDataUuid = "00000000-0000-0000-0000-000000000000";
    [XmlElement(Order = 7)]
    public bool IsSpawnOnFieldCreate = false;
    [XmlElement(Order = 8)]
    public bool IsDayDie = false;
    [XmlElement(Order = 9)]
    public bool IsNightDie = false;


    // Required for deserialization
    public MapNpc() { }

    public MapNpc(int id, string modelName, string instanceName, CoordS coord, CoordS rotation, bool isSpawnOnFieldCreate, bool isDayDie, bool isNightDie)
    {
        Id = id;
        ModelName = modelName;
        InstanceName = instanceName;
        Coord = coord;
        Rotation = rotation;
        IsSpawnOnFieldCreate = isSpawnOnFieldCreate;
        IsDayDie = isDayDie;
        IsNightDie = isNightDie;
    }

    public override string ToString()
    {
        return $"MapNpc(Id:{Id},ModelName:{ModelName},Rotation:{Rotation},Coord:{Coord})";
    }

    // TODO: Add other methods which idenntify the Type of NPC (always, event, etc)
}
[XmlType]
public class MapPortal
{
    [XmlElement(Order = 1)]
    public readonly int Id;
    [XmlElement(Order = 2)]
    public readonly string Name;
    [XmlElement(Order = 3)]
    public readonly bool Enable;
    [XmlElement(Order = 4)]
    public readonly bool IsVisible;
    [XmlElement(Order = 5)]
    public readonly bool MinimapVisible;
    [XmlElement(Order = 6)]
    public readonly int Target;
    [XmlElement(Order = 7)]
    public readonly CoordS Coord;
    [XmlElement(Order = 8)]
    public readonly CoordS Rotation;
    [XmlElement(Order = 9)]
    public readonly int TargetPortalId;
    [XmlElement(Order = 10)]
    public readonly PortalTypes PortalType;
    [XmlElement(Order = 11)]
    public readonly int TriggerId;

    // Required for deserialization
    public MapPortal() { }

    public MapPortal(int id, string name, bool enable, bool isVisible, bool minimapVisible, int target, CoordS coord, CoordS rotation, int targetPortalId, PortalTypes portalType, int triggerId = 0)
    {
        Id = id;
        Name = name;
        Enable = enable;
        IsVisible = isVisible;
        MinimapVisible = minimapVisible;
        Target = target;
        Coord = coord;
        Rotation = rotation;
        TargetPortalId = targetPortalId;
        PortalType = portalType;
        TriggerId = triggerId;
    }

    public override string ToString()
    {
        return $"MapPortal(Id:{Id},String:{Name},Enable:{Enable},IsVisible:{IsVisible},MinimapVisible:{MinimapVisible}," +
            $"Target:{Target},Rotation:{Rotation},Coord:{Coord},TargetPortalId:{TargetPortalId}, PortalType:{PortalType},TriggerId:{TriggerId})";
    }
}
[XmlType]
public class MapPlayerSpawn
{
    [XmlElement(Order = 1)]
    public readonly CoordS Coord;
    [XmlElement(Order = 2)]
    public readonly CoordS Rotation;

    // Required for deserialization
    public MapPlayerSpawn() { }

    public MapPlayerSpawn(CoordS coord, CoordS rotation)
    {
        Coord = coord;
        Rotation = rotation;
    }

    public override string ToString()
    {
        return $"MapPlayerSpawn(Coord:{Coord},Rotation:{Rotation})";
    }
}
[XmlType]
public class MapMobSpawn
{
    [XmlElement(Order = 1)]
    public readonly int Id;
    [XmlElement(Order = 2)]
    public readonly CoordS Coord;
    [XmlElement(Order = 3)]
    public readonly int NpcCount;
    [XmlElement(Order = 4)]
    public readonly List<int> NpcList;
    [XmlElement(Order = 5)]
    public readonly int SpawnRadius;
    [XmlElement(Order = 6)]
    public readonly SpawnMetadata SpawnData;

    public MapMobSpawn() { }

    public MapMobSpawn(int id, CoordS coord, int npcCount, List<int> npcList, int spawnRadius, SpawnMetadata spawnMetadata = null)
    {
        Id = id;
        Coord = coord;
        NpcCount = npcCount;
        NpcList = npcList;
        SpawnRadius = spawnRadius;
        SpawnData = spawnMetadata;
    }

    public override string ToString()
    {
        return $"MapMobSpawn(Id:{Id},Coord:{Coord},NpcCount:{NpcCount},NpcList{NpcList},SpawnRadius:{SpawnRadius})";
    }
}
[XmlType]
public class SpawnMetadata
{
    [XmlElement(Order = 1)]
    public readonly int Difficulty;
    [XmlElement(Order = 2)]
    public readonly int MinDifficulty;
    [XmlElement(Order = 3)]
    public readonly string[] Tags;
    [XmlElement(Order = 4)]
    public readonly int SpawnTime;
    [XmlElement(Order = 5)]
    public readonly int Population;
    [XmlElement(Order = 6)]
    public readonly bool IsPetSpawn;

    public SpawnMetadata() { }

    public SpawnMetadata(string[] tags, int population, int spawnTime, int difficulty, int minDifficulty = 1, bool isPetSpawn = false)
    {
        Tags = tags;
        Population = population;
        SpawnTime = spawnTime;
        Difficulty = difficulty;
        MinDifficulty = minDifficulty;
        IsPetSpawn = isPetSpawn;
    }
}
[XmlType]
public class PatrolData
{
    [XmlElement(Order = 1)]
    public string Name;
    [XmlElement(Order = 2)]
    public List<string> WayPointIds;
    [XmlElement(Order = 3)]
    public int PatrolSpeed;
    [XmlElement(Order = 4)]
    public bool IsLoop;
    [XmlElement(Order = 5)]
    public bool IsAirWayPoint;
    [XmlElement(Order = 6)]
    public List<string> ArriveAnimations;
    [XmlElement(Order = 7)]
    public List<string> ApproachAnimations;
    [XmlElement(Order = 8)]
    public List<int> ArriveAnimationTimes;

    public PatrolData() { }

    public PatrolData(string name, List<string> wayPointIds, int patrolSpeed, bool isLoop, bool isAirWayPoint, List<string> arriveAnimations, List<string> approachAnimations, List<int> arriveAnimationTimes)
    {
        Name = name;
        WayPointIds = wayPointIds;
        PatrolSpeed = patrolSpeed;
        IsLoop = isLoop;
        IsAirWayPoint = isAirWayPoint;
        ArriveAnimations = arriveAnimations;
        ApproachAnimations = approachAnimations;
        ArriveAnimationTimes = arriveAnimationTimes;
    }

    public override string ToString()
    {
        return $"PatrolData(Name:{Name},PatrolSpeed:{PatrolSpeed},IsLoop:{IsLoop},IsAirWayPoint:{IsAirWayPoint})";
    }
}
[XmlType]
public class WayPoint
{
    [XmlElement(Order = 1)]
    public string Id;
    [XmlElement(Order = 2)]
    public bool IsVisible;
    [XmlElement(Order = 3)]
    public CoordS Position;
    [XmlElement(Order = 4)]
    public CoordS Rotation;

    public WayPoint() { }

    public WayPoint(string id, bool isVisible, CoordS position, CoordS rotation)
    {
        Id = id;
        IsVisible = isVisible;
        Position = position;
        Rotation = rotation;
    }

    public override string ToString()
    {
        return $"PatrolData(Id:{Id},IsVisible:{IsVisible},Position:{Position},Rotation:{Rotation})";
    }
}
[XmlType]
[ProtoContract]
public class MapEventNpcSpawnPoint
{
    [ProtoMember(1)]
    [XmlElement(Order = 1)]
    public int Id;
    [ProtoMember(2)]
    [XmlElement(Order = 2)]
    public uint Count;
    [ProtoMember(3)]
    [XmlElement(Order = 3)]
    public List<string> NpcIds;
    [ProtoMember(4)]
    [XmlElement(Order = 4)]
    public string SpawnAnimation;
    [ProtoMember(5)]
    [XmlElement(Order = 5)]
    public float SpawnRadius;
    [ProtoMember(6)]
    [XmlElement(Order = 6)]
    public CoordF Position;
    [ProtoMember(7)]
    [XmlElement(Order = 7)]
    public CoordF Rotation;

    public MapEventNpcSpawnPoint() { }

    public MapEventNpcSpawnPoint(int id, uint count, List<string> npcIds, string spawnAnimation, float spawnRadius, CoordF position, CoordF rotation)
    {
        Id = id;
        Count = count;
        NpcIds = new(npcIds);
        SpawnAnimation = spawnAnimation;
        SpawnRadius = spawnRadius;
        Position = position;
        Rotation = rotation;
    }
}
[ProtoContract] [ProtoInclude(50, typeof(MapTriggerMesh))]
[ProtoInclude(51, typeof(MapTriggerEffect))]
[ProtoInclude(52, typeof(MapTriggerCamera))]
[ProtoInclude(53, typeof(MapTriggerBox))]
[ProtoInclude(54, typeof(MapTriggerLadder))]
[ProtoInclude(55, typeof(MapTriggerActor))]
[ProtoInclude(56, typeof(MapTriggerCube))]
[ProtoInclude(57, typeof(MapTriggerSound))]
[ProtoInclude(58, typeof(MapTriggerRope))]
[ProtoInclude(59, typeof(MapTriggerSkill))]
public class MapTriggerObject
{
    [ProtoMember(8)]
    public int Id;

    public MapTriggerObject() { }

    public MapTriggerObject(int id)
    {
        Id = id;
    }
}
[ProtoContract]
public class MapTriggerMesh : MapTriggerObject
{
    [ProtoMember(9)]
    public bool IsVisible;

    public MapTriggerMesh(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }

    private MapTriggerMesh() : base() { }
}
[ProtoContract]
public class MapTriggerEffect : MapTriggerObject
{
    [ProtoMember(10)]
    public bool IsVisible;

    public MapTriggerEffect(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }

    private MapTriggerEffect() : base() { }
}
[ProtoContract]
public class MapTriggerCamera : MapTriggerObject
{
    [ProtoMember(11)]
    public bool IsEnabled;

    public MapTriggerCamera(int id, bool isEnabled) : base(id)
    {
        IsEnabled = isEnabled;
    }

    private MapTriggerCamera() : base() { }
}
[ProtoContract]
public class MapTriggerBox : MapTriggerObject
{
    [ProtoMember(12)]
    public CoordF Position;
    [ProtoMember(13)]
    public CoordF Dimension;

    public MapTriggerBox(int id, CoordF position, CoordF dimension) : base(id)
    {
        Position = position;
        Dimension = dimension;
    }

    private MapTriggerBox() : base() { }
}
[ProtoContract]
public class MapTriggerLadder : MapTriggerObject
{
    [ProtoMember(14)]
    public bool IsVisible;

    public MapTriggerLadder(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }

    private MapTriggerLadder() : base() { }
}
[ProtoContract]
public class MapTriggerActor : MapTriggerObject
{
    [ProtoMember(15)]
    public bool IsVisible;
    [ProtoMember(16)]
    public string InitialSequence;

    public MapTriggerActor(int id, bool isVisible, string initialSequence) : base(id)
    {
        IsVisible = isVisible;
        InitialSequence = initialSequence;
    }

    private MapTriggerActor() : base() { }
}
[ProtoContract]
public class MapTriggerCube : MapTriggerObject
{
    [ProtoMember(17)]
    public bool IsVisible;

    public MapTriggerCube(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }

    public MapTriggerCube() : base() { }
}
[ProtoContract]
public class MapTriggerSound : MapTriggerObject
{
    [ProtoMember(18)]
    public bool IsEnabled;

    public MapTriggerSound(int id, bool enabled) : base(id)
    {
        IsEnabled = enabled;
    }

    public MapTriggerSound() : base() { }
}
[ProtoContract]
public class MapTriggerRope : MapTriggerObject
{
    [ProtoMember(19)]
    public bool IsVisible;

    public MapTriggerRope(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }

    private MapTriggerRope() : base() { }
}
[ProtoContract] [ProtoInclude(20, typeof(MapBreakableNifObject))]
[ProtoInclude(21, typeof(MapBreakableActorObject))]
public class MapBreakableObject
{
    [ProtoMember(22)]
    public string EntityId;
    [ProtoMember(23)]
    public bool IsEnabled;
    [ProtoMember(24)]
    public int HideDuration;
    [ProtoMember(25)]
    public int ResetDuration;

    public MapBreakableObject() { }

    public MapBreakableObject(string entityId, bool isEnabled, int hideDuration, int resetDuration)
    {
        EntityId = entityId;
        IsEnabled = isEnabled;
        HideDuration = hideDuration;
        ResetDuration = resetDuration;
    }
}
[ProtoContract]
public class MapBreakableNifObject : MapBreakableObject
{
    [ProtoMember(26)]
    public int TriggerId;

    public MapBreakableNifObject() { }

    public MapBreakableNifObject(string id, bool isEnabled, int triggerId, int hideDuration, int resetDuration) : base(id, isEnabled, hideDuration, resetDuration)
    {
        TriggerId = triggerId;
    }
}
[ProtoContract]
public class MapTriggerSkill : MapTriggerObject
{
    [ProtoMember(27)]
    public CoordF Position;
    [ProtoMember(28)]
    public byte Count;
    [ProtoMember(29)]
    public short SkillLevel;
    [ProtoMember(30)]
    public int SkillId;

    public MapTriggerSkill(int id, int skillId, short skillLevel, byte count, CoordF position) : base(id)
    {
        Position = position;
        Count = count;
        SkillLevel = skillLevel;
        SkillId = skillId;
    }

    public MapTriggerSkill() : base() { }
}
[ProtoContract]
public class MapBreakableActorObject : MapBreakableObject
{
    public MapBreakableActorObject() { }

    public MapBreakableActorObject(string id, bool isEnabled, int hideDuration, int resetDuration) : base(id, isEnabled, hideDuration, resetDuration) { }
}
[XmlType]
public class MapVibrateObject
{
    [XmlElement(Order = 1)]
    public string EntityId;

    public MapVibrateObject() { }

    public MapVibrateObject(string id)
    {
        EntityId = id;
    }
}
[XmlType]
public class MapInteractObject
{
    [XmlElement(Order = 1)]
    public string EntityId;
    [XmlElement(Order = 2)]
    public int InteractId;
    [XmlElement(Order = 3)]
    public bool IsEnabled; // or Visible
    [XmlElement(Order = 4)]
    public InteractObjectType Type;

    public MapInteractObject() { }

    public MapInteractObject(string entityId, int interactId, bool isEnabled, InteractObjectType type)
    {
        EntityId = entityId;
        InteractId = interactId;
        IsEnabled = isEnabled;
        Type = type;
    }
}
[XmlType]
public class MapLiftableObject
{
    [XmlElement(Order = 1)]
    public string EntityId;
    [XmlElement(Order = 2)]
    public int ItemId;
    [XmlElement(Order = 3)]
    public string MaskQuestId;
    [XmlElement(Order = 4)]
    public string MaskQuestState;

    public MapLiftableObject() { }

    public MapLiftableObject(string entityId, int itemId, string maskQuestId, string maskQuestState)
    {
        EntityId = entityId;
        ItemId = itemId;
        MaskQuestId = maskQuestId;
        MaskQuestState = maskQuestState;
    }
}
