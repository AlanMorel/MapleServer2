using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class CouponUsePacket
    {
        private enum CouponUsePacketMode : byte
        {
            InventoryExpanded = 0x0,
            MaxInventorySize = 0x1,
            CharacterSlotAdded = 0x2,
            MaxCharacterSlots = 0x3,
            BeautyCoupon = 0x6,
        }

        public static Packet CharacterSlotAdded()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
            pWriter.WriteEnum(CouponUsePacketMode.CharacterSlotAdded);
            return pWriter;
        }

        public static Packet MaxCharacterSlots()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
            pWriter.WriteEnum(CouponUsePacketMode.MaxCharacterSlots);
            return pWriter;
        }

        public static Packet BeautyCoupon(IFieldObject<Player> player, long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
            pWriter.WriteEnum(CouponUsePacketMode.BeautyCoupon);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(itemUid);
            return pWriter;
        }
    }
}
