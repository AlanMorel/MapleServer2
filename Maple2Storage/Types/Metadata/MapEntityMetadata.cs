﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class MapEntityMetadata
    {
        [XmlElement(Order = 1)]
        public readonly int MapId;
        [XmlElement(Order = 2)]
        public readonly List<NpcMetadata> Npcs;
        [XmlElement(Order = 3)]
        public readonly List<MapPortal> Portals;
        [XmlElement(Order = 4)]
        public readonly List<MapPlayerSpawn> PlayerSpawns;
        [XmlElement(Order = 5)]
        public readonly List<MapObject> Objects;
        [XmlElement(Order = 6)]
        public CoordS BoundingBox0;
        [XmlElement(Order = 7)]
        public CoordS BoundingBox1;

        // Required for deserialization
        public MapEntityMetadata()
        {
            PlayerSpawns = new List<MapPlayerSpawn>();
            Npcs = new List<NpcMetadata>();
            Portals = new List<MapPortal>();
            Objects = new List<MapObject>();
        }

        public MapEntityMetadata(int mapId)
        {
            MapId = mapId;
            PlayerSpawns = new List<MapPlayerSpawn>();
            Npcs = new List<NpcMetadata>();
            Portals = new List<MapPortal>();
            Objects = new List<MapObject>();
        }

        public override string ToString() =>
            $"MapEntityMetadata(Id:{MapId},PlayerSpawns:{string.Join(",", PlayerSpawns)},Npcs:{string.Join(",", Npcs)},Portals:{string.Join(",", Portals)},Objects:{string.Join(",", Objects)})";

        protected bool Equals(MapEntityMetadata other)
        {
            return MapId == other.MapId && PlayerSpawns.SequenceEqual(other.PlayerSpawns) && Npcs.SequenceEqual(other.Npcs) && Portals.SequenceEqual(other.Portals) && Objects.SequenceEqual(other.Objects);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
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
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
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
    public class MapPortal
    {
        [XmlElement(Order = 1)]
        public readonly int Id;
        [XmlElement(Order = 2)]
        public readonly MapPortalFlag Flags;
        [XmlElement(Order = 3)]
        public readonly int Target;
        [XmlElement(Order = 4)]
        public readonly CoordS Coord;
        [XmlElement(Order = 5)]
        public readonly CoordS Rotation;

        // Required for deserialization
        public MapPortal() { }

        public MapPortal(int id, MapPortalFlag flags, int target, CoordS coord, CoordS rotation)
        {
            Id = id;
            Flags = flags;
            Target = target;
            Coord = coord;
            Rotation = rotation;
        }

        public override string ToString() =>
            $"MapPortal(Id:{Id},Flags:{Flags},Target:{Target},Rotation:{Rotation},Coord:{Coord})";

        protected bool Equals(MapPortal other)
        {
            return Id == other.Id
                   && Flags == other.Flags
                   && Target == other.Target
                   && Coord.Equals(other.Coord)
                   && Rotation.Equals(other.Rotation);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
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

    [Flags]
    public enum MapPortalFlag : byte
    {
        None = 0,
        Visible = 1,
        Enabled = 2,
        MinimapVisible = 4,
    }
}
