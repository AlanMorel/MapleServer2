using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LiftablePacket
{
    private enum LiftableMode : byte
    {
        UpdateEntity = 0x02,
        Drop = 0x03,
        RemoveCube = 0x04,
    }

    public static PacketWriter UpdateEntityById(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.UpdateEntity);
        pWriter.WriteString(liftableObject.EntityId);
        pWriter.WriteByte();
        pWriter.WriteInt(liftableObject.Enabled ? 1 : 0); // 1 = enable, 0 = disable
        pWriter.Write(liftableObject.State);

        return pWriter;
    }

    public static PacketWriter UpdateEntityByCoord(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.UpdateEntity);
        pWriter.WriteString($"4_{CoordB.AsHexadecimal(liftableObject.Position.ToByte())}");
        pWriter.WriteByte();
        pWriter.WriteInt(liftableObject.Enabled ? 1 : 0); // 1 = enable, 0 = disable
        pWriter.Write(liftableObject.State);

        return pWriter;
    }

    public static PacketWriter Drop(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.Drop);
        pWriter.WriteString($"4_{CoordB.AsHexadecimal(liftableObject.Position.ToByte())}");
        pWriter.WriteInt(1);
        pWriter.WriteUnicodeString(liftableObject.MaskQuestId);
        pWriter.WriteUnicodeString(liftableObject.MaskQuestState);
        pWriter.WriteUnicodeString("0");
        pWriter.WriteUnicodeString("0");
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter RemoveCube(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.RemoveCube);
        pWriter.WriteString($"4_{CoordB.AsHexadecimal(liftableObject.Position.ToByte())}");

        return pWriter;
    }
}
