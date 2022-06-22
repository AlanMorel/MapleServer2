using Maple2Storage.Types;
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

    public static PacketWriter LoadLiftables(List<IFieldObject<LiftableObject>> liftableObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(LiftableMode.LoadLiftables);
        pWriter.Write(liftableObjects.Count);
        foreach (IFieldObject<LiftableObject> fieldLiftableObject in liftableObjects)
        {
            LiftableObject liftableObject = fieldLiftableObject.Value;

            pWriter.WriteString(liftableObject.EntityId);
            pWriter.WriteBool(liftableObject.Enabled);
            pWriter.Write(liftableObject.State);
            pWriter.WriteBool(liftableObject.PickedUp);
            pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestId);
            pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestState);
            pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestId);
            pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestState);
            pWriter.WriteByte(1);
        }

        return pWriter;
    }

    public static PacketWriter UpdateEntityById(LiftableObject liftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(LiftableMode.UpdateEntity);
        pWriter.WriteString(liftableObject.EntityId);
        pWriter.WriteBool(liftableObject.Enabled);
        pWriter.Write(liftableObject.State);
        pWriter.WriteBool(liftableObject.PickedUp);

        return pWriter;
    }

    public static PacketWriter UpdateEntityByCoord(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        LiftableObject liftableObject = fieldLiftableObject.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(LiftableMode.UpdateEntity);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");
        pWriter.WriteBool(liftableObject.Enabled);
        pWriter.Write(liftableObject.State);
        pWriter.WriteBool(liftableObject.PickedUp);

        return pWriter;
    }

    public static PacketWriter Drop(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        LiftableObject liftableObject = fieldLiftableObject.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(LiftableMode.Drop);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");
        pWriter.Write(liftableObject.State);
        pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestId);
        pWriter.WriteUnicodeString(liftableObject.Metadata.MaskQuestState);
        pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestId);
        pWriter.WriteUnicodeString(liftableObject.Metadata.EffectQuestState);
        pWriter.WriteByte(1);

        return pWriter;
    }

    public static PacketWriter RemoveCube(IFieldObject<LiftableObject> fieldLiftableObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Liftable);
        pWriter.Write(LiftableMode.RemoveCube);
        pWriter.WriteString($"4_{fieldLiftableObject.Coord.ToByte().AsHexadecimal()}");

        return pWriter;
    }
}
