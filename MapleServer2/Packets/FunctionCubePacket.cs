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
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FunctionCube);

        packetWriter.Write(FunctionCubeMode.SendCubes);
        packetWriter.WriteInt(cubes.Count);
        foreach (Cube cube in cubes)
        {
            packetWriter.WriteUnicodeString($"4_{cube.CoordF.ToByte().AsHexadecimal()}");
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
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FunctionCube);

        packetWriter.Write(FunctionCubeMode.Add);
        packetWriter.WriteUnicodeString($"4_{coordB.AsHexadecimal()}");
        packetWriter.WriteInt(status);
        packetWriter.WriteByte(unkByte);

        return packetWriter;
    }

    public static PacketWriter UseFurniture(long characterId, CoordB coordB, bool inUse)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FunctionCube);

        packetWriter.Write(FunctionCubeMode.Furniture);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{coordB.AsHexadecimal()}");
        packetWriter.WriteBool(inUse);

        return packetWriter;
    }

    public static PacketWriter SuccessLifeSkill(long characterId, CoordB coordB, int status)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FunctionCube);

        packetWriter.Write(FunctionCubeMode.SuccessLifeSkill);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{coordB.AsHexadecimal()}");
        packetWriter.WriteLong(TimeInfo.Now());
        packetWriter.WriteInt(status);

        return packetWriter;
    }

    public static PacketWriter FailLikeSkill(long characterId, CoordB coordB)
    {
        PacketWriter packetWriter = PacketWriter.Of(SendOp.FunctionCube);

        packetWriter.Write(FunctionCubeMode.FailLifeSkill);
        packetWriter.WriteLong(characterId);
        packetWriter.WriteUnicodeString($"4_{coordB.AsHexadecimal()}");
        packetWriter.WriteLong(TimeInfo.Now());

        return packetWriter;
    }
}
