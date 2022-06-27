using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Maple2Storage.Types;

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

    #region Constructors

    public static CoordF From(float x, float y, float z)
    {
        return new()
        {
            X = x,
            Y = y,
            Z = z
        };
    }

    public static CoordF From(float distance, double zRotation)
    {
        double angle = Math.PI * ((zRotation - 900) / 1800);
        return new()
        {
            X = (float) (distance * Math.Cos(angle)),
            Y = (float) (distance * Math.Sin(angle)),
            Z = 0.0f
        };
    }

    public static CoordF FromVector3(Vector3 vector3)
    {
        return From(vector3.X, vector3.Y, vector3.Z);
    }

    public static CoordF Parse(string value)
    {
        string[] coord = value.Split(",").Select(x => x.Trim()).ToArray();
        return From(
            float.Parse(coord[0]),
            float.Parse(coord[1]),
            float.Parse(coord[2])
        );
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

    #endregion

    #region Operators

    public static explicit operator CoordF(CoordS coordS) => From(coordS.X, coordS.Y, coordS.Z);
    public static implicit operator CoordS(CoordF coordS) => CoordS.From(coordS.X, coordS.Y, coordS.Z);

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
        return From(left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z);
    }

    public static CoordF operator +(CoordF coord, float value)
    {
        return From(
            coord.X + value,
            coord.Y + value,
            coord.Z + value);
    }

    public static CoordF operator +(float value, CoordF coord)
    {
        return From(
            coord.X + value,
            coord.Y + value,
            coord.Z + value);
    }

    public static CoordF operator -(CoordF left, CoordF right)
    {
        return From(left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z);
    }

    public static CoordF operator -(CoordF coord, float value)
    {
        return From(
            coord.X - value,
            coord.Y - value,
            coord.Z - value);
    }

    public static float operator *(CoordF left, CoordF right)
    {
        return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
    }

    public static CoordF operator *(CoordF left, float right)
    {
        return From(
            right * left.X,
            right * left.Y,
            right * left.Z);
    }

    public static CoordF operator /(CoordF value1, float value2)
    {
        return value1 / From(value2, value2, value2);
    }

    public static CoordF operator /(CoordF left, CoordF right)
    {
        return From(
            left.X / right.X,
            left.Y / right.Y,
            left.Z / right.Z
        );
    }

    #endregion

    public readonly float Length()
    {
        return MathF.Sqrt(LengthSquared());
    }

    public static float Distance(CoordF left, CoordF right)
    {
        return (left - right).Length();
    }

    // ReSharper disable once InconsistentNaming
    public double XYAngle()
    {
        return 1800 * Math.Atan2(Y, X) / Math.PI + 900;
    }

    public CoordF Normalize()
    {
        return this / Length();
    }

    public static float Dot(CoordF coord1, CoordF coord2)
    {
        return coord1.X * coord2.X + coord1.Y * coord2.Y + coord1.Z * coord2.Z;
    }

    public readonly float LengthSquared()
    {
        return Dot(this, this);
    }

    #region Overrides

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

    public override string ToString()
    {
        return $"CoordF({X}, {Y}, {Z})";
    }

    #endregion
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

    #region Constructors

    public static CoordS From(short x, short y, short z)
    {
        return new()
        {
            X = x,
            Y = y,
            Z = z
        };
    }

    public static CoordS From(int x, int y, int z)
    {
        return new()
        {
            X = (short) x,
            Y = (short) y,
            Z = (short) z
        };
    }

    public static CoordS From(float x, float y, float z)
    {
        return new()
        {
            X = (short) x,
            Y = (short) y,
            Z = (short) z
        };
    }

    public static CoordS From(short distance, double zRotation)
    {
        double angle = Math.PI * ((zRotation - 900) / 1800);
        return new()
        {
            X = (short) (distance * Math.Cos(angle)),
            Y = (short) (distance * Math.Sin(angle)),
            Z = 0
        };
    }

    public static CoordS FromVector3(Vector3 vector3)
    {
        return From((short) vector3.X, (short) vector3.Y, (short) vector3.Z);
    }

    public static CoordS Parse(string value)
    {
        string[] coord = value.Split(",").Select(x => x.Trim()).ToArray();
        return From(
            (short) float.Parse(coord[0]),
            (short) float.Parse(coord[1]),
            (short) float.Parse(coord[2])
        );
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

    #endregion

    #region Operators

    public static explicit operator CoordS(CoordF coordS) => From(coordS.X, coordS.Y, coordS.Z);
    public static implicit operator CoordF(CoordS coordS) => CoordF.From(coordS.X, coordS.Y, coordS.Z);

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
        return From((short) (left.X + right.X),
            (short) (left.Y + right.Y),
            (short) (left.Z + right.Z));
    }

    public static CoordS operator +(CoordS coord, short value)
    {
        return From((short) (coord.X + value),
            (short) (coord.Y + value),
            (short) (coord.Z + value));
    }

    public static CoordS operator +(short value, CoordS coord)
    {
        return From((short) (coord.X + value),
            (short) (coord.Y + value),
            (short) (coord.Z + value));
    }

    public static CoordS operator -(CoordS left, CoordS right)
    {
        return From((short) (left.X - right.X),
            (short) (left.Y - right.Y),
            (short) (left.Z - right.Z));
    }

    public static CoordS operator -(CoordS coord, short value)
    {
        return From((short) (coord.X - value),
            (short) (coord.Y - value),
            (short) (coord.Z - value));
    }

    public static short operator *(CoordS left, CoordS right)
    {
        return (short) (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
    }

    public static CoordS operator *(CoordS coord, double zRotation)
    {
        double angle = Math.PI * ((zRotation - 900) / 1800);
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);
        return From((short) (coord.X * cos - coord.Y * sin),
            (short) (coord.X * sin + coord.Y * cos),
            coord.Z);
    }

    #endregion

    public short Length()
    {
        return (short) Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public static short Distance(CoordS left, CoordS right)
    {
        return (left - right).Length();
    }

    public double XYAngle()
    {
        return 1800 * Math.Atan2(Y, X) / Math.PI + 900;
    }

    #region Overrides

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

    public override string ToString()
    {
        return $"CoordS({X}, {Y}, {Z})";
    }

    #endregion
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

    #region Constructors

    public static CoordB From(sbyte x, sbyte y, sbyte z)
    {
        return new()
        {
            X = x,
            Y = y,
            Z = z
        };
    }

    public static CoordB From(sbyte distance, double zRotation)
    {
        double angle = Math.PI * ((zRotation - 900) / 1800);
        return new()
        {
            X = (sbyte) (distance * Math.Cos(angle)),
            Y = (sbyte) (distance * Math.Sin(angle)),
            Z = 0
        };
    }

    public static CoordB FromVector3(Vector3 vector3)
    {
        return From(
            (sbyte) (vector3.X / Block.BLOCK_SIZE),
            (sbyte) (vector3.Y / Block.BLOCK_SIZE),
            (sbyte) (vector3.Z / Block.BLOCK_SIZE));
    }

    public static CoordB Parse(string value, string separator)
    {
        string[] coord = value.Split(separator).Select(x => x.Trim()).ToArray();
        return From(
            sbyte.Parse(coord[0]),
            sbyte.Parse(coord[1]),
            sbyte.Parse(coord[2])
        );
    }

    public readonly CoordF ToFloat()
    {
        return CoordF.From(
            X * Block.BLOCK_SIZE,
            Y * Block.BLOCK_SIZE,
            Z * Block.BLOCK_SIZE);
    }

    public readonly CoordS ToShort()
    {
        return CoordS.From(
            (short) (X * Block.BLOCK_SIZE),
            (short) (Y * Block.BLOCK_SIZE),
            (short) (Z * Block.BLOCK_SIZE));
    }

    #endregion

    #region Operators

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
        return From((sbyte) (left.X + right.X),
            (sbyte) (left.Y + right.Y),
            (sbyte) (left.Z + right.Z));
    }

    public static CoordB operator +(CoordB coord, short value)
    {
        return From((sbyte) (coord.X + value),
            (sbyte) (coord.Y + value),
            (sbyte) (coord.Z + value));
    }

    public static CoordB operator +(short value, CoordB coord)
    {
        return From((sbyte) (coord.X + value),
            (sbyte) (coord.Y + value),
            (sbyte) (coord.Z + value));
    }

    public static CoordB operator -(CoordB left, CoordB right)
    {
        return From((sbyte) (left.X - right.X),
            (sbyte) (left.Y - right.Y),
            (sbyte) (left.Z - right.Z));
    }

    public static CoordB operator -(CoordB coord, sbyte value)
    {
        return From((sbyte) (coord.X - value),
            (sbyte) (coord.Y - value),
            (sbyte) (coord.Z - value));
    }

    public static sbyte operator *(CoordB left, CoordB right)
    {
        return (sbyte) (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
    }

    public static CoordB operator *(CoordB coord, double zRotation)
    {
        double angle = Math.PI * ((zRotation - 900) / 1800);
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);
        return From((sbyte) (coord.X * cos - coord.Y * sin),
            (sbyte) (coord.X * sin + coord.Y * cos),
            coord.Z);
    }

    #endregion

    public sbyte Length()
    {
        return (sbyte) Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public static sbyte Distance(CoordB left, CoordB right)
    {
        return (left - right).Length();
    }

    public double XYAngle()
    {
        return 1800 * Math.Atan2(Y, X) / Math.PI + 900;
    }

    /// <summary>
    /// Get the block coord, transform to hexa, reverse and then transform to long;
    /// Example: (-1, -1, 1);
    /// Reverse and transform to hexadecimal as string: '1FFFF';
    /// Convert the string above to long: 65535.
    /// </summary>
    public long AsHexadecimal()
    {
        string coordRevertedAsString = $"{Z:X2}{Y:X2}{X:X2}";
        return Convert.ToInt64(coordRevertedAsString, 16);
    }

    #region Overrides

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

    public override string ToString()
    {
        return $"CoordB({X}, {Y}, {Z})";
    }

    #endregion
}
