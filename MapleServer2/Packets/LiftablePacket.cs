using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LiftablePacket
{
    private enum LiftableMode : byte
    {
        LoadLiftables = 0x00,
        UpdateEntity = 0x02,
        Drop = 0x03,
        RemoveCube = 0x04
    }

    public static PacketWriter LoadLiftables(List<LiftableObject> liftableObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.LoadLiftables);
        pWriter.Write(liftableObjects.Count);
        foreach (LiftableObject liftableObject in liftableObjects)
        {
            pWriter.WriteString(liftableObject.EntityId);
            pWriter.WriteByte(1);
            pWriter.WriteInt(liftableObject.Enabled ? 1 : 0); // 1 = enable, 0 = disable
            pWriter.Write(liftableObject.State);
            pWriter.WriteUnicodeString("0"); // unknown
            pWriter.WriteUnicodeString(); // ""
            pWriter.WriteUnicodeString(liftableObject.EffectQuestId);
            pWriter.WriteUnicodeString(liftableObject.EffectQuestState);
            pWriter.WriteByte(1);
        }

        return pWriter;
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
        pWriter.WriteString($"4_{liftableObject.Position.ToByte().AsHexadecimal()}");
        pWriter.WriteByte();
        pWriter.WriteInt(liftableObject.Enabled ? 1 : 0); // 1 = enable, 0 = disable
        pWriter.Write(liftableObject.State);

        return pWriter;
    }

    public static PacketWriter Drop(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.Drop);
        pWriter.WriteString($"4_{liftableObject.Position.ToByte().AsHexadecimal()}");
        pWriter.WriteInt(1);
        pWriter.WriteUnicodeString(liftableObject.EffectQuestId);
        pWriter.WriteUnicodeString(liftableObject.EffectQuestState);
        pWriter.WriteUnicodeString("0");
        pWriter.WriteUnicodeString("0");
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter RemoveCube(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LIFTABLE);
        pWriter.Write(LiftableMode.RemoveCube);
        pWriter.WriteString($"4_{liftableObject.Position.ToByte().AsHexadecimal()}");

        return pWriter;
    }
}
