using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GemPacket
    {
        private enum GemMode : byte
        {
            EquipItem = 0x00,
            UnequipItem = 0x01,
            EquipError = 0x04
        }

        public enum GemEquipError : short
        {
            InventoryFull,
            SlotNoLongerValid,
            ItemNotEligible,
            ItemExpired,
            ItemPCCafeOnly,
            ActivePremiumClubRequired
        }

        public static Packet EquipItem(GameSession session, Item item, int index)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

            pWriter.WriteEnum(GemMode.EquipItem);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteByte((byte) (index));
            ItemPacketHelper.WriteItem(pWriter, item);

            return pWriter;
        }

        public static Packet UnequipItem(GameSession session, GemSlot gemSlot)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

            pWriter.WriteEnum(GemMode.UnequipItem);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteEnum(gemSlot);

            return pWriter;
        }

        public static Packet EquipError(GemEquipError type)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

            pWriter.WriteEnum(GemMode.EquipError);
            pWriter.WriteShort((short) type);

            return pWriter;
        }
    }
}
