using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Maple2Storage.Types
{
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct CoordF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static CoordF From(float x, float y, float z)
        {
            return new CoordF
            {
                X = x,
                Y = y,
                Z = z,
            };
        }

        public readonly CoordS ToShort()
        {
            return CoordS.From((short) X, (short) Y, (short) Z);
        }

        public readonly CoordF ClosestBlock()
        {
            return From(
                ((int) X + 75) / 150 * 150,
                ((int) Y + 75) / 150 * 150,
                ((int) Z + 75) / 150 * 150
            );
        }

        public static bool operator ==(CoordF left, CoordF right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CoordF left, CoordF right)
        {
            return !Equals(left, right);
        }

        public static CoordF operator +(CoordF left, CoordF right)
        {
            return From((float) (left.X + right.X), (float) (left.Y + right.Y), (float) (left.Z + right.Z));
        }

        public static CoordF operator -(CoordF left, CoordF right)
        {
            return From((float) (left.X - right.X), (float) (left.Y - right.Y), (float) (left.Z - right.Z));
        }

        public float Length()
        {
            return (float) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public override string ToString() => $"CoordF({X}, {Y}, {Z})";

        public bool Equals(CoordF other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((CoordF) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    [XmlType]
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 6)]
    public struct CoordS
    {
        [XmlElement(Order = 1)]
        public short X { get; set; }
        [XmlElement(Order = 2)]
        public short Y { get; set; }
        [XmlElement(Order = 3)]
        public short Z { get; set; }

        public static CoordS From(short x, short y, short z)
        {
            return new CoordS
            {
                X = x,
                Y = y,
                Z = z,
            };
        }

        public readonly CoordF ToFloat()
        {
            return CoordF.From(X, Y, Z);
        }

        public static bool operator ==(CoordS left, CoordS right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CoordS left, CoordS right)
        {
            return !Equals(left, right);
        }

        public static CoordS operator +(CoordS left, CoordS right)
        {
            return From((short) (left.X + right.X), (short) (left.Y + right.Y), (short) (left.Z + right.Z));
        }

        public static CoordS operator -(CoordS left, CoordS right)
        {
            return From((short) (left.X - right.X), (short) (left.Y - right.Y), (short) (left.Z - right.Z));
        }

        public short Length()
        {
            return (short) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public readonly CoordS ClosestBlock()
        {
            return From(
                (short) ((X + 75) / 150 * 150),
                (short) ((Y + 75) / 150 * 150),
                (short) ((Z + 75) / 150 * 150)
            );
        }

        public override string ToString() => $"CoordS({X}, {Y}, {Z})";

        public bool Equals(CoordS other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((CoordS) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    [XmlType]
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 3)]
    public struct CoordB
    {
        [XmlElement(Order = 1)]
        public sbyte X { get; private set; }
        [XmlElement(Order = 2)]
        public sbyte Y { get; private set; }
        [XmlElement(Order = 3)]
        public sbyte Z { get; private set; }

        public static CoordB From(sbyte x, sbyte y, sbyte z)
        {
            return new CoordB
            {
                X = x,
                Y = y,
                Z = z,
            };
        }

        public readonly CoordF ToFloat()
        {
            return CoordF.From(X, Y, Z);
        }

        public static bool operator ==(CoordB left, CoordB right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CoordB left, CoordB right)
        {
            return !Equals(left, right);
        }

        public static CoordB operator +(CoordB left, CoordB right)
        {
            return From((sbyte) (left.X + right.X), (sbyte) (left.Y + right.Y), (sbyte) (left.Z + right.Z));
        }

        public static CoordB operator -(CoordB left, CoordB right)
        {
            return From((sbyte) (left.X - right.X), (sbyte) (left.Y - right.Y), (sbyte) (left.Z - right.Z));
        }

        public sbyte Length()
        {
            return (sbyte) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public static CoordB Parse(string value, string separator)
        {
            string[] coord = value.Split(separator);
            return CoordB.From(
                (sbyte) sbyte.Parse(coord[0]),
                (sbyte) sbyte.Parse(coord[1]),
                (sbyte) sbyte.Parse(coord[2])
            );
        }

        public override string ToString() => $"CoordB({X}, {Y}, {Z})";

        public bool Equals(CoordB other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((CoordB) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
