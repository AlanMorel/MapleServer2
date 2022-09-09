using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LiftablePacket
{
    private enum Mode : byte
    {
        LoadLiftables = 0x00,
        UpdateEntity = 0x02,
        Drop = 0x03,
        RemoveCube = 0x04
    }

    public static PacketWriter LoadLiftables(List<IFieldObject<LiftableObject>> liftableObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(Mode.LoadLiftables);
        pWriter.Write(liftableObjects.Count);
        foreach (IFieldObject<LiftableObject> fieldLiftableObject in liftableObjects)
        {
            LiftableObject liftableObject = fieldLiftableObject.Value;

            pWriter.WriteString(liftableObject.EntityId);
            pWriter.WriteByte(); // not sure ?
            pWriter.WriteInt(liftableObject.ItemCount);
            pWriter.Write(liftableObject.State);
            pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestId);
            pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestState);
            pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestId);
            pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestState);
            pWriter.WriteBool(true); // effect
        }

        return pWriter;
    }

    public static PacketWriter UpdateEntityById(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(Mode.UpdateEntity);
        pWriter.WriteString(liftableObject.EntityId);
        pWriter.WriteByte();
        pWriter.WriteInt(liftableObject.ItemCount);
        pWriter.Write(liftableObject.State);

        return pWriter;
    }

    public static PacketWriter UpdateEntityByCoord(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        LiftableObject liftableObject = fieldLiftableObject.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(Mode.UpdateEntity);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");
        pWriter.WriteByte();
        pWriter.WriteInt(liftableObject.ItemCount);
        pWriter.Write(liftableObject.State);

        return pWriter;
    }

    public static PacketWriter Drop(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        LiftableObject liftableObject = fieldLiftableObject.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(Mode.Drop);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");
        pWriter.WriteInt(liftableObject.ItemCount);
        pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestId);
        pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestState);
        pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestId);
        pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestState);
        pWriter.WriteBool(true); // effect

        return pWriter;
    }

    public static PacketWriter RemoveCube(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(Mode.RemoveCube);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");

        return pWriter;
    }
}
