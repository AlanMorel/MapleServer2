using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class InteractObjectPacket
{
    private enum InteractObjectMode : byte
    {
        QuestUse = 0x04,
        Use = 0x05,
        SetInteractObject = 0x06,
        LoadInteractObject = 0x08,
        AddInteractObject = 0x09,
        RemoveInteractObject = 0x0A,
        Interact = 0x0D
    }

    public static PacketWriter QuestUse(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.QuestUse);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.State);
        pWriter.Write(interactObject.Type);

        return pWriter;
    }

    public static PacketWriter Use(InteractObject interactObject, short result = 0, int numDrops = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.Use);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.Type);

        if (interactObject.Type == InteractObjectType.Gathering)
        {
            pWriter.WriteShort(result);
            pWriter.WriteInt(numDrops);
        }

        return pWriter;
    }

    public static PacketWriter SetInteractObject(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.SetInteractObject);
        pWriter.WriteInt(interactObject.InteractId);
        pWriter.Write(interactObject.State);
        return pWriter;
    }

    public static PacketWriter LoadInteractObject(List<InteractObject> interactObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.LoadInteractObject);
        pWriter.WriteInt(interactObjects.Count);
        foreach (InteractObject interactObject in interactObjects)
        {
            WriteInteractObject(pWriter, interactObject);
        }

        return pWriter;
    }

    public static PacketWriter AddInteractObject(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.AddInteractObject);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.State);
        pWriter.Write(interactObject.Type);
        pWriter.WriteInt(interactObject.InteractId);
        pWriter.Write(interactObject.Position);
        pWriter.Write(interactObject.Rotation);
        pWriter.WriteUnicodeString(interactObject.Model);
        pWriter.WriteUnicodeString(interactObject.Asset);
        pWriter.WriteUnicodeString(interactObject.NormalState);
        pWriter.WriteUnicodeString(interactObject.Reactable);
        pWriter.WriteFloat(interactObject.Scale);
        pWriter.WriteByte();
        if (interactObject is AdBalloon adBalloon)
        {
            pWriter.WriteLong(adBalloon.Owner.CharacterId);
            pWriter.WriteUnicodeString(adBalloon.Owner.Name);
        }

        return pWriter;
    }

    public static PacketWriter RemoveInteractObject(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.RemoveInteractObject);
        pWriter.WriteString(interactObject.Id);
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter Interact(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
        pWriter.Write(InteractObjectMode.Interact);
        pWriter.WriteByte();
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.Type);
        return pWriter;
    }

    private static void WriteInteractObject(PacketWriter pWriter, InteractObject interactObject)
    {
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.State);
        pWriter.Write(interactObject.Type);
        if (interactObject.Type == InteractObjectType.Gathering)
        {
            pWriter.WriteInt();
        }
    }
}
