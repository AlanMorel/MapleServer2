using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldItemPacket
{
    public static PacketWriter AddItem(IFieldObject<Item> item, int userObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddItem);
        pWriter.Write(item.ObjectId); // object id
        pWriter.Write(item.Value.Id);
        pWriter.Write(item.Value.Amount);

        bool flag = true;
        pWriter.WriteBool(flag);
        if (flag)
        {
            pWriter.WriteLong();
        }

        pWriter.Write(item.Coord); // drop location
        pWriter.WriteInt(userObjectId);
        pWriter.WriteInt();
        pWriter.WriteByte(2);
        pWriter.WriteInt(item.Value.Rarity);
        pWriter.WriteShort(1005);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteItem(item.Value);

        return pWriter;
    }

    public static PacketWriter AddItem(IFieldObject<Item> item, IFieldObject<NpcMetadata> sourceMob, IFieldObject<Player> targetPlayer)
    {
        // Works for meso

        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddItem);
        pWriter.WriteInt(item.ObjectId);
        pWriter.WriteInt(item.Value.Id);
        pWriter.WriteInt(item.Value.Amount);

        pWriter.WriteByte(1); // Unknown (GMS2) (character lock flag?)
        pWriter.WriteLong(targetPlayer.Value.CharacterId); // Lock drop to character

        pWriter.Write(item.Coord);
        pWriter.WriteInt(sourceMob.ObjectId);
        pWriter.WriteInt(); // Unknown (GMS2)
        pWriter.WriteByte();
        pWriter.WriteInt(item.Value.Rarity);
        pWriter.WriteInt(21);

        if (item.Value.Id >= 90000004 && item.Value.Id <= 90000011)
        {
            // Extra for special items
            pWriter.WriteInt(1); // 0 = SP/EP, 1 = quest item?
            pWriter.WriteInt();
            pWriter.WriteInt(-1);
            pWriter.WriteInt(targetPlayer.ObjectId); // Unknown
            for (int i = 0; i < 14; i++)
            {
                pWriter.WriteInt();
            }
            pWriter.WriteInt(-1);
            for (int i = 0; i < 24; i++)
            {
                pWriter.WriteInt();
            }
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(6);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
        }
        //pWriter.Write(sourceMob.Coord);
        //pWriter.WriteItem(item.Value);

        return pWriter;
    }

    public static PacketWriter PickupItem(int objectId, Item item, int userObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldPickupItem);
        pWriter.WriteByte(0x01);
        pWriter.WriteInt(objectId);
        pWriter.WriteInt(userObjectId);
        pWriter.WriteLong(item.Amount); // Amount (GUI)

        return pWriter;
    }

    public static PacketWriter RemoveItem(int objectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldRemoveItem);
        pWriter.WriteInt(objectId);

        return pWriter;
    }
}
