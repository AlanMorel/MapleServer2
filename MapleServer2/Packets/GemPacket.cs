using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets;

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

    public static PacketWriter EquipItem(GameSession session, Item item, int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

        pWriter.Write(GemMode.EquipItem);
        pWriter.WriteInt(session.Player.FieldPlayer.ObjectId);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteByte((byte) index);
        ItemPacketHelper.WriteItem(pWriter, item);

        return pWriter;
    }

    public static PacketWriter UnequipItem(GameSession session, GemSlot gemSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

        pWriter.Write(GemMode.UnequipItem);
        pWriter.WriteInt(session.Player.FieldPlayer.ObjectId);
        pWriter.Write(gemSlot);

        return pWriter;
    }

    public static PacketWriter EquipError(GemEquipError type)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GEM);

        pWriter.Write(GemMode.EquipError);
        pWriter.WriteShort((short) type);

        return pWriter;
    }
}
