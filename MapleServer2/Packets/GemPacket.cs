using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GemPacket
{
    private enum Mode : byte
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

    public static PacketWriter EquipItem(int playerObjectId, Item item, int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Gem);

        pWriter.Write(Mode.EquipItem);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteByte((byte) index);
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter UnequipItem(int playerObjectId, GemSlot gemSlot)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Gem);

        pWriter.Write(Mode.UnequipItem);
        pWriter.WriteInt(playerObjectId);
        pWriter.Write(gemSlot);

        return pWriter;
    }

    public static PacketWriter EquipError(GemEquipError type)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Gem);

        pWriter.Write(Mode.EquipError);
        pWriter.WriteShort((short) type);

        return pWriter;
    }
}
