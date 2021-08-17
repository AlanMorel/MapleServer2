using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Maple2Storage.Types
{
    [XmlType]
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct CoordF
    {
        [XmlElement(Order = 1)]
        public float X { get; set; }
        [XmlElement(Order = 2)]
        public float Y { get; set; }
        [XmlElement(Order = 3)]
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

        public static CoordF From(float distance, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            return new CoordF
            {
                X = (float) (distance * Math.Cos(angle)),
                Y = (float) (distance * Math.Sin(angle)),
                Z = 0.0f,
            };
        }

        public static CoordF FromVector3(Vector3 vector3)
        {
            return new CoordF
            {
                X = vector3.X,
                Y = vector3.Y,
                Z = vector3.Z
            };
        }

        public readonly CoordS ToShort()
        {
            return CoordS.From((short) X, (short) Y, (short) Z);
        }

        public readonly CoordB ToByte()
        {
            return CoordB.From(
                (sbyte) (X / Block.BLOCK_SIZE),
                (sbyte) (Y / Block.BLOCK_SIZE),
                (sbyte) (Z / Block.BLOCK_SIZE));
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

        public static CoordF operator +(CoordF coord, float value)
        {
            return From(coord.X + value, coord.Y + value, coord.Z + value);
        }

        public static CoordF operator +(float value, CoordF coord)
        {
            return From(coord.X + value, coord.Y + value, coord.Z + value);
        }

        public static CoordF operator -(CoordF left, CoordF right)
        {
            return From((float) (left.X - right.X), (float) (left.Y - right.Y), (float) (left.Z - right.Z));
        }

        public static CoordF operator -(CoordF coord, float value)
        {
            return From(coord.X - value, coord.Y - value, coord.Z - value);
        }

        public static float operator *(CoordF left, CoordF right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static CoordF operator *(CoordF coord, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return From((float) (coord.X * cos - coord.Y * sin), (float) (coord.X * sin + coord.Y * cos), coord.Z);
        }

        public float Length()
        {
            return (float) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public static float Distance(CoordF left, CoordF right)
        {
            CoordF displacement = left - right;
            return displacement.Length();
        }

        public double XYAngle()
        {
            return (1800 * Math.Atan2(Y, X) / Math.PI) + 900;
        }

        public override string ToString() => $"CoordF({X}, {Y}, {Z})";

        public bool Equals(CoordF other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((CoordF) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static CoordF Parse(string value)
        {
            string[] coord = value.Split(", ");
            return From(
                float.Parse(coord[0]),
                float.Parse(coord[1]),
                float.Parse(coord[2])
            );
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

        public static CoordS From(short distance, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            return new CoordS
            {
                X = (short) (distance * Math.Cos(angle)),
                Y = (short) (distance * Math.Sin(angle)),
                Z = 0,
            };
        }

        public static CoordS FromVector3(Vector3 vector3)
        {
            return From((short) vector3.X, (short) vector3.Y, (short) vector3.Z);
        }

        public readonly CoordF ToFloat()
        {
            return CoordF.From(X, Y, Z);
        }

        public readonly CoordB ToByte()
        {
            return CoordB.From(
                (sbyte) (X / Block.BLOCK_SIZE),
                (sbyte) (Y / Block.BLOCK_SIZE),
                (sbyte) (Z / Block.BLOCK_SIZE));
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

        public static CoordS operator +(CoordS coord, short value)
        {
            return From((short) (coord.X + value), (short) (coord.Y + value), (short) (coord.Z + value));
        }

        public static CoordS operator +(short value, CoordS coord)
        {
            return From((short) (coord.X + value), (short) (coord.Y + value), (short) (coord.Z + value));
        }

        public static CoordS operator -(CoordS left, CoordS right)
        {
            return From((short) (left.X - right.X), (short) (left.Y - right.Y), (short) (left.Z - right.Z));
        }

        public static CoordS operator -(CoordS coord, short value)
        {
            return From((short) (coord.X - value), (short) (coord.Y - value), (short) (coord.Z - value));
        }

        public static short operator *(CoordS left, CoordS right)
        {
            return (short) ((left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z));
        }

        public static CoordS operator *(CoordS coord, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return From((short) (coord.X * cos - coord.Y * sin), (short) (coord.X * sin + coord.Y * cos), coord.Z);
        }

        public short Length()
        {
            return (short) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public static short Distance(CoordS left, CoordS right)
        {
            CoordS displacement = left - right;
            return displacement.Length();
        }

        public double XYAngle()
        {
            return (1800 * Math.Atan2(Y, X) / Math.PI) + 900;
        }

        public override string ToString() => $"CoordS({X}, {Y}, {Z})";

        public bool Equals(CoordS other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((CoordS) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static CoordS Parse(string value)
        {
            string[] coord = value.Split(", ");
            return From(
                (short) float.Parse(coord[0]),
                (short) float.Parse(coord[1]),
                (short) float.Parse(coord[2])
            );
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

        public static CoordB From(sbyte distance, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            return new CoordB
            {
                X = (sbyte) (distance * Math.Cos(angle)),
                Y = (sbyte) (distance * Math.Sin(angle)),
                Z = 0,
            };
        }

        public readonly CoordF ToFloat()
        {
            return CoordF.From(
                (float) (X * Block.BLOCK_SIZE),
                (float) (Y * Block.BLOCK_SIZE),
                (float) (Z * Block.BLOCK_SIZE));
        }

        public readonly CoordS ToShort()
        {
            return CoordS.From(
                (short) (X * Block.BLOCK_SIZE),
                (short) (Y * Block.BLOCK_SIZE),
                (short) (Z * Block.BLOCK_SIZE));
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

        public static CoordB operator +(CoordB coord, short value)
        {
            return From((sbyte) (coord.X + value), (sbyte) (coord.Y + value), (sbyte) (coord.Z + value));
        }

        public static CoordB operator +(short value, CoordB coord)
        {
            return From((sbyte) (coord.X + value), (sbyte) (coord.Y + value), (sbyte) (coord.Z + value));
        }

        public static CoordB operator -(CoordB left, CoordB right)
        {
            return From((sbyte) (left.X - right.X), (sbyte) (left.Y - right.Y), (sbyte) (left.Z - right.Z));
        }

        public static CoordB operator -(CoordB coord, sbyte value)
        {
            return From((sbyte) (coord.X - value), (sbyte) (coord.Y - value), (sbyte) (coord.Z - value));
        }

        public static sbyte operator *(CoordB left, CoordB right)
        {
            return (sbyte) ((left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z));
        }

        public static CoordB operator *(CoordB coord, double zRotation)
        {
            double angle = Math.PI * ((zRotation - 900) / 1800);
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return From((sbyte) (coord.X * cos - coord.Y * sin), (sbyte) (coord.X * sin + coord.Y * cos), coord.Z);
        }

        public sbyte Length()
        {
            return (sbyte) Math.Sqrt((X * X + Y * Y + Z * Z));
        }

        public static sbyte Distance(CoordB left, CoordB right)
        {
            CoordB displacement = left - right;
            return displacement.Length();
        }

        public double XYAngle()
        {
            return (1800 * Math.Atan2(Y, X) / Math.PI) + 900;
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
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((CoordB) obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
