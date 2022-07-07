using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class LimitBreakPacket
{
    private enum Mode : byte
    {
        SelectedItem = 0x00,
        LimitBreakItem = 0x01,
        Notice = 0x02
    }

    public static PacketWriter SelectedItem(long itemUid, Item item, long mesoCost, List<EnchantIngredient> ingredients)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LimitBreak);
        pWriter.Write(Mode.SelectedItem);
        pWriter.WriteLong(itemUid);
        pWriter.WriteLong(mesoCost);
        pWriter.WriteByte((byte) ingredients.Count);

        foreach (EnchantIngredient ingredient in ingredients)
        {
            pWriter.WriteInt();
            pWriter.Write(ingredient.Tag);
            pWriter.WriteInt(ingredient.Amount);
        }

        pWriter.WriteItem(item);
        return pWriter;
    }

    public static PacketWriter LimitBreakItem(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LimitBreak);
        pWriter.Write(Mode.LimitBreakItem);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);
        return pWriter;
    }

    public static PacketWriter Notice(short noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LimitBreak);
        pWriter.Write(Mode.Notice);
        pWriter.WriteShort(noticeId);
        return pWriter;
    }
}
