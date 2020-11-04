using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using Maple2.Data.Types;
using MapleServer2.Types.FieldObjects;
using Maple2.Data.Types.Items;
using Maple2Storage.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class EquipmentPacket {
        public static Packet EquipItem(IFieldObject<Player> player, Item item, EquipSlot equipSlot) {
            return PacketWriter.Of(SendOp.ITEM_PUT_ON)
                .WriteInt(player.ObjectId)
                .WriteInt(item.Id)
                .WriteLong(item.Uid)
                .WriteUnicodeString(equipSlot.ToString())
                .WriteInt(item.Rarity)
                .WriteByte()
                .WriteItem(item);
        }

        public static Packet UnequipItem(IFieldObject<Player> player, Item item) {
            return PacketWriter.Of(SendOp.ITEM_PUT_OFF)
                .WriteInt(player.ObjectId)
                .WriteLong(item.Uid);
        }
    }
}
