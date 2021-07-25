using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Maple2Storage.Enums;
using ProtoBuf;

namespace Maple2Storage.Types.Metadata
{
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
        public readonly List<MapObject> Objects;
        [XmlElement(Order = 7)]
        public CoordS BoundingBox0;
        [XmlElement(Order = 8)]
        public CoordS BoundingBox1;
        [XmlElement(Order = 9)]
        public readonly List<MapInteractObject> InteractObjects;
        [XmlElement(Order = 10)]
        public readonly List<MapInteractMesh> InteractMeshes;
        [XmlElement(Order = 11)]
        public List<CoordS> HealingSpot;
        [XmlElement(Order = 12)]
        public readonly List<TriggerObject> TriggerObjects;

        // Required for deserialization
        public MapEntityMetadata()
        {
            PlayerSpawns = new List<MapPlayerSpawn>();
            MobSpawns = new List<MapMobSpawn>();
            Npcs = new List<MapNpc>();
            Portals = new List<MapPortal>();
            Objects = new List<MapObject>();
            InteractObjects = new List<MapInteractObject>();
            InteractMeshes = new List<MapInteractMesh>();
            HealingSpot = new List<CoordS>();
            TriggerObjects = new List<TriggerObject>();
        }

        public MapEntityMetadata(int mapId)
        {
            MapId = mapId;
            PlayerSpawns = new List<MapPlayerSpawn>();
            MobSpawns = new List<MapMobSpawn>();
            Npcs = new List<MapNpc>();
            Portals = new List<MapPortal>();
            Objects = new List<MapObject>();
            InteractObjects = new List<MapInteractObject>();
            InteractMeshes = new List<MapInteractMesh>();
            HealingSpot = new List<CoordS>();
            TriggerObjects = new List<TriggerObject>();
        }

        public override string ToString() =>
            $"MapEntityMetadata(Id:{MapId},PlayerSpawns:{string.Join(",", PlayerSpawns)},MobSpawns:{string.Join(",", MobSpawns)},Npcs:{string.Join(",", Npcs)},Portals:{string.Join(",", Portals)},Objects:{string.Join(",", Objects)})";

        protected bool Equals(MapEntityMetadata other)
        {
            return MapId == other.MapId && PlayerSpawns.SequenceEqual(other.PlayerSpawns) && MobSpawns.SequenceEqual(other.MobSpawns) && Npcs.SequenceEqual(other.Npcs) && Portals.SequenceEqual(other.Portals) && Objects.SequenceEqual(other.Objects);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MapEntityMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MapId, PlayerSpawns, Npcs, Portals, Objects);
        }

        public static bool operator ==(MapEntityMetadata left, MapEntityMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapEntityMetadata left, MapEntityMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class MapObject
    {
        [XmlElement(Order = 1)]
        public readonly CoordB Coord;
        [XmlElement(Order = 2)]
        public readonly int WeaponId;

        // Required for deserialization
        public MapObject() { }

        public MapObject(CoordB coord, int weaponId)
        {
            Coord = coord;
            WeaponId = weaponId;
        }

        public override string ToString() =>
            $"MapObject(Coord:{Coord},WeaponId:{WeaponId})";

        protected bool Equals(MapObject other)
        {
            return Coord.Equals(other.Coord) && WeaponId == other.WeaponId;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MapObject) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coord, WeaponId);
        }

        public static bool operator ==(MapObject left, MapObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapObject left, MapObject right)
        {
            return !Equals(left, right);
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

        public MapNpc(int id, string modelName, string instanceName, CoordS coord, CoordS rotation)
        {
            Id = id;
            ModelName = modelName;
            InstanceName = instanceName;
            Coord = coord;
            Rotation = rotation;
        }

        public override string ToString() =>
            $"MapNpc(Id:{Id},ModelName:{ModelName},Rotation:{Rotation},Coord:{Coord})";

        protected bool Equals(MapNpc other)
        {
            // TODO: Check instance name instead.
            return Id == other.Id && Coord.Equals(other.Coord) && Rotation.Equals(other.Rotation);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MapNpc) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public static bool operator ==(MapNpc left, MapNpc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapNpc left, MapNpc right)
        {
            return !Equals(left, right);
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
        public readonly MapPortalFlag Flags;
        [XmlElement(Order = 4)]
        public readonly int Target;
        [XmlElement(Order = 5)]
        public readonly CoordS Coord;
        [XmlElement(Order = 6)]
        public readonly CoordS Rotation;
        [XmlElement(Order = 7)]
        public readonly int TargetPortalId;
        [XmlElement(Order = 8)]
        public readonly byte PortalType;

        // Required for deserialization
        public MapPortal() { }

        public MapPortal(int id, string name, MapPortalFlag flags, int target, CoordS coord, CoordS rotation, int targetPortalId, byte portalType)
        {
            Id = id;
            Name = name;
            Flags = flags;
            Target = target;
            Coord = coord;
            Rotation = rotation;
            TargetPortalId = targetPortalId;
            PortalType = portalType;
        }

        public override string ToString() =>
            $"MapPortal(Id:{Id},String:{Name},Flags:{Flags},Target:{Target},Rotation:{Rotation},Coord:{Coord},TargetPortalId:{TargetPortalId}, PortalType:{PortalType})";

        protected bool Equals(MapPortal other)
        {
            return Id == other.Id
                   && Name == other.Name
                   && Flags == other.Flags
                   && Target == other.Target
                   && Coord.Equals(other.Coord)
                   && Rotation.Equals(other.Rotation)
                   && TargetPortalId == other.TargetPortalId
                   && PortalType == other.PortalType;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MapPortal) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, (byte) Flags, Target, Coord, Rotation);
        }

        public static bool operator ==(MapPortal left, MapPortal right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapPortal left, MapPortal right)
        {
            return !Equals(left, right);
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

        public override string ToString() =>
            $"MapPlayerSpawn(Coord:{Coord},Rotation:{Rotation})";
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

        public override string ToString() =>
            $"MapMobSpawn(Id:{Id},Coord:{Coord},NpcCount:{NpcCount},NpcList{NpcList},SpawnRadius:{SpawnRadius})";
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
    public class MapInteractObject
    {
        [XmlElement(Order = 1)]
        public readonly string Uuid;
        [XmlElement(Order = 2)]
        public readonly string Name;
        [XmlElement(Order = 3)]
        public readonly InteractObjectType Type;
        [XmlElement(Order = 4)]
        public readonly int InteractId;
        [XmlElement(Order = 5)]
        public readonly int RecipeId;

        public MapInteractObject() { }
        public MapInteractObject(string uuid, string name, InteractObjectType type, int interactId, int recipeId = 0)
        {
            Uuid = uuid;
            Name = name;
            Type = type;
            InteractId = interactId;
            RecipeId = recipeId;
        }
        public override string ToString() =>
            $"MapInteractObject(UUID:{Uuid},Name:{Name},Type:{Type},InteractId:{InteractId},RecipeId:{RecipeId})";
    }

    [XmlType]
    public class MapInteractMesh
    {
        [XmlElement(Order = 1)]
        public readonly string Uuid;
        [XmlElement(Order = 2)]
        public readonly string Name;

        public MapInteractMesh() { }
        public MapInteractMesh(string uuid, string name)
        {
            Uuid = uuid;
            Name = name;
        }
        public override string ToString() =>
            $"MapInteractMesh(UUID:{Uuid},Name:{Name})";
    }

    [XmlType]
    [ProtoContract, ProtoInclude(5, typeof(TriggerMesh))]
    public class TriggerObject
    {
        [XmlElement(Order = 1)]
        public int Id;

        public TriggerObject(int id)
        {
            Id = id;
        }
    }

    [XmlType]
    public class TriggerMesh : TriggerObject
    {
        [XmlElement(Order = 1)]
        public bool IsVisible;
        public TriggerMesh(int id, bool isVisible) : base(id)
        {
            IsVisible = isVisible;
        }
    }

    //[XmlType]
    //public class TriggerEffect : TriggerObject
    //{
    //    [XmlElement(Order = 1)]
    //    public bool IsVisible = true;
    //    public TriggerEffect(int id) : base(id)
    //    {
    //    }
    //}

    [Flags]
    public enum MapPortalFlag : byte
    {
        None = 0,
        Visible = 1,
        Enabled = 2,
        MinimapVisible = 4,
    }
}
