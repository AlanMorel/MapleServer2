using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public class FunctionCubePacket
{
    private enum FunctionCubeMode : byte
    {
        SendCubes = 0x02,
        Add = 0x03,
        Furniture = 0x05,
        SuccessLifeSkill = 0x08,
        FailLifeSkill = 0x09
    }

    public static PacketWriter SendCubes(List<Cube> cubes)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FUNCTION_CUBE);

        packetWriter.Write(FunctionCubeMode.SendCubes);
        packetWriter.WriteInt(cubes.Count);
        foreach (Cube cube in cubes)
        {
            packetWriter.WriteUnicodeString($"4_{CoordBAsHexadimal(cube.CoordF.ToByte())}");
            switch (cube.Item.HousingCategory)
            {
                case ItemHousingCategory.Farming:
                case ItemHousingCategory.Ranching:
                    packetWriter.WriteInt(1);
                    break;
                default:
                    packetWriter.WriteInt();
                    break;
            }
            packetWriter.WriteByte();
        }

        return packetWriter;
    }

    public static PacketWriter UpdateFunctionCube(CoordB coordB, int status, byte unkByte)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FUNCTION_CUBE);

        packetWriter.Write(FunctionCubeMode.Add);
        packetWriter.WriteUnicodeString($"4_{CoordBAsHexadimal(coordB)}");
        packetWriter.WriteInt(status);
        packetWriter.WriteByte(unkByte);

        return packetWriter;
    }

    public static PacketWriter UseFurniture(long characterId, CoordB coordB, bool inUse)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FUNCTION_CUBE);

        packetWriter.Write(FunctionCubeMode.Furniture);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{CoordBAsHexadimal(coordB)}");
        packetWriter.WriteBool(inUse);

        return packetWriter;
    }

    public static PacketWriter SuccessLifeSkill(long characterId, CoordB coordB, int status)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FUNCTION_CUBE);

        packetWriter.Write(FunctionCubeMode.SuccessLifeSkill);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{CoordBAsHexadimal(coordB)}");
        packetWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds());
        packetWriter.WriteInt(status);

        return packetWriter;
    }

    public static PacketWriter FailLikeSkill(long characterId, CoordB coordB)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FUNCTION_CUBE);

        packetWriter.Write(FunctionCubeMode.FailLifeSkill);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{CoordBAsHexadimal(coordB)}");
        packetWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds());

        return packetWriter;
    }

    private static long CoordBAsHexadimal(CoordB coordB)
    {
        /// Get the block coord, transform to hexa, reverse and then transform to long
        /// Example: (-1, -1, 1)
        /// Reverse and transform to hexadecimal as string: '1FFFF'
        /// Convert the string above to long: 65535
        byte[] coords = coordB.ToArray();
        string coordRevertedAsString = $"{coords[2]}{coords[1]:X2}{coords[0]:X2}";
        return Convert.ToInt64(coordRevertedAsString, 16);
    }
}
