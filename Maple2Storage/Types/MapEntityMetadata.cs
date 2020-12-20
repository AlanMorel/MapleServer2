using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types {
    [XmlType]
    public class MapEntityMetadata {
        [XmlElement(Order = 1)]
        public readonly int MapId;
        [XmlElement(Order = 2)]
        public readonly List<MapNpc> Npcs;
        [XmlElement(Order = 3)]
        public readonly List<MapPortal> Portals;
        [XmlElement(Order = 4)]
        public readonly List<MapPlayerSpawn> PlayerSpawn;

        // Required for deserialization
        public MapEntityMetadata() {
            this.Npcs = new List<MapNpc>();
            this.Portals = new List<MapPortal>();
            this.PlayerSpawn = new List<MapPlayerSpawn>();
        }

        public MapEntityMetadata(int mapId) {
            this.MapId = mapId;
            this.Npcs = new List<MapNpc>();
            this.Portals = new List<MapPortal>();
            this.PlayerSpawn = new List<MapPlayerSpawn>();
        }

        public override string ToString() =>
            $"MapEntityMetadata(Id:{MapId},Npcs:{string.Join(",", Npcs)},Portals:{string.Join(",", Portals)},PlayerSpawn:{string.Join(",", PlayerSpawn)},)";

        protected bool Equals(MapEntityMetadata other) {
            return MapId == other.MapId && Npcs.SequenceEqual(other.Npcs) && Portals.SequenceEqual(other.Portals) && PlayerSpawn.SequenceEqual(other.PlayerSpawn);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapEntityMetadata) obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(MapId, Npcs, Portals, PlayerSpawn);
        }

        public static bool operator ==(MapEntityMetadata left, MapEntityMetadata right) {
            return Equals(left, right);
        }

        public static bool operator !=(MapEntityMetadata left, MapEntityMetadata right) {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class MapNpc {
        [XmlElement(Order = 1)]
        public readonly int Id;
        [XmlElement(Order = 2)]
        public readonly CoordS Coord;
        [XmlElement(Order = 3)]
        public readonly CoordS Rotation;

        // Required for deserialization
        public MapNpc() { }

        public MapNpc(int id, CoordS coord, CoordS rotation) {
            this.Id = id;
            this.Coord = coord;
            this.Rotation = rotation;
        }

        public override string ToString() =>
            $"MapNpc(Id:{Id},Rotation:{Rotation},Coord:{Coord})";

        protected bool Equals(MapNpc other) {
            return Id == other.Id && Coord.Equals(other.Coord) && Rotation.Equals(other.Rotation);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapNpc) obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public static bool operator ==(MapNpc left, MapNpc right) {
            return Equals(left, right);
        }

        public static bool operator !=(MapNpc left, MapNpc right) {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class MapPortal {
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

        public MapPortal(int id, MapPortalFlag flags, int target, CoordS coord, CoordS rotation) {
            this.Id = id;
            this.Flags = flags;
            this.Target = target;
            this.Coord = coord;
            this.Rotation = rotation;
        }

        public override string ToString() =>
            $"MapPortal(Id:{Id},Flags:{Flags},Target:{Target},Rotation:{Rotation},Coord:{Coord})";

        protected bool Equals(MapPortal other) {
            return Id == other.Id
                   && Flags == other.Flags
                   && Target == other.Target
                   && Coord.Equals(other.Coord)
                   && Rotation.Equals(other.Rotation);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapPortal) obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, (byte) Flags, Target, Coord, Rotation);
        }

        public static bool operator ==(MapPortal left, MapPortal right) {
            return Equals(left, right);
        }

        public static bool operator !=(MapPortal left, MapPortal right) {
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

        public MapPlayerSpawn()
        {

        }
        public MapPlayerSpawn(CoordS coord, CoordS rotation)
        {
            this.Coord = coord;
            this.Rotation = rotation;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapPlayerSpawn)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coord, Rotation);
        }

        public static bool operator ==(MapPlayerSpawn left, MapPlayerSpawn right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MapPlayerSpawn left, MapPlayerSpawn right)
        {
            return !Equals(left, right);
        }
        public override string ToString() => $"MapPlayerSpawn(Position:{Coord},Rotation:{Rotation}";
    }

    [Flags]
    public enum MapPortalFlag : byte {
        None = 0,
        Visible = 1,
        Enabled = 2,
        MinimapVisible = 4,
    }
}