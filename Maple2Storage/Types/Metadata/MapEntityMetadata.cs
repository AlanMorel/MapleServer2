using System.Xml.Serialization;
using Maple2Storage.Enums;
using ProtoBuf;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MapEntityMetadata
{
    [XmlElement(Order = 1)]
    public readonly List<MapNpc> Npcs = new();
    [XmlElement(Order = 2)]
    public readonly List<MapPortal> Portals = new();
    [XmlElement(Order = 3)]
    public readonly List<MapPlayerSpawn> PlayerSpawns = new();
    [XmlElement(Order = 4)]
    public readonly List<MapMobSpawn> MobSpawns = new();
    [XmlElement(Order = 5)]
    public readonly List<MapWeaponObject> WeaponObjects = new();
    [XmlElement(Order = 6)]
    public CoordS BoundingBox0;
    [XmlElement(Order = 7)]
    public CoordS BoundingBox1;
    [XmlElement(Order = 8)]
    public List<CoordS> HealingSpot = new();
    [XmlElement(Order = 9)]
    public List<PatrolData> PatrolDatas = new();
    [XmlElement(Order = 10)]
    public List<WayPoint> WayPoints = new();
    [XmlElement(Order = 11)]
    public readonly List<MapTriggerMesh> TriggerMeshes = new();
    [XmlElement(Order = 12)]
    public readonly List<MapTriggerEffect> TriggerEffects = new();
    [XmlElement(Order = 13)]
    public readonly List<MapTriggerCamera> TriggerCameras = new();
    [XmlElement(Order = 14)]
    public readonly List<MapTriggerBox> TriggerBoxes = new();
    [XmlElement(Order = 15)]
    public readonly List<MapTriggerLadder> TriggerLadders = new();
    [XmlElement(Order = 16)]
    public readonly List<MapEventNpcSpawnPoint> EventNpcSpawnPoints = new();
    [XmlElement(Order = 17)]
    public readonly List<MapTriggerActor> TriggerActors = new();
    [XmlElement(Order = 18)]
    public readonly List<MapTriggerCube> TriggerCubes = new();
    [XmlElement(Order = 19)]
    public readonly List<MapTriggerSound> TriggerSounds = new();
    [XmlElement(Order = 20)]
    public readonly List<MapTriggerRope> TriggerRopes = new();
    [XmlElement(Order = 21)]
    public readonly List<MapBreakableActorObject> BreakableActors = new();
    [XmlElement(Order = 22)]
    public readonly List<MapBreakableNifObject> BreakableNifs = new();
    [XmlElement(Order = 23)]
    public readonly List<MapVibrateObject> VibrateObjects = new();
    [XmlElement(Order = 24)]
    public readonly List<MapTriggerSkill> TriggerSkills = new();
    [XmlElement(Order = 25)]
    public readonly List<MapInteractObject> InteractObjects = new();
    [XmlElement(Order = 26)]
    public readonly List<MapLiftableObject> LiftableObjects = new();
    [XmlElement(Order = 27)]
    public readonly List<MapLiftableTarget> LiftableTargets = new();
    [XmlElement(Order = 28)]
    public readonly List<MapChestMetadata> MapChests = new();

    public MapEntityMetadata() { }
}

[XmlType]
public class MapWeaponObject
{
    [XmlElement(Order = 1)]
    public readonly CoordB Coord;
    [XmlElement(Order = 2)]
    public readonly List<int> WeaponItemIds = new();

    public MapWeaponObject() { }

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
    public string PatrolDataUuid;
    [XmlElement(Order = 7)]
    public bool IsSpawnOnFieldCreate;
    [XmlElement(Order = 8)]
    public bool IsDayDie;
    [XmlElement(Order = 9)]
    public bool IsNightDie;

    public MapNpc() { }

    public MapNpc(int id, string modelName, string instanceName, CoordS coord, CoordS rotation, bool isSpawnOnFieldCreate, bool isDayDie, bool isNightDie,
        string patrolDataUuid)
    {
        Id = id;
        ModelName = modelName;
        InstanceName = instanceName;
        Coord = coord;
        Rotation = rotation;
        IsSpawnOnFieldCreate = isSpawnOnFieldCreate;
        IsDayDie = isDayDie;
        IsNightDie = isNightDie;
        PatrolDataUuid = patrolDataUuid;
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

    public MapPortal() { }

    public MapPortal(int id, string name, bool enable, bool isVisible, bool minimapVisible, int target, CoordS coord, CoordS rotation, int targetPortalId,
        PortalTypes portalType, int triggerId = 0)
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
    public string Uuid;
    [XmlElement(Order = 2)]
    public string Name;
    [XmlElement(Order = 3)]
    public bool IsAirWayPoint;
    [XmlElement(Order = 4)]
    public int PatrolSpeed;
    [XmlElement(Order = 5)]
    public bool IsLoop;
    [XmlElement(Order = 6)]
    public List<WayPoint> WayPoints;

    public PatrolData() { }

    public PatrolData(string uuid, string name, bool isAirWayPoint, int patrolSpeed, bool isLoop, List<WayPoint> wayPoints)
    {
        Uuid = uuid;
        Name = name;
        IsAirWayPoint = isAirWayPoint;
        PatrolSpeed = patrolSpeed;
        IsLoop = isLoop;
        WayPoints = wayPoints;
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
    [XmlElement(Order = 5)]
    public string ApproachAnimation;
    [XmlElement(Order = 6)]
    public string ArriveAnimation;
    [XmlElement(Order = 7)]
    public int ArriveAnimationTime;

    public WayPoint() { }

    public WayPoint(string id, bool isVisible, CoordS position, CoordS rotation, string approachAnimation, string arriveAnimation, int arriveAnimationTime)
    {
        Id = id;
        IsVisible = isVisible;
        Position = position;
        Rotation = rotation;
        ApproachAnimation = approachAnimation;
        ArriveAnimation = arriveAnimation;
        ArriveAnimationTime = arriveAnimationTime;
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

    public MapEventNpcSpawnPoint(int id, uint count, IEnumerable<string> npcIds, string spawnAnimation, float spawnRadius, CoordF position, CoordF rotation)
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

[ProtoContract]
[ProtoInclude(50, typeof(MapTriggerMesh))]
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

    private MapTriggerMesh() { }
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

    private MapTriggerEffect() { }
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

    private MapTriggerCamera() { }
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

    private MapTriggerBox() { }
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

    private MapTriggerLadder() { }
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

    private MapTriggerActor() { }
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

    public MapTriggerCube() { }
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

    public MapTriggerSound() { }
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

    private MapTriggerRope() { }
}

[ProtoContract]
[ProtoInclude(20, typeof(MapBreakableNifObject))]
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

    public MapBreakableNifObject(string id, bool isEnabled, int triggerId, int hideDuration, int resetDuration) : base(id, isEnabled, hideDuration,
        resetDuration)
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

    public MapTriggerSkill() { }
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
    [XmlElement(Order = 2)]
    public CoordF Position;

    public MapVibrateObject() { }

    public MapVibrateObject(string id, CoordF position)
    {
        EntityId = id;
        Position = position;
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
    [XmlElement(Order = 5)]
    public CoordF Position;
    [XmlElement(Order = 6)]
    public CoordF Rotation;

    public MapInteractObject() { }

    public MapInteractObject(string entityId, int interactId, bool isEnabled, InteractObjectType type, CoordF position, CoordF rotation)
    {
        EntityId = entityId;
        InteractId = interactId;
        IsEnabled = isEnabled;
        Type = type;
        Position = position;
        Rotation = rotation;
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
    public int ItemStackCount;
    [XmlElement(Order = 4)]
    public string MaskQuestId;
    [XmlElement(Order = 5)]
    public string MaskQuestState;
    [XmlElement(Order = 6)]
    public string EffectQuestId;
    [XmlElement(Order = 7)]
    public string EffectQuestState;
    [XmlElement(Order = 8)]
    public int ItemLifeTime;
    [XmlElement(Order = 9)]
    public int LiftableRegenCheckTime;
    [XmlElement(Order = 10)]
    public int LiftableFinishTime;
    [XmlElement(Order = 11)]
    public CoordF Position;
    [XmlElement(Order = 12)]
    public CoordF Rotation;

    public MapLiftableObject() { }

    public MapLiftableObject(string entityId, int itemId, int itemStackCount, string maskQuestId, string maskQuestState, string effectQuestId, string effectQuestState,
        int itemLifeTime, int regenCheckTime, int liftableFinishTime, CoordF position, CoordF rotation)
    {
        EntityId = entityId;
        ItemId = itemId;
        ItemStackCount = itemStackCount;
        MaskQuestId = maskQuestId;
        MaskQuestState = maskQuestState;
        EffectQuestId = effectQuestId;
        EffectQuestState = effectQuestState;
        ItemLifeTime = itemLifeTime;
        LiftableRegenCheckTime = regenCheckTime;
        LiftableFinishTime = liftableFinishTime;
        Position = position;
        Rotation = rotation;
    }
}

[XmlType]
public class MapLiftableTarget
{
    [XmlElement(Order = 1)]
    public int Target;
    [XmlElement(Order = 2)]
    public CoordF Position;
    [XmlElement(Order = 3)]
    public CoordF ShapeDimensions;

    public MapLiftableTarget() { }

    public MapLiftableTarget(int target, CoordF position, CoordF shapeDimensions)
    {
        Target = target;
        Position = position;
        ShapeDimensions = shapeDimensions;
    }
}

[XmlType]
public class MapChestMetadata
{
    [XmlElement(Order = 1)]
    public CoordF Position;
    [XmlElement(Order = 2)]
    public CoordF Rotation;
    [XmlElement(Order = 3)]
    public bool IsGolden;
}
