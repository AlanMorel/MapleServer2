using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class EquipmentPacket
{
    public static PacketWriter EquipItem(IFieldObject<Player> player, Item item, ItemSlot equipSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_PUT_ON);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteUnicodeString(equipSlot.ToString());
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteByte();
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter UnequipItem(IFieldObject<Player> player, Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ITEM_PUT_OFF);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteLong(item.Uid);

        return pWriter;
    }
}
