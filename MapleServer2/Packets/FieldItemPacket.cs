using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldItemPacket
{
    public static PacketWriter AddItem(IFieldObject<Item> fieldItem)
    {
        Item item = fieldItem.Value;

        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddItem);
        pWriter.WriteInt(fieldItem.ObjectId);
        pWriter.WriteInt(item.Id);
        pWriter.WriteInt(item.Amount);

        pWriter.WriteBool(true);
        if (true)
        {
            pWriter.WriteLong(item.DropInformation.BoundToCharacterId);
        }

        pWriter.Write(fieldItem.Coord); // drop location
        pWriter.WriteInt(item.DropInformation.SourceObjectId);
        pWriter.WriteInt();
        pWriter.WriteByte(2);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteShort(1005);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter PickupItem(IFieldObject<Item> fieldItem, int receiverObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldPickupItem);
        pWriter.WriteByte(0x01);
        pWriter.WriteInt(fieldItem.ObjectId);
        pWriter.WriteInt(receiverObjectId);
        pWriter.WriteLong(fieldItem.Value.Amount);

        return pWriter;
    }

    public static PacketWriter RemoveItem(int objectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldRemoveItem);
        pWriter.WriteInt(objectId);

        return pWriter;
    }
}
