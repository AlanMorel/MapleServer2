using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class CouponUsePacket
{
    private enum CouponUsePacketMode : byte
    {
        InventoryExpanded = 0x0,
        MaxInventorySize = 0x1,
        CharacterSlotAdded = 0x2,
        MaxCharacterSlots = 0x3,
        BeautyCoupon = 0x6
    }

    public static PacketWriter CharacterSlotAdded()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
        pWriter.Write(CouponUsePacketMode.CharacterSlotAdded);
        return pWriter;
    }

    public static PacketWriter MaxCharacterSlots()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
        pWriter.Write(CouponUsePacketMode.MaxCharacterSlots);
        return pWriter;
    }

    public static PacketWriter BeautyCoupon(IFieldObject<Player> player, long itemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.COUPON_USE);
        pWriter.Write(CouponUsePacketMode.BeautyCoupon);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteLong(itemUid);
        return pWriter;
    }
}
