using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Maple2Storage.Types
{
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct CoordF
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

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

        public readonly CoordF Add(CoordF other)
        {
            return From(X + other.X, Y + other.Y, Z + other.Z);
        }

        public readonly CoordF ClosestBlock()
        {
            return From(
                ((int) X + 75) / 150 * 150,
                ((int) Y + 75) / 150 * 150,
                ((int) Z + 75) / 150 * 150
            );
        }

        public override string ToString() => $"CoordF({X}, {Y}, {Z})";
    }

    [XmlType]
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 6)]
    public struct CoordS
    {
        [XmlElement(Order = 1)]
        public short X { get; private set; }
        [XmlElement(Order = 2)]
        public short Y { get; private set; }
        [XmlElement(Order = 3)]
        public short Z { get; private set; }

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

        public readonly CoordS Add(CoordS other)
        {
            return From((short) (X + other.X), (short) (Y + other.Y), (short) (Z + other.Z));
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

        public readonly CoordB Add(CoordB other)
        {
            return From((sbyte) (X + other.X), (sbyte) (Y + other.Y), (sbyte) (Z + other.Z));
        }

        public override string ToString() => $"CoordB({X}, {Y}, {Z})";
    }
}
