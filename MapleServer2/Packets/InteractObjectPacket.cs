using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class InteractObjectPacket
{
    private enum Mode : byte
    {
        Update = 0x04,
        Use = 0x05,
        Set = 0x06,
        Load = 0x08,
        Add = 0x09,
        Remove = 0x0A,
        Interact = 0x0D
    }

    public static PacketWriter Update(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Update);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.State);
        pWriter.Write(interactObject.Type);

        return pWriter;
    }

    public static PacketWriter Use(InteractObject interactObject, short result = 0, int numDrops = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Use);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.Type);

        if (interactObject.Type == InteractObjectType.Gathering)
        {
            pWriter.WriteShort(result);
            pWriter.WriteInt(numDrops);
        }

        return pWriter;
    }

    public static PacketWriter Set(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Set);
        pWriter.WriteInt(interactObject.InteractId);
        pWriter.Write(interactObject.State);
        return pWriter;
    }

    public static PacketWriter LoadObjects(List<IFieldObject<InteractObject>> interactObjects)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Load);
        pWriter.WriteInt(interactObjects.Count);
        foreach (IFieldObject<InteractObject> interactObject in interactObjects)
        {
            WriteInteractObject(pWriter, interactObject.Value);
        }

        return pWriter;
    }

    public static PacketWriter Add(IFieldObject<InteractObject> fieldInteractObject)
    {
        InteractObject interactObject = fieldInteractObject.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Add);
        pWriter.WriteString(interactObject.Id);
        pWriter.Write(interactObject.State);
        pWriter.Write(interactObject.Type);
        pWriter.WriteInt(interactObject.InteractId);
        pWriter.Write(fieldInteractObject.Coord);
        pWriter.Write(fieldInteractObject.Rotation);
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

    public static PacketWriter Remove(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Remove);
        pWriter.WriteString(interactObject.Id);
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter Interact(InteractObject interactObject)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.InteractObject);
        pWriter.Write(Mode.Interact);
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
